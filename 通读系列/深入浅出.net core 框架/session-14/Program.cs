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

// class Program
// {
//     static void Main(string[] args)
//     {
//         var thread = new Thread(DoWork);
//         thread.Start();
//         Thread.Sleep(50);

//         Console.ReadKey();
//     }

//     private static volatile bool _isCompleted = false;

//     static void DoWork()
//     {
//         var spinWait = new SpinWait();

//         while (!_isCompleted)
//         {
//             spinWait.SpinOnce();
//             Console.WriteLine($"自旋次数：{spinWait.Count}, 下一次调用触发上下文切换和内核宣旋转：{spinWait.NextSpinWillYield}");
//         }

//         Console.WriteLine("Waiting is complete");
//     }

// }

// var spinLock = new SpinLock();
// var list = new List<int>();


// Parallel.For(0, 10000000, r =>
// {
//     bool lockTaken = false;  // 释放锁
//     try
//     {
//         spinLock.Enter(ref lockTaken); // 进入锁
//         list.Add(r);
//     }
//     finally
//     {
//         if (lockTaken) spinLock.Exit(false); //释放
//     }

//     Console.WriteLine(list.Count);
//     // 输出 10000000
// });


// class Program
// {
//     private static readonly object _object = new object();

//     public static void Print()
//     {
//         bool _lock = false;
//         Monitor.Enter(_object, ref _lock);

//         try
//         {
//             for (int i = 0; i < 5; i++)
//             {
//                 Thread.Sleep(100);
//                 Console.Write(i + ",");
//             }
//             Console.WriteLine();
//         }
//         finally
//         {
//             if (_lock)
//             {
//                 Monitor.Exit(_object);
//             }
//         }
//     }


//     static void Main()
//     {
//         var threads = new Thread[3];

//         for (int i = 0; i < 3; i++)
//         {
//             threads[i] = new Thread(Print);
//         }


//         foreach (var t in threads)
//         {
//             t.Start();
//         }

//         Console.ReadLine();
//     }
// }


class Program
{
    private static Mutex mutex = new Mutex(false);

    static void Main()
    {
        for (int i = 0; i < 5; i++)
        {
            var thread = new Thread(Print);
            thread.Name = "Thread: "  + i;
            thread.Start();
        }
        Console.ReadKey();
    }


    static void Print()
    {
        Console.WriteLine($"{Thread.CurrentThread.Name} 即将开始进行处理");

        try
        {
            mutex.WaitOne();
            Console.WriteLine($"开始：{Thread.CurrentThread.Name} 处理中。。。");
            Thread.Sleep(2000);
            Console.WriteLine($"完成：{Thread.CurrentThread.Name}");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }
}
