using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Lesson2WebApi.Controllers;

[ApiController]
[Route("weather")] // POST weather/post POST weather/post-1 POST weather/cancel
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost("post")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        throw new Exception();
        Task.Delay(10000).Wait();
        //
        // var httpClient = new HttpClient();
        // httpClient.BaseAddress = new Uri("http://localhost:5175");
        // var responseGet = await httpClient.PostAsync("weather/post-1", null);
        // var responseBodyAsString = await responseGet.Content.ReadAsStringAsync();
        // var jsonContent = Parse("application/json", responseBodyAsString);
        
        Console.WriteLine("Request");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpPost("post-1")]
    public IEnumerable<WeatherForecast> GetOne()
    {
        Task.Delay(3000).Wait();
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    
    [HttpPost("cancel")]
    public async Task<IEnumerable<WeatherForecast>> GetOne(CancellationToken cancellationToken)
    {
        await Task.Delay(4000, cancellationToken);
        
        // // уже не надо)
        // for (int i = 0; i < 100000; i++)
        // {
        //     var g = 1;
        // }
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    private static WeatherForecast[] Parse(string contentType, string body)
    {
        if (contentType == "application/json")
        {
            var jsonContent = JsonSerializer.Deserialize<WeatherForecast[]>(body);
            return jsonContent;
        }

        throw new Exception();
    }
}
