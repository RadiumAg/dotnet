using Microsoft.AspNetCore.Builder;

namespace _3._0winform;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        app.MapGet("/", () => "Hello World!");
        app.RunAsync();

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        var urls = app.Urls;
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1(string.Join(",", urls)));
    }
}