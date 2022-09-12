using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

ThreadPractice.Execute();

app.Run();

public static class ThreadPractice
{
    // писать можно только в этом классе. Код можно только добавлять.
    private static object _lock = new object();

    public static List<double> Collection = new List<double>();

    public static void Execute()
    {
        var thread1 = new Thread(() => HandleCollection(FillFirstSequence));
        var thread2 = new Thread(() => HandleCollection(FillSecondSequence));
        var thread3 = new Thread(() => HandleCollection(FillThirdSequence));

        thread1.Start();
        thread2.Start();
        thread3.Start();
    }

    public static void HandleCollection(Action<List<double>> fillSequence)
    {
        lock (_lock)
        {
            fillSequence(Collection);
            Print(Collection);
            Collection.Clear();
        }
    }

    public static void FillFirstSequence(List<double> collection)
    {
        for (var i = 0; i < 10; i++)
        {
            var value = Math.Pow(2, i);
            collection.Add(value);
        }
    }

    public static void FillSecondSequence(List<double> collection)
    {
        for (var i = 0; i < 10; i++)
        {
            var value = Math.Pow(1, i);
            collection.Add(value);
        }
    }

    public static void FillThirdSequence(List<double> collection)
    {
        for (var i = 0; i < 10; i++)
        {
            var value = Math.Pow(3, i);
            collection.Add(value);
        }
    }

    public static void Print(List<double> collection)
    {
        Console.WriteLine();
        var list = new List<double>(collection);
        for (var i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i]);
        }
        Console.WriteLine();
    }
}
