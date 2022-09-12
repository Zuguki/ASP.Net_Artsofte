using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

await TaskPractice.GetExceptionMessageListAsync();

app.Run();

public static class TaskPractice
{
    public static string FirstExceptionMessage = nameof(FirstExceptionMessage);
    public static string SecondExceptionMessage = nameof(SecondExceptionMessage);

    private const int MainDelayTime = 1000;
    private const int FirstDelayTime = 1000;
    private const int SecondDelayTime = 1000;

    public static async Task<List<string>> GetExceptionMessageListAsync()
    {
        var result = new List<string>();
        var task = DooMainTaskAsync();
        var task2 = task.ContinueWith(ThrowFirstExceptionAsync);
        var task3 = task.ContinueWith(ThrowSecondExceptionAsync);
        // var allTasks = Task.WhenAll(task, await task2, await task3);
        Task? allTasks = null;
        // var allTasks = Task.WhenAll(task2, task3);

        try
        {
            allTasks = Task.WhenAll(task, await task2, await task3);
            await allTasks;
        }
        catch (Exception _)
        {
            if (allTasks?.Exception is not null)
                result.AddRange(allTasks.Exception.InnerExceptions.Select(exception => exception.Message));
        }

        foreach (var exc in result)
        {
            Console.WriteLine(exc);
        }
            
        return result;
    }

    public static Task DooMainTaskAsync() => Task.Delay(MainDelayTime);
    
    public static async Task ThrowFirstExceptionAsync(Task task)
    {
        await Task.Delay(FirstDelayTime);
        throw new Exception(FirstExceptionMessage);
    }

    public static async Task ThrowSecondExceptionAsync(Task task)
    {
        await Task.Delay(SecondDelayTime);
        throw new Exception(SecondExceptionMessage);
    }
}