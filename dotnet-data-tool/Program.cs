// See https://aka.ms/new-console-template for more information
using System.CommandLine;

class Run
{
    static async Task<int> Main(string[] args)
    {
        var option1 = new Option<string>("--name", "请输入执行者名称");
        var option2 = new Option<string>("--format", "获取指定格式的事件字符串")
        {
            IsRequired = true
        };
        var cmd = new RootCommand();
        cmd.AddOption(option1);
        cmd.AddOption(option2);
        cmd.Name = "dotnet-data-tool";
        cmd.Description = "日期获取工具";
        cmd.SetHandler((option1, option2) =>
        {
            HandleCmd(option1, option2);
        }, option1, option2);

        return await cmd.InvokeAsync(args);
    }

    static void HandleCmd(string name, string format)
    {

        if (!string.IsNullOrWhiteSpace(format))
        {
            var date = DateTime.Now.ToString(format);
            if (!string.IsNullOrWhiteSpace(name))
            {
                Console.Out.WriteLine($"你好,{name},日期更具指定格式转后为{date}");
                return;
            }
            Console.Out.WriteLine(date);
        }
    }
};

