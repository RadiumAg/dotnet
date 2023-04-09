// See https://aka.ms/new-console-template for more information


Task<int>  downloading =  


static async Task<int> DownloadDocsMainPageAsync(){
    Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}:About to start downloading.");
   
   var client = new HttpClient();

   byte[] content = await client.GetByteArrayAsync("https://www.bing.com/");
}