// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using System.CommandLine.Binding;

public class Run
{
    static async Task<int> Main(string[] args)
    {
        var cmd = new RootCommand(){
            new  Option<string>("--name","请输入执行者名称"),
            new Option<string>("--format", "获取指定格式的事件字符串")
            {
                 IsRequired = true
            },
          };

        cmd.Name = "dotnet-data-tool";
        cmd.Description = "日期获取工具";
        cmd.SetHandler<string, string, IConsole>(HandleCmd, cmd.Options[0] as IValueDescriptor<string>, cmd.Options[1] as IValueDescriptor<string>, null);

        return await cmd.InvokeAsync(args);
    }

    static void HandleCmd(string name, string format, IConsole console)
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
