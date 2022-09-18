using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Lesson2.Dto;

namespace Lesson2;

// TODO попробуем вызвать в цикле 10 000 http запросов, что будет? netstat -an
public static class ExampleRequest
{
    public static async Task MainAsync()
    { 
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:5175");

        // запросы в рамках роазных соединений!
        // Connection: keep-alive / close 
        httpClient.DefaultRequestHeaders.ConnectionClose = true;
        
        // почему вызываем запрос по http и получаем ошибку с сертификатом?
        // убираем UseHttpsRedirection - получаем 405, потому что надо POST
        var sw = Stopwatch.StartNew();
        Console.WriteLine("Starting request");
        var requestList = new List<Task<HttpResponseMessage>>(1000);
        for (int i = 0; i < 1000; i++)
        {
            var responseGet = httpClient.PostAsync("weather/post", null); // Task.Start()
            requestList.Add(responseGet);
        } // 1000 http запросов в рамках 1 TCP 

        // когда мы дошли до сюда у нас уже выполнились все запросы
        // здесь мы когда попадаем в WhenAll, он смотрит, каждый Task, а не стоит ли он уже в статусе выполнен
        var result = await Task.WhenAll(requestList);
        Console.WriteLine($"Finished request in {sw.ElapsedMilliseconds}ms");
        
        foreach (var VARIABLE in result)
        {
            if (VARIABLE.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Not found");
            }
            
            // считываем тело ответа в виде строки в которой JSON
            var responseBodyAsString = await VARIABLE.Content.ReadAsStringAsync();
            
            // а какой тип ответа нам вернулся ?
            var contentType = VARIABLE.Content.Headers.ContentType?.MediaType;
            
            // серилозовали в WeatherForecast[]
            var jsonContent = Parse(contentType, responseBodyAsString);
            
            if (jsonContent == null)
            {
                throw new Exception();
            }
        
            //Console.WriteLine(JsonSerializer.Serialize(jsonContent));
        }
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