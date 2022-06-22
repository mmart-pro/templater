using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace templater.Model;

public class Context : DbContext
{
    /// <summary>
    /// Готовые документы
    /// </summary>
    public DbSet<Document> Documents { get; set; }

    public DbSet<TemplateApp> TemplateApps { get; set; }

    /// <summary>
    /// Шаблоны документов
    /// </summary>
    public DbSet<Template> Templates { get; set; }

    private readonly AppSettings _settings;

    public Context(
        IOptions<AppSettings> settings
        )
    {
        _settings = settings.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Template>()
            .HasKey(x => new { x.TemplateAppId, x.Id });

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={_settings.DB_PATH}");
    }
}
