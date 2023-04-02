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