namespace Lesson2;

public class TokenRequest
{
    // TODO зачем нам это если есть дефолтный таймер
    public static async Task MainAsync()
    {
        var httpClient = new HttpClient();
        
        httpClient.BaseAddress = new Uri("http://localhost:5175");
        httpClient.Timeout = TimeSpan.FromSeconds(2);
        
        var timeout = TimeSpan.FromSeconds(2);
        
        //  CancellationTokenSource - управляет и посылает уведомление об отмене токену.
        var timeoutCancellationToken = new CancellationTokenSource();
        timeoutCancellationToken.CancelAfter(timeout);
        timeoutCancellationToken.Token.Register(() =>
        {
            var d = 1; // подписка на отмену токена
        });

        object result;
        try
        {
            // обрываем соединение
            // httpClient.CancelPendingRequests(); тоже самое
            
            result = await httpClient.PostAsync("weather/cancel", null, timeoutCancellationToken.Token);
            // исключение на случай неправлиьного кода
            // responsePost.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

// CancellationTokenSource source = new CancellationTokenSource();
// source.CancelAfter(TimeSpan.FromSeconds(1));
//
// var tasks = urls.Select(url => Task.Run( async () => 
// {
//     using (var webClient = new WebClient())
//     {
//         token.Register(webClient.CancelAsync);
//         var result = (Url: url, Data: await webClient.DownloadStringTaskAsync(url));
//         token.ThrowIfCancellationRequested();
//         return result.Url;
//     }
// }, token)).ToArray();
//
// string url;
// try
// {
//     // (A canceled task will raise an exception when awaited).
//     var firstTask = await Task.WhenAny(tasks);
//     url = (await firstTask).Url;
// }   
// catch (AggregateException ae) {
//     foreach (Exception e in ae.InnerExceptions) {
//         if (e is TaskCanceledException)
//             Console.WriteLine("Timeout: {0}", 
//                 ((TaskCanceledException) e).Message);
//         else
//             Console.WriteLine("Exception: " + e.GetType().Name);
//     }
// }