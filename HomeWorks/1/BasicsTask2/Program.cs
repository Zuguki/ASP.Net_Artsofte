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

    public static int MainDelayTime = 1000;
    public static int FirstDelayTime = 1000;
    public static int SecondDelayTime = 1000;

    public static async Task<List<string>> GetExceptionMessageListAsync()
    {
        var result = new List<string>();
        var task = DooMainTaskAsync();
        var task2 = task.ContinueWith(ThrowFirstExceptionAsync);
        var task3 = task.ContinueWith(ThrowSecondExceptionAsync);
        Task? allTasks = null;

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