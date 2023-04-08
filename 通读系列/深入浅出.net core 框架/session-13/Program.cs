// See https://aka.ms/new-console-template for more information
Console.WriteLine("输入一行地址，按Enter键进行下载");

while(true) {
    var url = Console.ReadLine();
    var uri = new Uri(url);
    new Thread(()=>{
      Request(uri);
    }).Start();
}


static  void   Request(Uri uri) {
    using HttpClient client =  new HttpClient();

    HttpResponseMessage response;
    try {
        response = client.GetAsync(uri).Result;
        var  content = response.Content.ReadAsByteArrayAsync().Result;
        if(content != null) {
            using var fileStream = File.Create($"./{DateTime.Now.ToString("yyyyMMdd-HHmmssff")}.html");
            fileStream.Write(content);
            fileStream.Flush();
        }

    } catch (Exception ex)
    { 
        Console.WriteLine($"{uri} 下载任务失败，错误：{ex}");
    }
}