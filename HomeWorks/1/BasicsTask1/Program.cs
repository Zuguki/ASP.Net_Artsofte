using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DictionaryPractice.PrintDictionaryValues();

app.Run();

// код можно писать только внутри класса Person
public class Person
{
    public string Name { get; set; }

    public override bool Equals(object? obj) => obj is Person other && other.Name == Name;

    public override int GetHashCode() => HashCode.Combine(Name);
}

// нельзя трогать этот класс
public static class DictionaryPractice
{
    public static void PrintDictionaryValues()
    {
        var misha = new Person { Name = "Миша" };
        var kate = new Person { Name = "Катя" };
        var sasha = new Person { Name = "Саша" };

        var dictionry = new Dictionary<Person, int>
        {
            { misha, 1 },
            { kate, 1 },
            { sasha, 1 },
        };

        foreach (var person in dictionry.Keys)
        {
            Console.WriteLine(dictionry[person]);
        }
    }
}