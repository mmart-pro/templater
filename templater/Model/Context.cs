using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using templater.Classes;

namespace templater.Model;

public class Context : DbContext
{
    /// <summary>
    /// Готовые документы
    /// </summary>
    public DbSet<Document> Documents { get; set; }

    public DbSet<TemplateApp> TemplateApps { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateFormat> TemplateFormats { get; set; }

    private readonly AppSettings _settings;

    public Context(
        IOptions<AppSettings> settings
        )
    {
        _settings = settings.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TemplateFormat>()
            .HasData(new TemplateFormat[]{
                new ()
                {
                    Id = (int)EnumTemplateFormats.XLSX,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                },
                new ()
                {
                    Id = (int)EnumTemplateFormats.DOCX,
                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                }
            });

        modelBuilder.Entity<Template>()
            .HasIndex(x => new { x.TemplateAppId, x.ApiRef })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={_settings.DB_PATH}");
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetTimeStamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimeStamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        SetTimeStamps();
        return base.SaveChanges();
    }

    private void SetTimeStamps()
    {
        var timeStamp = DateTime.Now;
        // заносим изменённые данные в журнал
        var modifiedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified || p.State == EntityState.Added).ToList();
        foreach (var entry in modifiedEntities)
        {
            var type = entry.Entity.GetType();

            var timeStampProp = type.GetProperty("timestamp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (timeStampProp != null)
                timeStampProp.SetValue(entry.Entity, timeStamp);

            if (entry.State == EntityState.Added)
            {
                var createTimeStampProp = type.GetProperty("createTimeStamp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (createTimeStampProp != null)
                    createTimeStampProp.SetValue(entry.Entity, timeStamp);
            }
        }
    }
}
