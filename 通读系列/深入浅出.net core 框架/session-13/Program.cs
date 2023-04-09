// Task<int> downloading = DownloadDocsMainPageAsync();
// Console.WriteLine($"{nameof(Program)}: Launched downloading");
// int bytesLoaded = await downloading;
// Console.WriteLine($"{nameof(Program)}:Downloaded {bytesLoaded} bytes");

// static async Task<int> DownloadDocsMainPageAsync()
// {
//     Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}:About to start downloading.");

//     var client = new HttpClient();

//     byte[] content = await client.GetByteArrayAsync("https://www.bing.com/");
//     Console.WriteLine($"{nameof(DownloadDocsMainPageAsync): Finished downloading}");

//     return content.Length;
// }


// try
// {
//     var task = Task.Run(() => TaskMedhod());
// }
// catch (AggregateException ae)
// {
//     foreach (var e in ae.InnerExceptions)
//     {
//         Console.WriteLine(e.Message);
//     }
// }


// static Task TaskMedhod()
// {
//     throw new Exception("This exception is expected!");
// }


// var TaskA = Task.Run(() => DateTime.Now);
// //ContinueWith 延续上下文
// await TaskA.ContinueWith(antecedent => Console.WriteLine($"Today is {antecedent.Result.DayOfWeek}"));

// using System.Collections.Concurrent;

// var scheduler = new CustomTaskScheduler();
// var tasks = new List<Task>();
// var task1 = new Task(() =>
// {
//     Write("Running 1 seconds");
//     Thread.Sleep(1000);
// });

// tasks.Add(task1);

// var task2 = new Task(() =>
// {
//     Write("Running 2 seconds");
//     Thread.Sleep(2000);
// });
// tasks.Add(task2);
// foreach (var t in tasks)
// {
//     t.Start(scheduler);
// }

// Write("Press any key to quit..");

// Console.ReadKey();

// static void Write(string msg)
// {
//     Console.WriteLine($"{DateTime.Now:HH:mm:ss} on Thread {Thread.CurrentThread.ManagedThreadId} -- {msg}");
// }

// public class CustomTaskScheduler : TaskScheduler, IDisposable
// {
//     private BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
//     private readonly Thread? _mainThread;

//     public CustomTaskScheduler()
//     {
//         _mainThread = new Thread(this.Execute);
//     }

//     private void Execute()
//     {
//         Console.WriteLine($"Starting Thread {Thread.CurrentThread.ManagedThreadId}");

//         foreach (var t in _tasks.GetConsumingEnumerable())
//         {
//             TryExecuteTask(t);
//         }
//     }


//     public void Dispose()
//     {
//         _tasks.CompleteAdding();
//     }

//     protected override IEnumerable<Task>? GetScheduledTasks()
//     {
//         return _tasks.ToArray<Task>();
//     }

//     protected override void QueueTask(Task task)
//     {
//         _tasks.Add(task);
//         if (!_mainThread.IsAlive)
//         {
//             _mainThread.Start();
//         }
//     }

//     protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
//     {
//         return false;
//     }
// }

var cts = new CancellationTokenSource();

Parallel.ForEach(Enumerable.Range(1, 10), new ParallelOptions
{
    CancellationToken = cts.Token,
    MaxDegreeOfParallelism = Environment.ProcessorCount,
    TaskScheduler = TaskScheduler.Default
}, (i, state) =>
{
    Process(i);
});

static void Process(int i)
{
    Console.WriteLine($"Id:{i}, ThreadId: {Thread.CurrentThread.ManagedThreadId}");
}

