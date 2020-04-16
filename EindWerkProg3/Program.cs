using System;
using System.Diagnostics;
namespace EindWerkProg3
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Starting Tool 1");stopwatch.Start();
            ReadFiles.RunFirstTool();
            Console.WriteLine($"Tool 1 Completed    <{stopwatch.ElapsedMilliseconds / 1000.0}>seconds"); stopwatch.Reset();

            Console.WriteLine("Starting Tool 2");stopwatch.Start();
            WriteDB.RunSecondTool();
            Console.WriteLine($"Tool 2 Completed    <{stopwatch.ElapsedMilliseconds / 1000.0}>seconds"); stopwatch.Reset();
        }
    }
}
