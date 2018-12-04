using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace AoC2018
{
    class Day2 : IDay
    {
        public string FirstPuzzle()
        {
            int doubles = 0, triples = 0;
            var ids = ReadIds(Utilities.InputPath(2));
            foreach (var id in ids)
            {
                var hist = LetterHistogram(id);
                if (hist.ContainsValue(2))
                {
                    doubles++;
                }

                if (hist.ContainsValue(3))
                {
                    triples++;
                }
            }

            return (doubles * triples).ToString();
        }

        public string SecondPuzzle()
        {
            var ids = ReadIds(Utilities.InputPath(2));
            var idLength = ids.First().Length;
            for (int idx = 0; idx < idLength; idx++)
            {
                var duplicates = ids.Select(id => id.Remove(idx, 1))
                                    .GroupBy(id => id)
                                    .Where(g => g.Count() > 1);
                if (duplicates.Count() > 0)
                {
                    return duplicates.First().First();
                }
            }

            return "Could not find Id";
        }

        private List<string> ReadIds(string filePath)
        {
            var ids = new List<string>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    ids.Add(reader.ReadLine());
                }
            }

            return ids;
        }

        private Dictionary<char, UInt32> LetterHistogram(string s)
        {
            var hist = new Dictionary<char, UInt32>();
            foreach (var l in s)
            {
                UInt32 v = 0;
                if (!hist.TryGetValue(l, out v))
                {
                    hist.Add(l, v);
                }
                else
                {
                    v++;
                }

                hist[l]++;
            }
            return hist;
        }
    }
}
