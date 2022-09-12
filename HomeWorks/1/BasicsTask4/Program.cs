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
    // писать код в этом классе

    public static object GetWheel()
    {
        return null;
    }
}