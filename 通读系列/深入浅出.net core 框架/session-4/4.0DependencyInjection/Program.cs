var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISampleTransient,Sample>();
builder.Services.AddScoped<ISampleScoped,Sample>();
builder.Services.AddSingleton<ISampleSingleton,Sample>();
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



public interface ISample
{
    int Id { get; }
}

public interface ISampleSingleton : ISample
{

}

public interface ISampleScoped : ISample { }

public interface ISampleTransient : ISample { }

public class Sample : ISampleSingleton, ISampleScoped, ISampleTransient
{
    private static int counter;

    private int id;

    public Sample()
    {
        id = ++counter;
    }

    public int Id => id;
}