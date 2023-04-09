// See https://aka.ms/new-console-template for more information



// var value = 0;

// Parallel.For(0, 10000, _ =>
// {
//     Interlocked.Add(ref value, 1);
// });

// Console.WriteLine(value);

var location = 1;
var value = 3;
var compared = 1;
Interlocked.CompareExchange(ref location, value, compared);
Console.WriteLine(location);