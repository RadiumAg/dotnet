using System.Collections;
using StackExchange.Redis;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// app.MapControllers();

// const string cacheMaxAge = "604800";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new RedisFileProvider(Options.Create(new RedisFileOptions { HostAndPort = "localhost:6379" }))
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new RedisFileProvider(Options.Create(new RedisFileOptions { HostAndPort = "localhost:6379" }))
});
app.Run();


public class RedisFileInfo : IFileInfo
{
    public RedisFileInfo(string name, string? content)
    {
        Name = name;
        LastModified = DateTimeOffset.Now;
        _fileContent = Convert.FromBase64String(content);
    }


    public RedisFileInfo(string name, bool isDirectory)
    {
        Name = name;
        LastModified = DateTimeOffset.Now;
        IsDirectory = isDirectory;
    }

    public string Name { get; set; }

    public string? PhysicalPath { get; }

    public bool Exists => true;

    public bool IsDirectory { get; }

    public DateTimeOffset LastModified { get; }

    public long Length => throw new NotImplementedException();

    private readonly byte[]? _fileContent;


    public Stream? CreateReadStream()
    {
        if (_fileContent != null)
        {
            var stream = new MemoryStream(_fileContent);
            stream.Position = 0;
            return stream;
        }

        return null;
    }
}

public class EnumerableDirectoryContents : IDirectoryContents
{
    public EnumerableDirectoryContents(IEnumerable<IFileInfo> entries)
    {
        _entries = entries;
    }

    private readonly IEnumerable<IFileInfo> _entries;
    public bool Exists => true;

    public IEnumerator<IFileInfo>? GetEnumerator()
    {
        return _entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


public class RedisFileOptions
{
    public string HostAndPort { get; set; }
}


public class RedisFileProvider : IFileProvider
{
    public RedisFileProvider(IOptions<RedisFileOptions> options)
    {
        _options = options.Value;
        _redis = ConnectionMultiplexer.Connect(new ConfigurationOptions { 
            EndPoints = {  _options.HostAndPort }   
        
        });
    }
    private readonly RedisFileOptions? _options;

    private readonly ConnectionMultiplexer? _redis;

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var db = _redis?.GetDatabase();
        var server = _redis?.GetServer(_options.HostAndPort);
        var list = new List<IFileInfo>();
        subpath = NormalizePath(subpath);

        foreach (var key in server.Keys(0, $"{subpath}"))
        {
            var k = "";
            if (subpath != "")
            {
                k = key.ToString().Replace(subpath, "").Split(":")[0];
            }
            else
            {
                k = key.ToString().Split(":")[0];
            }

            if (list.Find(f => f.Name == k) == null)
            {
                if (k.IndexOf('.', StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    list.Add(new RedisFileInfo(k, db?.StringGet(k)));
                }
                else
                {
                    list.Add(new RedisFileInfo(k, true));
                }
            }

        }

        if (list.Count == 0)
        {
            return NotFoundDirectoryContents.Singleton;
        }

        return new EnumerableDirectoryContents(list);

    }

    public IChangeToken Watch(string filter)
    {
        throw new NotImplementedException();
    }

    private static string NormalizePath(string path) => path.TrimStart('/').Replace('/', ':');

    public IFileInfo? GetFileInfo(string subpath)
    {
        subpath = NormalizePath(subpath);
        var db = _redis?.GetDatabase();
        var redisValue = db?.StringGet(subpath);

        return !redisValue.HasValue ? new NotFoundFileInfo(subpath) : new RedisFileInfo(subpath, redisValue.ToString());
    }

}