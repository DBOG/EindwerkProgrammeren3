using System;
using System.Diagnostics;
namespace EindWerkProg3
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                Console.WriteLine("Wich tool would you like to start? (1,2,3)");
                int anwser;
                int.TryParse(Console.ReadLine(), out anwser);
                Console.Clear();
                if(anwser == 1)
                {
                    Console.WriteLine("Starting Tool 1");stopwatch.Start();
                    ReadFiles.RunFirstTool();
                    Console.WriteLine($"Tool 1 Completed    <{stopwatch.ElapsedMilliseconds / 1000.0}>seconds"); stopwatch.Reset();
                }
                else if(anwser == 2)
                {
                    Console.WriteLine("Starting Tool 2");stopwatch.Start();
                    WriteDB.RunSecondTool();
                    Console.WriteLine($"Tool 2 Completed    <{stopwatch.ElapsedMilliseconds / 1000.0}>seconds"); stopwatch.Reset();
                }
                else if(anwser == 3)
                {
                    ReadDB.RunThirdTool();
                }
                else
                {
                    Console.WriteLine("No correct input");
                }
            }
        }
    }
}
