// See https://aka.ms/new-console-template for more information


// var value = 0;

// Parallel.For(0, 10000, _ =>
// {
//     Interlocked.Add(ref value, 1);
// });

// Console.WriteLine(value);

// var location = 1;
// var value = 3;
// var compared = 1;
// Interlocked.CompareExchange(ref location, value, compared);
// Console.WriteLine(location);

class Program
{
    static void Main(string[] args)
    {
        var thread = new Thread(DoWork);
        thread.Start();
        Thread.Sleep(50);

        Console.ReadKey();
    }

    private static volatile bool _isCompleted = false;

    static void DoWork()
    {
        var spinWait = new SpinWait();

        while (!_isCompleted)
        {
            spinWait.SpinOnce();
            Console.WriteLine($"自旋次数：{spinWait.Count}, 下一次调用触发上下文切换和内核宣旋转：{spinWait.NextSpinWillYield}");
        }

        Console.WriteLine("Waiting is complete");
    }

}