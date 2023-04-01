// See https://aka.ms/new-console-template for more information
var dictionary = new Dictionary<string, string>();
dictionary.Add("mac", "2");
Console.Out.WriteLine(dictionary["mac"]);

class a
{
    public string _v = "1";

    public string v { get { return _v; } }

    public object? this[string key]
    {
        get
        {
            var type = this.GetType();
            var propertyInfo = type.GetProperty(key);
            var value = propertyInfo?.GetValue(this);
            return value;
        }
    }
}


