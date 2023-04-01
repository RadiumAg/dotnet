// See https://aka.ms/new-console-template for more information
var dictionary = new Dictionary<string, string>();
dictionary.Add("mac", "2");
Console.Out.WriteLine(new a()["v"]);

class a
{
    public string _v = "1";

    public string v { get { return _v; } }

    public string? this[string key]
    {
        get
        {
            var type = this.GetType();
            var propertyInfo = type.GetProperty(key);
            var value = propertyInfo?.GetValue(this) as string;
            return value;
        }
    }
}


