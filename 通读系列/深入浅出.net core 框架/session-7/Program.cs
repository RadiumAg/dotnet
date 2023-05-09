using Newtonsoft.Json;

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

app.MapGet("/test", http =>
{
    http.Response.Headers.Add("Content-Disposition", "attachment; filename=_________.xlsx; filename*=UTF-8''%E4%BE%9B%E5%BA%94%E5%95%86%E6%A1%A3%E6%A1%88%E5%AF%BC%E5%85%A5%E6%A8%A1%E6%9D%BF.xlsx");
    return Task.CompletedTask;
});

// app.UseCustomExtension();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public class CustomMiddleware
{
    private readonly RequestDelegate? _next;
    private readonly ILogger<CustomMiddleware>? _logger;

    public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger,IHttpContextAccessor httpContextAccessor)
    {
        this._next = next;
        this._logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalResponseBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;
        if (_next != null)
        {
            await _next(context);
        }

        context.Response.Body = originalResponseBodyStream;
        memoryStream.Seek(0, SeekOrigin.Begin);

        var readToEnd = await new StreamReader(memoryStream).ReadToEndAsync();
        var objResult = JsonConvert.SerializeObject(readToEnd);
        var result = CustomApiResponse.Create(context.Response.StatusCode, objResult, context.TraceIdentifier);
        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
}

// public static class MiddlewareExtension
// {
//     public static void UseCustomExtension(this IApplicationBuilder app)
//     {
//         app.UseMiddleware<CustomMiddleware>();
//     }
// }


public class CustomApiResponse
{
    internal CustomApiResponse(int statusCode, object result, string requestId)
    {
        Status = statusCode;
        RequestId = requestId;
        Result = result;
    }

    public static CustomApiResponse Create(int statusCode, object result, string requestId)
    {
        return new CustomApiResponse(statusCode, result, requestId);
    }


    public int Status { get; set; }
    public string RequestId { get; set; }
    public object Result { get; set;}
}

