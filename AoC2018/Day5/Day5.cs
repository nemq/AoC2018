using System;
using System.IO;
using System.Text;

namespace AoC2018.Day5
{
    public class Day5 : IDay
    {
        private Polymer polymer = null;

        public Day5()
        {
            using (var stream = new StreamReader(Utilities.InputPath(5)))
            {
                polymer = new Polymer(stream.ReadToEnd().Trim());
            }
        }

        public string FirstPuzzle()
        {
            var p = polymer.Clone();
            p.Reduce();
            return p.Units.Length.ToString();
        }

        public string SecondPuzzle()
        {
            var minLength = polymer.Units.Length;
            for (char c = 'a'; c <= 'z'; c++)
            {
                var p = polymer.Clone();
                p.RemoveUnit(c);
                p.Reduce();
                minLength = Math.Min(minLength, p.Units.Length);
            }

            return minLength.ToString();
        }
    }
    public class Polymer
    {
        private static readonly int CaseOffset = 'a' - 'A';
        public Polymer(string data)
        {
            Units = data;
        }

        public void Reduce()
        {
            while (true)
            {
                var reducedUnits = Reduce(Units);
                if (reducedUnits.Length == Units.Length)
                {
                    break;
                }
                Units = reducedUnits;
            }
        }

        public void RemoveUnit(char unit)
        {
            Units = Units.Replace(unit.ToString(), "", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string Reduce(string data)
        {
            var sb = new StringBuilder();
            int lastChunkEnd = 0;
            for (int pos = 0; pos < data.Length - 1;)
            {
                if (Reacts(data[pos], data[pos + 1]))
                {
                    sb.Append(data.Substring(lastChunkEnd, pos - lastChunkEnd));
                    pos += 2;
                    lastChunkEnd = pos;
                }
                else
                {
                    pos += 1;
                }
            }

            sb.Append(data.Substring(lastChunkEnd));
            return sb.ToString();
        }

        public static bool Reacts(char firstUnit, char secondUnit)
        {
            if (firstUnit > secondUnit)
            {
                return firstUnit - secondUnit == CaseOffset;
            }
            else
            {
                return secondUnit - firstUnit == CaseOffset;
            }
        }

        public Polymer Clone()
        {
            return new Polymer(Units);
        }

        public string Units { get; set; }
    }
}
