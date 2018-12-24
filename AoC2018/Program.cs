using System;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            var days = new IDay[] {
                new Day1.Day1(),
                new Day2.Day2(),
                new Day3.Day3(),
                new Day4.Day4(),
                new Day5.Day5(),
                new Day6.Day6(),
                new Day7.Day7(),
                new Day8.Day8()
            };

            for (int i = 0; i < days.Length; i++)
            {
                Console.WriteLine("\nDay{0}", i + 1);
                Console.WriteLine(days[i].FirstPuzzle());
                Console.WriteLine(days[i].SecondPuzzle());
            }
        }
    }
}
