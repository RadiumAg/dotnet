Third();


static void Third()
{
    long? a = 1000000000000000000;
    int? b = a as int?;

    Console.WriteLine(b);
}


// 内插字符
Console.WriteLine($"The value of pi is {Math.PI:F2}");
Console.WriteLine($"The value of pi is {(true ? Math.PI.ToString() : Math.PI.ToString("F2"))}");

var c = new { Name = "" };

Console.WriteLine($"The customer's name is {c?.Name ?? "Name is missing"}");

var record = new Dictionary<string, string>();

string? result = "";
string? a = "123";
Console.WriteLine($@"Record is {(record.TryGetValue(a, out result) ? result : $"No record found at index {a}")}");
var output = $@"The First five items are:{"path".Take(5).Select(n => $@"Item:{n.ToString()}").Aggregate((c, a) => $@"{c}{Environment.NewLine}-{a}")}";

FormattableString second = $"It's the {DateTime.Now.Day} of the {DateTime.Now.Month} month";


class Test
{
    public static void ExceptionMessage(object thisCantBeNull)
    {
        if (thisCantBeNull == null)
        {
            throw new ArgumentNullException(nameof(thisCantBeNull), "we told you this cant be null");
        }
    }
}

// 8 用null条件运算符调用时间处理程序
public class EventSource
{
    private int counter = 0;
    private EventHandler<int> Updated;

    public void RaiseUpdates()
    {
        var handler = Updated;
        if (handler != null)
            handler(this, counter);
    }
}


static string ToGerman(FormattableString src)
{
    return string.Format(null, System.Globalization.CultureInfo.CreateSpecificCulture("de-de"), src.Format, src.GetArguments());
}

static string ToFrenchCanada(FormattableString src)
{
    return string.Format(null, System.Globalization.CultureInfo.CreateSpecificCulture("fr-CA"), src.Format, src.GetArguments());
}