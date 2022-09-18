using System.Net;
using System.Text.Json;
using Polly;
using Polly.CircuitBreaker;

namespace Lesson2;

public class Polly
{
    private readonly HttpClient _httpClient;

    public Polly()
    {
        _httpClient = new HttpClient();
    }
    
    public async Task MainAsync()
    {
        var result = await Policy
            // 1. указать, какое исключение обрабатывать
            .Handle<Exception>()
            // или укажите, какой тип ошибки должен быть обработан
            .OrResult<Task<HttpResponseMessage>>(r =>
            {
                return r.Result.StatusCode == HttpStatusCode.InternalServerError;
            })
            // 2. Укажите количество повторов и стратегию повторов
            .Retry(3, (exception, retryCount, context) =>
            {
                Console.WriteLine($"Начало {retryCount}  Попытки повтора: ");
            })
            // 3. Выполнение конкретных задач
            .Execute(ExecuteMockRequest);
        
        Console.WriteLine();
        
        // .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)).ExecuteAsync(ExecuteMockRequest) - перезапрос через промежуток времени
    }
    
    private async Task<HttpResponseMessage> ExecuteMockRequest()
    {
        var responseGet = await _httpClient.PostAsync("http://localhost:5175/weather/post", null);
        return responseGet;
    }
}