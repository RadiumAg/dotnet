var logger = new ServiceCollection().AddLogging(bulder => bulder.AddConsole())
.BuildServiceProvider()
.GetRequiredService<ILoggerFactory>()
.CreateLogger<Program>();


logger.LogInformation("This information log from Program");
logger.LogInformation("This error log from  Program");
logger.LogInformation("This critical log  from Program");
logger.LogInformation("This warning log from Program");


logger.LogInformation(ApplicaitonEvents.Create,"创建订单");
logger.LogInformation(ApplicationEvents.Create,"创建订单");



var levels = Enum.GetValues<LogLevel>()
.Where(l => l != LogLevel.None);

var eventId = 1;

foreach (var level in levels)
{
    logger.Log(level, eventId++, "LogLevel:{0}", level);
}

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

internal static class ApplicationEvents {
  internal const int Create = 1000;
  internal const int Read = 1001;
  internal const int Update = 1002;
  internal const int Delete = 1003;


}
