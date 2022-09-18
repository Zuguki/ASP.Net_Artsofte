using System.Net;

namespace Lesson2;
// как сделать в рамках 1.1 и 1 tcp сделать 2 запроса, что бы они оба пришли на тот сервер без ответа по 1
// http 1.1 pipline не работает
public static class Http2Request
{
    public static async Task MainAsync()
    {
        var clientHandler = new HttpClientHandler();
        // ограничение на TCP подключения
        clientHandler.MaxConnectionsPerServer = 2; // или 1
        
        var httpClient = new HttpClient(clientHandler);

        var reqList = new List<HttpRequestMessage>()
        {
            new HttpRequestMessage(HttpMethod.Post, "http://localhost:5175/weather/post")
            {
                Version = HttpVersion.Version11,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            },
            new HttpRequestMessage(HttpMethod.Post, "http://localhost:5175/weather/post-1")
            {
                Version = HttpVersion.Version11,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            }
        };

        var requests  = reqList.Select
        (
            item =>
            {
                var task = httpClient.SendAsync(item);
                // что бы их отправить в разное время
                Task.Delay(1000).Wait();
                return task;
            }
        ).ToList();
        
        var result = await await Task.WhenAny(requests);
        

        var d = 1;
    }
    
    public static async Task MainAsyncHttpV2()
    {
        var clientHandler = new HttpClientHandler();
        // ограничение на TCP подключения
        clientHandler.MaxConnectionsPerServer = 1;

        var httpClient = new HttpClient(clientHandler);
        
        var reqList = new List<HttpRequestMessage>()
        {
            new HttpRequestMessage(HttpMethod.Post, "http://localhost:5176/weather/post")
            {
                Version = HttpVersion.Version20,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            },
            new HttpRequestMessage(HttpMethod.Post, "http://localhost:5176/weather/post-1")
            {
                Version = HttpVersion.Version20,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            }
        };

        var requests  = reqList.Select
        (
            item =>
            {
                var task = httpClient.SendAsync(item);
                Task.Delay(1000).Wait();
                return task;
            }
        ).ToList();
        
        await Task.WhenAny(requests);
        
        var responses = requests.Select
        (
            task => task.Result
        );

        foreach (var r in responses)
        {
            // Extract the message body
            var s = await r.Content.ReadAsStringAsync();
            Console.WriteLine(s);
        }
        
        var d = 1;
    }
}