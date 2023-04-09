Task<int> downloading = DownloadDocsMainPageAsync();
Console.WriteLine($"{nameof(Program)}: Launched downloading");
int bytesLoaded = await downloading;
Console.WriteLine($"{nameof(Program)}:Downloaded {bytesLoaded} bytes");

static async Task<int> DownloadDocsMainPageAsync()
{
    Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}:About to start downloading.");

    var client = new HttpClient();

    byte[] content = await client.GetByteArrayAsync("https://www.bing.com/");
    Console.WriteLine($"{nameof(DownloadDocsMainPageAsync): Finished downloading}");

    return content.Length;
}