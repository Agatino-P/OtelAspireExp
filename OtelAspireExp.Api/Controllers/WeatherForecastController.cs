using Microsoft.AspNetCore.Mvc;

namespace OtelAspireExp.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{


    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    private readonly OtelAspireExtMetrics _otelAspireExtMetrics;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(OtelAspireExtMetrics otelAspireExtMetrics, ILogger<WeatherForecastController> logger)
    {
        _otelAspireExtMetrics = otelAspireExtMetrics;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {

        using var _ = _otelAspireExtMetrics.MeasureRequestDuration();
        try
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        }
        finally 
        {
            _otelAspireExtMetrics.IncreaseOtelAspireExpRequestCount();
        }
    }
}
