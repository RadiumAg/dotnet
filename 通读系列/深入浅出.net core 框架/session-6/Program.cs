var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var host = new HostBuilder().ConfigureLogging(logging =>
{
    logging.AddConsole();
}).ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<MyBackgroundService>();
});


await host.RunConsoleAsync();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



public class MyHostedService : IHostedService
{
    private readonly ILogger<MyHostedService>? _logger;
    private const int WAITTIME = 5000; //定义等待时间

    public MyHostedService(ILogger<MyHostedService> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger?.LogInformation("Staring IHostService registered in Startup");

        while (true)
        {
            await Task.Delay(WAITTIME);
            DoWork();
        }
    }

    private void DoWork()
    {
        _logger?.LogInformation($"Hello World - {DateTime.Now}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger?.LogInformation("Stopping IHostedService registered in Startup");

        return Task.CompletedTask;
    }
}


public class MyBackgroundService : BackgroundService
{
    private readonly ILogger<MyBackgroundService>? _logger;

    public MyBackgroundService(ILogger<MyBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger?.LogInformation("Starting IHostedService registered in Startup");
        while (true)
        {
            DoWork();
            await Task.Delay(5000);
        }

    }

    private void DoWork()
    {
        _logger?.LogInformation($"Hello World! - {DateTime.Now}");
    }
}

