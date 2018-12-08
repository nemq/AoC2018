using System;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day1");
            var d1 = new Day1();
            Console.WriteLine(d1.FirstPuzzle());
            Console.WriteLine(d1.SecondPuzzle());
            Console.WriteLine("Day2");
            var d2 = new Day2();
            Console.WriteLine(d2.FirstPuzzle());
            Console.WriteLine(d2.SecondPuzzle());
            Console.WriteLine("Day3");
            var d3 = new Day3();
            Console.WriteLine(d3.FirstPuzzle());
            Console.WriteLine(d3.SecondPuzzle());
        }
    }
}
