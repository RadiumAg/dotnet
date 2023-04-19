using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Text;

StringFormatDemo.Main();
StringCompareDemo.Main();
UnicodeDemo.Main();
ResizeImage.Main();

class StringFormatDemo
{
    public static void Main()
    {
        var str = String.Format("当前时间：{0}，温度是：{1}", DateTime.Now, 24.5);
        Console.WriteLine(str);
        Console.WriteLine("当前时间：{0:d}，时间：{0:t}", DateTime.Now);
        int[] years = { 2013, 2014, 2015 };
        int[] population = { 1025632, 4545444, 4545454 };
        Console.WriteLine("{0,6}{1,15}", "Year", "Population");
        for (int i = 0; i < years.Length; i++)
        {
            Console.WriteLine("{0,6}{1,15:N0}", years[i], population[i]);
        }
        Console.WriteLine("");
        Console.WriteLine("{0,-12}{1,8:yyyy}{2,12:N2}{3,14:P1}", "BeiJing", DateTime.Now, 123456, 0.32d);
        // 标准数字格式
        Console.WriteLine(123.456m.ToString("N2"));
        // 自定义数字格式
        Console.WriteLine(123.4.ToString("00000.000"));
        char c1 = '{', c2 = '}';
        Console.WriteLine("打印大括号用法相同：\n{{和}}\n{0}和{1}", c1, c2);
        Console.WriteLine("打印大括号用法相同：\n{{和}}\n{0}和{1}", c1, c2);
        var path1 = @"c:\china-pub\C#\sample-code";
        var path2 = "c:\\china-pub\\C#\\sample-code";
        Console.WriteLine("两个路径是相同的:\n{0}\n{1}", path1, path2);
    }
}

class StringCompareDemo
{
    public static void Main()
    {
        object a = 1, b = 1;
        Console.WriteLine(a == (object)1);
        Console.WriteLine(a == b);

        string c = "1", d = "1";
        Console.WriteLine(c == d);
        Console.WriteLine(c == "1");


        Console.WriteLine(string.Compare("1", "2"));
        Console.WriteLine(string.Compare("a", "A"));
        Console.WriteLine(string.Compare("a", "A", true));

        Console.WriteLine(string.Compare("财经传讯公司", "房地产及按揭"));
        Console.WriteLine(string.Compare("财经传讯公司", "房地产及按揭", false, new CultureInfo("zh-CN")));
        Console.WriteLine(string.Compare("财经传讯公司", "房地产及按揭", false, new CultureInfo("en-US")));
    }
}

class UnicodeDemo
{
    public static void Main()
    {
        var emoji = "\uD83D\uDE42";
        Console.WriteLine(emoji);
        var x = "😀";
        Console.WriteLine(GetUnicodeString(x));
        Console.WriteLine("Unicode - UTF16");
        var bytes = Encoding.Unicode.GetBytes(x);
        foreach (var b in bytes) Console.Write("{0:x2}", b);
    }


    static string? GetUnicodeString(string s)
    {
        var sb = new StringBuilder();
        foreach (char c in s)
        {
            sb.Append("\\u");
            sb.Append(String.Format("{0:x4}", (int)c));
        }
        return sb.ToString();
    }

}


class CsxCopy
{

    static void DoXCopy(string srcdir, string dstdir, string searchPattern)
    {
        if (srcdir[srcdir.Length - 1] == Path.DirectorySeparatorChar)
            srcdir = srcdir.Substring(0, srcdir.Length - 1);

        if (dstdir[dstdir.Length - 1] == Path.DirectorySeparatorChar)
            dstdir = dstdir.Substring(0, dstdir.Length - 1);

        var files = Directory.GetFiles(srcdir, searchPattern, SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var directory = Path.GetDirectoryName(file);
            var filename = Path.GetFileName(file);
            var newdirectory = dstdir;

            if (directory.Length > srcdir.Length)
            {
                var relativePath = directory.Substring(srcdir.Length + 1, directory.Length - srcdir.Length - 1);
                newdirectory = Path.Combine(dstdir, relativePath);
            }

            if (!Directory.Exists(newdirectory))
                Directory.CreateDirectory(newdirectory);

            File.Copy(file, Path.Combine(newdirectory, filename));
        }
    }
}


class StreamDemo
{
    static void Main()
    {
        using (var fs = new FileStream("filestream.demo", FileMode.OpenOrCreate))
        {
            for (var i = 0; i < 26; ++i)
            {
                fs.WriteByte((byte)(i + 'a'));
            }

            fs.Seek(0, SeekOrigin.Begin);
            int b = 0;
            while ((b = fs.ReadByte()) > 0)
            {
                Console.Write((char)b);
            }

            Console.WriteLine();

            fs.Seek(0, SeekOrigin.Begin);

            var bytes = new byte[20];

            var count = 0;

            while ((count = fs.Read(bytes, 0, bytes.Length)) > 0)
            {
                Console.Write(System.Text.Encoding.ASCII.GetString(bytes));
                Array.Clear(bytes, 0, bytes.Length);
            }
            Console.WriteLine();
        }
    }
}



class ResizeImage
{
    public static void Main()
    {
        var request = WebRequest.CreateHttp("https://www.yulumi.cn/gl/uploads/allimg/201128/162003D24-2.jpg");
        var response = request.GetResponse();

        using (var inputStream = response.GetResponseStream())
        {
            using (var outputStream = File.Open("Resized.jpg", FileMode.OpenOrCreate))
            {
                var img = Image.Load(inputStream);
                img.Mutate(data => data.Resize(img.Width / 2, img.Height / 2));

                img.Save(outputStream, img.Metadata.DecodedImageFormat);
                outputStream.Seek(0, SeekOrigin.Begin);
                Console.WriteLine(outputStream.ReadByte());
            }
        }
    }
}


class CsGzip
{
    public static void Compress(string file)
    {
        using (var fileStream = File.OpenRead(file))
        {
            if ((File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                return;
            }

            using (var outputStream = File.Create(file + ".gz"))
            {
                using (var gzstream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    fileStream.CopyTo(gzstream);
                }
            }
        }
    }
}