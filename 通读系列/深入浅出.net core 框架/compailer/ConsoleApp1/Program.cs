using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace DynamicCodeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = @"
                using System;
                using MathLib;

                public class MyClass 
                {
                    public static object MyMethod() {
                       return  TestMath.Left(""test"",3);
                    }
                }";
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("MyAssembly", new[] { syntaxTree }, references: new[] {
                 MetadataReference.CreateFromFile("MathLib.dll"),
                 MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                 MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                 MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location),
                 MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                 MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.Unsafe).Assembly.Location),
            }, options: options)
               .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


            // 将编译结果写入文件
            using (var peStream = new FileStream("MyAssembly.dll", FileMode.Create))
            {
                var result = compilation.Emit(peStream);
                if (!result.Success)
                {
                    // 处理编译失败的情况
                    foreach (var error in result.Diagnostics)
                    {
                        Console.WriteLine(error);
                    }

                    return;
                }
            }

            var assembly = Assembly.LoadFrom("MyAssembly.dll");
            var myClassType = assembly.GetType("MyClass");

            var mainMethod = myClassType?.GetMethod("MyMethod");
            Console.WriteLine($"执行成功：{mainMethod?.Invoke(null, null)}");
        }
    }
}

