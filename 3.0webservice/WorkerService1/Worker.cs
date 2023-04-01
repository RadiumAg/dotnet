using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging.EventLog;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }


    
    public static class WindowServiceLifetimeHostBuilderExtensions
    {
        public static IHostBuilder UseWindowsService(this IHostBuilder hostBuilder)
        {
            return UseWindowsService(hostBuilder, _ => { });
        }

        public static IHostBuilder UseWindowsService(this IHostBuilder hostBuilder, Action<WindowsServiceLifetimeOptions> configure)
        {
            if (WindowsServiceHelpers.IsWindowsService())
            {
                hostBuilder.UseContentRoot(AppContext.BaseDirectory);
                hostBuilder.ConfigureServices((hostContext, services) =>
                {
                    Debug.Assert(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
                    services.AddSingleton<IHostLifetime, WindowsServiceLifetime>();
                    services.Configure<EventLogSettings>(settings =>
                    {
                        Debug.Assert(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
                        if (string.IsNullOrEmpty(settings.SourceName))
                        {
                            settings.SourceName = hostContext.HostingEnvironment.ApplicationName;
                        }
                    });
                    services.Configure(configure);
                });
            }
            return hostBuilder;
        }
    }
}