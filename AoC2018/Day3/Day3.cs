using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;


namespace AoC2018
{
    using Cell = Tuple<int, int>;

    public class Day3 : IDay
    {
        private List<Claim> claims;
        private Dictionary<Cell, int> coverage;

        public Day3()
        {
            claims = ReadClaims(Utilities.InputPath(3));
            BuildCoverage(claims);
        }

        public string FirstPuzzle()
        {
            return coverage.Count(c => c.Value > 1).ToString();
        }

        public string SecondPuzzle()
        {
            foreach (var claim in claims)
            {
                var test = new OverlapTest(coverage);
                claim.ForEachCell(test.Test);
                if (!test.OverlapFound)
                {
                    return claim.Id.ToString();
                }
            }

            throw new Exception("Failed to find not overlapped claim");
        }

        private List<Claim> ReadClaims(string path)
        {
            var claims = new List<Claim>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    claims.Add(new Claim(reader.ReadLine()));
                }
            }
            return claims;
        }
        
        private void BuildCoverage(List<Claim> claims)
        {
            coverage = new Dictionary<Cell, int>();
            foreach (var claim in claims)
            {
                claim.ForEachCell(BuildCoverage);
            }
        }

        private void BuildCoverage(Tuple<int, int> cell)
        {
            if (coverage.ContainsKey(cell))
            {
                coverage[cell] += 1; 
            }
            else
            {
                coverage.Add(cell, 1);
            }
        }
        
        public class OverlapTest
        {
            private readonly Dictionary<Cell, int> coverage;
            public OverlapTest(Dictionary<Cell, int> coverage)
            {
                OverlapFound = false;
                this.coverage = coverage; 
            }

            public bool OverlapFound { get; set; }
            public void Test(Cell cell)
            {
                if(coverage[cell] > 1)
                {
                    OverlapFound = true;
                }
            }
        }

        public class Claim
        {
            private static readonly string Pattern = @"#(?<id>\d+)\s@\s" +
                                    @"(?<left>\d+),(?<top>\d+):\s" +
                                    @"(?<width>\d+)x(?<height>\d+)";
            private static readonly Regex Regex = new Regex(Pattern);

            public delegate void CellHandler(Tuple<int, int> cell);

            public Claim(string claim)
            {
                var match = Regex.Match(claim);
                if (!match.Success)
                {
                    throw new ArgumentException("Invalid claim: " + claim);
                }

                Id = int.Parse(match.Groups["id"].Value);
                Left = int.Parse(match.Groups["left"].Value);
                Top = int.Parse(match.Groups["top"].Value);
                Width = int.Parse(match.Groups["width"].Value);
                Height = int.Parse(match.Groups["height"].Value);
            }

            public void ForEachCell(CellHandler handler)
            {
                for (int x = Left; x < Left + Width; x++)
                {
                    for(int y = Top; y < Top + Height; y++)
                    {
                        var cell = new Tuple<int, int>(x, y);
                        handler(cell);
                    }
                }
            }

            public int Id { get; set; }
            public int Left { get; set; }
            public int Top { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
