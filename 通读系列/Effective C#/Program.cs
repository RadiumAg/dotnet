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


// 9 尽量避免装箱与取消装箱这两种操作
int firstNumber = 100;
int secondNumber = 200;
int thirdNumber = 300;
Console.WriteLine($"A few numbers: {firstNumber},{secondNumber},{thirdNumber}");

var attendees = new List<Person>();
Person p = new Person { Name = "OldName" };

attendees.Add(p);
var p2 = attendees[0];
p2.Name = "New Name";

Console.WriteLine(attendees[0].ToString());

// 10. 只有在对应新版基类与现有子类质检的冲突时才应该使用new修饰符
var d = new MyOtherClass();
d.MagicMethod();

var cl2 = d as MyClass;
cl2.MagicMethod();
// 13 用适当的方式初始化类中的静态成员
Console.WriteLine(MySingleton2.TheOnly);

static string ToGerman(FormattableString src)
{
    return string.Format(null, System.Globalization.CultureInfo.CreateSpecificCulture("de-de"), src.Format, src.GetArguments());
}

static string ToFrenchCanada(FormattableString src)
{
    return string.Format(null, System.Globalization.CultureInfo.CreateSpecificCulture("fr-CA"), src.Format, src.GetArguments());
}


public class MySingleton
{
    private static readonly MySingleton theOneAndOnly = new MySingleton();

    public static MySingleton TheOnly
    {
        get { return theOneAndOnly; }
    }

    private MySingleton() { }
}

public class MySingleton2
{
    private static readonly MySingleton2 theOneAndOnly;

    static MySingleton2()
    {
        theOneAndOnly = new MySingleton2();
    }

    public static MySingleton2 TheOnly
    {
        get { return theOneAndOnly; }
    }

    private MySingleton2()
    {

    }
}

public class MyClass
{
    public void MagicMethod()
    {
        Console.WriteLine("MyClass");
    }
}


public class MyOtherClass : MyClass
{
    public new void MagicMethod()
    {
        Console.WriteLine("MyOtherClass");
    }
}

public struct Person
{
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}

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
    private EventHandler<int>? Updated;

    public void RaiseUpdates()
    {
        // var handler = Updated;
        // if (handler != null)
        //     handler(this, counter);

        counter++;
        Updated?.Invoke(this, counter);
    }
}


