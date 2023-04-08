public class Run
{
    static void run()
    {

        Console.WriteLine("输入一行地址，按Enter键进行下载");

        for (int i = 0; i < 20; i++)
        {
            ThreadPool.QueueUserWorkItem(state => DoWork());
        }

        while (true)
        {
            var url = Console.ReadLine();
            var uri = new Uri(url);
            new Thread(() =>
            {
                Request(uri);
            }).Start();
        }


        var t = new Thread(() => Console.WriteLine("Hello!"));
        t.Start();

        static void Request(Uri uri)
        {
            using HttpClient client = new HttpClient();

            HttpResponseMessage response;
            try
            {
                response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsByteArrayAsync().Result;
                if (content != null)
                {
                    using var fileStream = File.Create($"./{DateTime.Now.ToString("yyyyMMdd-HHmmssff")}.html");
                    fileStream.Write(content);
                    fileStream.Flush();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{uri} 下载任务失败，错误：{ex}");
            }
        }

    }

    static void DoWork()
    {
        Console.WriteLine($"ThreadId:{Thread.CurrentThread.ManagedThreadId}, ThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}, Background: {Thread.CurrentThread.IsBackground}");
    }
}