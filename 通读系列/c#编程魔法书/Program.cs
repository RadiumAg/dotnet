﻿using System.Globalization;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Text;

// StringFormatDemo.Main();
// StringCompareDemo.Main();
// UnicodeDemo.Main();
// ResizeImage.Main();
// Console.WriteLine(Image2Ascii.Convert("Resized.jpg", 120, 100));
// ColorFul.Main();
// MMapDemo.MemMapDemo("./Resized.png", "test");

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

class Image2Ascii
{
    static string _ASCIICharacters = "##@%=+*:-. ";

    public static string Convert(string file, int width, int height)
    {
        var img = Image.Load<Rgba32>(Path.GetFullPath(file));
        width = Math.Min(width, img.Width);
        height = Math.Min(height * img.Height / img.Width, img.Height);

        img.Mutate(data => data.Resize(width, height).Grayscale());

        var sb = new StringBuilder();

        for (var h = 0; h < height; ++h)
        {
            for (var w = 0; w < width; ++w)
            {
                var pixel = img[w, h];
                var idx = pixel.R * _ASCIICharacters.Length / 255;
                idx = Math.Max(0, Math.Min(_ASCIICharacters.Length - 1, idx));
                var c = _ASCIICharacters[idx];
                sb.Append(c);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}

class ColorFul
{
    static string _ASCIICharacters = "##@%=+*:-. ";


    public static void Main()
    {
        var originColor = Console.ForegroundColor;
        var colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));


        var c = (char)Console.Read();

        while (c > 0 && c < 255)
        {
            var idx = _ASCIICharacters.IndexOf(c);

            if (idx >= 0)
            {
                Console.ForegroundColor = (ConsoleColor)(colors[colors.Length - idx - 1]);
            }
            else
            {
                Console.ForegroundColor = originColor;
            }

            Console.WriteLine(c);
            c = (char)Console.Read();
        }
    }
}


class MMapDemo
{
    public static void FileIoDemo(string source, string destination)
    {
        var input = Image.Load<Rgba32>(Path.GetFullPath(source));

        for (int i = 0; i < input.Height; i++)
        {
            for (var j = 0; j < input.Width; ++j)
            {
                Rgba32 white;
                Rgba32.TryParseHex("#ffff", out white);
                input[j, i] = white;
            }
        }

        input.Save(Path.GetFullPath(destination));
    }

    public static void MemMapDemo(string source, string destination)
    {
        File.Copy(source, destination, true);
        byte[] buffer = new byte[2];
        using (var fileStream = new FileStream(source, FileMode.Open, FileAccess.Read))
        using (var header = new BinaryReader(fileStream))
        {

            var pngHeader = header.ReadBytes(8);
            var ihdrHeader = header.ReadBytes(8);

            var widthBytes = header.ReadBytes(4);
            var heightBytes = header.ReadBytes(4);

            Array.Reverse(widthBytes);
            Array.Reverse(heightBytes);

            var width = BitConverter.ToInt32(widthBytes, 0);
            var height = BitConverter.ToInt32(heightBytes, 0);
            using (var mm = MemoryMappedFile.CreateFromFile(destination))
            {
                var whiteRow = new byte[width];
                for (var i = 0; i < width; ++i) whiteRow[i] = 255;
                using (var writer = mm.CreateViewAccessor(21, width * height))
                {
                    for (var i = 0; i < height; i += 50)
                    {
                        writer.WriteArray(i * width, whiteRow, 0, whiteRow.Length);
                    }
                }
            }
        }
    }
}


class GlobalizationDemo
{

    static void Main()
    {
        // 源码位置：第2章\GlobalizationDemo.cs
        // 编译命令：csc GlobalizationDemo.cs
        var culture = CultureInfo.CreateSpecificCulture("en-GB");
        var date = DateTime.Parse("1/9/2019", culture);
        // 输出: 2019年9月1日 星期日
        Console.WriteLine(date.ToLongDateString());
        culture = CultureInfo.CreateSpecificCulture("en-US");
        date = DateTime.Parse("1/9/2019", culture);
        // 输出: 2019年1月9日 星期三
        Console.WriteLine(date.ToLongDateString());
        // 输出：1/9/19 12:00:00 AM
        Console.WriteLine(date.ToString(culture));
        // 输出：1/9/19 12:00:00 AM
        Console.WriteLine(date.ToString(culture.DateTimeFormat));
        // 模式: yyyy'-'MM'-'dd'T'HH':'mm':'ss，输出：2019-01-09T00:00:00
        Console.WriteLine(date.ToString(
            culture.DateTimeFormat.SortableDateTimePattern));
        // 模式: dddd, MMMM d, yyyy，输出: 星期三, 一月 9, 2019
        Console.WriteLine(date.ToString(culture.DateTimeFormat.LongDatePattern));
        Console.WriteLine($"进程的区域设置: {CultureInfo.CurrentCulture.Name}，" +
            $"UI界面的区域设置：{CultureInfo.CurrentUICulture.Name}");
    }
}