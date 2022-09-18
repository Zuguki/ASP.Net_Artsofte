using Lesson2;

try
{
    var task = new Task(() => throw new Exception());
    task.Start();
    task.GetAwaiter().GetResult();
    //task.Wait();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


Console.ReadLine();


