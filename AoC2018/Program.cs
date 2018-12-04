using System;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            var d1 = new Day1();
            Console.WriteLine(d1.FirstPuzzle());
            Console.WriteLine(d1.SecondPuzzle());
            var d2 = new Day2();
            Console.WriteLine(d2.FirstPuzzle());
            Console.WriteLine(d2.SecondPuzzle());
        }
    }
}
