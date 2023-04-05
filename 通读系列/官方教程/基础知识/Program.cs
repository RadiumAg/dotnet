// See https://aka.ms/new-console-template for more information
// var dictionary = new Dictionary<string, string>();
// dictionary.Add("mac", "2");
// Console.Out.WriteLine(dictionary["mac"]);

var testObj = new TestItem();
testObj["a"] = "testvalue";
var a = "123";
Console.Out.WriteLine(testObj[a]);

abstract class ItemStatement
{
    public virtual object? this[string key]
    {
        get
        {
            var type = this.GetType();
            var propertyInfo = type.GetProperty(key);
            var value = propertyInfo?.GetValue(this);
            return value;
        }
        set
        {
            var type = this.GetType();
            var propertyInfo = type.GetProperty(key);
            propertyInfo?.SetValue(this, value);
        }
    }
}


class TestItem : ItemStatement
{
    public string? a { get; set; }

}


