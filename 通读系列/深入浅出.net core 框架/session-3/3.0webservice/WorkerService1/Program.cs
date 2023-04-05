using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

;



class TestItem
{
    public string? a { get; set; }


    public object this[object key]
    {
        get
        {
            return "a";
        }
    }

}
