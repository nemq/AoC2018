using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace AoC2018
{
    class Day1 : IDay
    {
        public string FirstPuzzle()
        {
            var freqChanges = ReadFreqChanges(@"C:\Projects\AoC2018\AoC2018\Day1\input");
            return freqChanges.Sum().ToString();
        }

        public string SecondPuzzle()
        {
            var freqChanges = ReadFreqChanges(@"C:\Projects\AoC2018\AoC2018\Day1\input");
            var freq = 0;
            var usedFreqs = new HashSet<int>();
            usedFreqs.Add(freq);
            while(true)
            {
                foreach (var delta in freqChanges)
                {
                    freq += delta;
                    if (usedFreqs.Contains(freq))
                    {
                        return freq.ToString();
                    }
                    else
                    {
                        usedFreqs.Add(freq);
                    }
                }
            }
        }

        private List<int> ReadFreqChanges(string filePath)
        {
            var freqChanges = new List<int>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var change = int.Parse(line);
                    freqChanges.Add(change);
                }
            }

            return freqChanges;
        }
    }
}
