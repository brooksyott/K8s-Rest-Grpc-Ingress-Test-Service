using System;
using System.Threading;
using System.Threading.Tasks;
using K8sEchoService.Configuration;
using Microsoft.Extensions.Hosting;



public class ConfigChangeService : BackgroundService
{
    private const int Interval = 5000; // 1 second
    private readonly ILogger<ConfigChangeService> _logger;

    public ConfigChangeService(ILogger<ConfigChangeService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConfigChangeService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Your logic here (run every second)
            if (GlobalConfig.HasChanged())
            {
                _logger.LogInformation("ConfigChangeService: Config has changed, reloading");
                GlobalConfig.LoadConfig();
            }

            await Task.Delay(Interval, stoppingToken); // Wait for 1 second
        }

        _logger.LogInformation("ConfigChangeService is stopping.");
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MyTimedService is stopping.");
        await base.StopAsync(stoppingToken);
    }
}