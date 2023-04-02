using Microsoft.AspNetCore.Mvc;

namespace _4._0DependencyInjection.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ISampleSingleton _sampleSingleton;
    private readonly ISampleScoped _sampleScoped;
    private readonly ISampleTransient _sampleTransient;

    public WeatherForecastController(ISampleSingleton sampleSingleton, ISampleScoped sampleScoped, ISampleTransient sampleTransient)
    {
        _sampleScoped = sampleScoped;
        _sampleSingleton = sampleSingleton;
        _sampleTransient = sampleTransient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public string Get()
    {
      Console.Out.WriteLine($@"
          name:sampleScoped,Id:{this._sampleScoped.Id},hasCode:{this._sampleScoped.GetHashCode()}
          name:sampleTransient,Id:{this._sampleTransient.Id},hashCode:{this._sampleTransient.GetHashCode()}
          name:sampleSingleton,Id:{this._sampleSingleton.Id},hashCode:{this._sampleSingleton.GetHashCode()}
      ");
      return "123";
    }
}
