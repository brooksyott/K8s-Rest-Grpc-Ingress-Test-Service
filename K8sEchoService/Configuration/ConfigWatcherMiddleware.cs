using K8sEchoService.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class ConfigWatcherMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ConfigWatcherMiddleware> _logger;
    private readonly FileSystemWatcher _fileWatcher;

    public ConfigWatcherMiddleware(RequestDelegate next, ILogger<ConfigWatcherMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        // Path to monitor
        string filePath = GlobalConfig.GetConfigFile();
        string directory = Path.GetDirectoryName(filePath);
        string fileName = Path.GetFileName(filePath);

        // Set up FileSystemWatcher to monitor the file
        _fileWatcher = new FileSystemWatcher(directory, fileName)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Size
        };

        // Subscribe to file change events
        _fileWatcher.Changed += OnFileChanged;
        _fileWatcher.Created += OnFileChanged;
        _fileWatcher.Deleted += OnFileChanged;
        _fileWatcher.Renamed += OnFileChanged;

        // Start monitoring
        _fileWatcher.EnableRaisingEvents = true;

        bool loadConfigSuccess = GlobalConfig.LoadConfig();
        _logger.LogInformation($"GlobalConfig loaded: {loadConfigSuccess}");

        _logger.LogInformation($"GlobalConfig: Started monitoring file: {filePath}");
    }

    // This method is invoked when the file is changed
    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        // Log when the file has been modified, created, or deleted
        _logger.LogInformation($"GlobalConfig: File '{e.FullPath}' has been {e.ChangeType}");

        bool loadConfigSuccess = GlobalConfig.LoadConfig();
        _logger.LogInformation($"GlobalConfig: Config loaded: {loadConfigSuccess}");


    }

    public async Task Invoke(HttpContext context)
    {
        // Continue to the next middleware
        await _next(context);
    }
}
