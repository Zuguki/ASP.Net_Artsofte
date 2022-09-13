using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var result = ReflectionPractice.GetWheel();

app.Run();

public interface IWheel
{
}

public abstract class Wheel : IWheel
{
    protected int _loadCapacity;

    private void SetDefaultLoadCapacity()
    {
        _loadCapacity = 120;
    }
}

public class Car
{
    private class CarWheel : Wheel
    {
        private string _name;

        private CarWheel()
        {

        }

        public string Name => _name;

        public int LoadCapacity => _loadCapacity;

        public int AxisLength { get; private set; }
    }
}

public static class ReflectionPractice
{
    public static object GetWheel()
    {
        var wheelType = Assembly.GetExecutingAssembly().GetTypes()
            .FirstOrDefault(type => type.IsClass && !type.IsAbstract && typeof(IWheel).IsAssignableFrom(type));

        if (wheelType is null)
            return new object();
        
        var instance = Activator.CreateInstance(wheelType, true);
        var setMethod = wheelType.GetMethod("set_AxisLength", BindingFlags.NonPublic | BindingFlags.Instance);
        var baseMethod = wheelType.BaseType?.GetMethod("SetDefaultLoadCapacity", BindingFlags.NonPublic | BindingFlags.Instance);
        setMethod!.Invoke(instance, new object[] {999});
        baseMethod!.Invoke(instance, null);
        
        var fields = wheelType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
        foreach (var field in fields)
            field.SetValue(instance, field.FieldType == typeof(int) ? 999 : "999");

        return instance;
    }
}