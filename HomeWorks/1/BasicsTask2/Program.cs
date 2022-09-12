using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
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
        // код писать надо в этом методе

        var result = new List<string>();
        var task = DooMainTaskAsync();
        var task2 = task.ContinueWith(ThrowFirstExceptionAsync);
        var task3 = task.ContinueWith(ThrowSecondExceptionAsync);

        try
        {
            await task;
            await task2;
            await task3;
        }
        catch
        {
        }

        return result;
    }

    public static Task DooMainTaskAsync()
    {
        return Task.Delay(MainDelayTime);
    }

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