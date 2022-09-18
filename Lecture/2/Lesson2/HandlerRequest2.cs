using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;

namespace Lesson2;

public class HandlerRequest2
{
    public static async Task MainAsync()
    {
        // HttpClientHandler vs SocketsHttpHandler
        var socketsHttpHandler = new SocketsHttpHandler()
        {
            // управление пулом
            PooledConnectionLifetime = TimeSpan.FromMinutes(10),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
            MaxConnectionsPerServer = 10 // подебажим пул
        };
        // var httpHandler = new HttpClientHandler(); // внутри себея переопределяет путь на SocketsHttpHandler
        
        var handler = new Example1HttpHandler(new Example2HttpHandler(socketsHttpHandler));

        //https://habr.com/ru/post/424873/ короче надо рассказать про него
        var httpClient = new HttpClient(handler, false);

        // httpClient.Timeout = new TimeSpan(0, 0, 1); установили таймер
        httpClient.BaseAddress = new Uri("http://localhost:5175");
        
        var responsePost = await httpClient.PostAsync("weather/post", null);
        // Content - Возвращает или задает содержимое ответного HTTP-сообщения.
        // Headers - Возвращает коллекцию заголовков HTTP-ответа.
        // IsSuccessStatusCode	- Возвращает значение, указывающее, завершился ли успешно HTTP-ответ.
        // ReasonPhrase - Возвращает или задает фразу причины, которая обычно отправляется серверами вместе с кодом состояния.
        // RequestMessage - Возвращает или задает сообщение запроса, которое привело к получению этого ответного сообщения.
        // StatusCode - Возвращает или задает код состояния HTTP-ответа.
        // TrailingHeaders - Возвращает коллекцию конечных заголовков, содержащихся в ответе HTTP.
        // Version - Возвращает или задает версию HTTP-сообщения.  	

        //var streamPost = await httpClient.GetStreamAsync("WeatherForecast"); заменить на GEt на той стороне

        var d = 1;
    }
    
    // https://www.stevejgordon.co.uk/httpclientfactory-aspnetcore-outgoing-request-middleware-pipeline-delegatinghandlers
    // рассказать 
    private class Example1HttpHandler : DelegatingHandler // просто позволяет встроить в конвейер обработки дополнительную логику
    {
        private static ConcurrentDictionary<string, HttpResponseMessage> _cache = new();
        
        public Example1HttpHandler(HttpMessageHandler innerHandler)
        {
            InnerHandler = innerHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // _cache.
            // TODO добавим тут логи как пример логики
            var result = await base.SendAsync(request, cancellationToken);

            return result;
        }
    }
    
    private class Example2HttpHandler : DelegatingHandler // просто позволяет встроить в конвейер обработки дополнительную логику
    {
        public Example2HttpHandler(HttpMessageHandler innerHandler)
        {
            InnerHandler = innerHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Starting request");
            var result = await base.SendAsync(request, cancellationToken);
            Console.WriteLine($"Finished request in {sw.ElapsedMilliseconds}ms");

            return result;
        }
    }
}