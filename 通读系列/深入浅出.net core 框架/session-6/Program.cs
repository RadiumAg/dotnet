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

// var host = new HostBuilder().ConfigureLogging(logging =>
// {
//     logging.AddConsole();
// }).ConfigureServices((hostContext, services) =>
// {
//     services.AddHostedService<MyBackgroundService>();
// });


// await host.RunConsoleAsync();

a c = new b();

Console.Out.WriteLine(c.getA());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();





class b : a
{
    public new string name = "b";
    public override string getA()
    {
        return this.name;
    }
}


class a
{
    public  string name = "a";
    public virtual string getA()
    {
        return this.name;
    }
}




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


public abstract class MyBackgroundService : IHostedService, IDisposable
{
    private Task? _executeTask;
    private CancellationTokenSource? _stoppingCts;
    public virtual Task? ExecuteTask => _executeTask;

    protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

    private readonly ILogger<MyBackgroundService>? _logger;

    public MyBackgroundService(ILogger<MyBackgroundService> logger)
    {
        _logger = logger;
    }

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _executeTask = ExecuteAsync(_stoppingCts.Token);

        if (_executeTask.IsCompleted)
        {
            return _executeTask;
        }

        return Task.CompletedTask;
    }

    private void DoWork()
    {
        _logger?.LogInformation($"Hello World! - {DateTime.Now}");
    }



    public void Dispose()
    {
        _stoppingCts?.Cancel();
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_executeTask == null)
        {
            return;
        }
        try
        {
            _stoppingCts?.Cancel();
        }
        finally
        {
            await Task.WhenAny(_executeTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
        }
    }
}

