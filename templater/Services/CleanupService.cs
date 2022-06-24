using templater.Model;

namespace templater.Services;

/// <summary>
/// Сервис очистки базы и удаления файлов
/// </summary>
public class CleanupService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CleanupService> _logger;

    private Timer _timer;
    private const int _timerPeriod = 1000 * 60 * 60 * 24;    // раз в 24 часа

    public CleanupService(
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration configuration,
        ILogger<CleanupService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Запуск сервиса очистки БД и файлов...");
        var dueTime = 5 * 1000;  // запуск через 5 секунд после старта приложения
        _timer = new Timer(DoWork, null, dueTime, Timeout.Infinite);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Остановка сервиса очистки БД и файлов...");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _timer.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void DoWork(object? state)
    {
        try
        {
            _logger.LogDebug("Очистка БД...");

            // считать текущие настройки
            var appSettings = _configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            using var scope = _serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetService<Context>();

            if (context == null)
                throw new Exception("Не удалось создать контекст БД");

            // удаление старых документов
            var docMaxDate = DateTime.Now.AddDays(-appSettings.DOC_DAYS_KEEP);
            context.Documents.RemoveRange(context.Documents.Where(d => d.CreateTimeStamp < docMaxDate));
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при очистке БД");
        }

        // следующее срабатывание таймера
        _timer.Change(_timerPeriod, Timeout.Infinite);
    }
}
