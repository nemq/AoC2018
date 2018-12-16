using System;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day1");
            var d1 = new Day1.Day1();
            Console.WriteLine(d1.FirstPuzzle());
            Console.WriteLine(d1.SecondPuzzle());
            Console.WriteLine("Day2");
            var d2 = new Day2.Day2();
            Console.WriteLine(d2.FirstPuzzle());
            Console.WriteLine(d2.SecondPuzzle());
            Console.WriteLine("Day3");
            var d3 = new Day3.Day3();
            Console.WriteLine(d3.FirstPuzzle());
            Console.WriteLine(d3.SecondPuzzle());
            Console.WriteLine("Day4");
            var d4 = new Day4.Day4();
            Console.WriteLine(d4.FirstPuzzle());
            Console.WriteLine(d4.SecondPuzzle());
            Console.WriteLine("Day5");
            var d5 = new Day5.Day5();
            Console.WriteLine(d5.FirstPuzzle());
            Console.WriteLine(d5.SecondPuzzle());
            Console.WriteLine("Day6");
            var d6 = new Day6.Day6();
            Console.WriteLine(d6.FirstPuzzle());
            Console.WriteLine(d6.SecondPuzzle());
        }
    }
}
