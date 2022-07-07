using Microsoft.EntityFrameworkCore;
using NLog.Web;
using templater;
using templater.Classes;
using templater.Model;
using templater.Services;

// Установка лицензии Aspose
new Aspose.Cells.License().SetLicense("aspose.lic");
new Aspose.Words.License().SetLicense("aspose.lic");

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.WebHost.UseNLog();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<Context>();
builder.Services.AddHostedService<CleanupService>();

builder.Services.AddTransient<DocxFiller>();
builder.Services.AddTransient<XlsxFiller>();
builder.Services.AddTransient<Filler>();
builder.Services.AddTransient<DefaultReplacements>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// авто-миграция БД
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Context>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка создания/обновления БД.");
    }
}

app.Run();
