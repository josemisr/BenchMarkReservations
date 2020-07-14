using BenchmarkDotNet.Running;
using System;

namespace BenchmarkProject
{

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run <BenchmarkReservation> ();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Benchmark finished!. Press any key to close");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
