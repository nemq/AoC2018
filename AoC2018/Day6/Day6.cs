using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AoC2018.Day6
{
    public class Day6 : IDay
    {
        private Grid grid;

        public Day6()
        {
            grid = new Grid(ReadCoords(Utilities.InputPath(6)));
        }

        public static List<Coord> ReadCoords(string path)
        {
            var coords = new List<Coord>();
            using (var stream = new StreamReader(path))
            {
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    var tokens = line.Split(',');
                    if (tokens.Count() != 2)
                    {
                        throw new InvalidDataException("Wrong number of coords in line: " + line);
                    }

                    var x = int.Parse(tokens[0].Trim());
                    var y = int.Parse(tokens[1].Trim());
                    coords.Add(new Coord(x, y));
                }
            }
            return coords;
        }

        public string FirstPuzzle()
        {
            return grid.LargestFiniteArea().ToString();
        }

        public string SecondPuzzle()
        {
            return grid.TotalDistanceRegionArea(10000).ToString();
        }
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static int ManhattanDistance(Coord a, Coord b)
        {
            return Math.Abs(b.x - a.x) + Math.Abs(b.y - a.y);
        }
    }

    public class Grid
    {
        public Dictionary<int, Coord> Locations { get; private set; } = new Dictionary<int, Coord>();
        //!Rectangle of interest
        public Dictionary<int, int> CoordsAreaInROI { get; private set; } = new Dictionary<int, int>();
        public RectangleOfInterest TotalDists { get; private set; }
        public int MinX { get; private set; } = int.MaxValue;
        public int MinY { get; private set; } = int.MaxValue;
        public int MaxX { get; private set; } = int.MinValue;
        public int MaxY { get; private set; } = int.MinValue;
        public int Diameter { get; private set; }

        public Grid(IEnumerable<Coord> coords)
        {
            var id = 1;
            foreach (var cord in coords)
            {
                Locations.Add(id++, cord);
                MinX = Math.Min(MinX, cord.x);
                MinY = Math.Min(MinY, cord.y);
                MaxX = Math.Max(MaxX, cord.x);
                MaxY = Math.Max(MaxY, cord.y);
            }
            Diameter = Coord.ManhattanDistance(new Coord(MinX, MinY), new Coord(MaxX, MaxY));
            TotalDists = new RectangleOfInterest(MinX, MinY, MaxX, MaxY);

            for (var y = MinY; y <= MaxY; y++)
            {
                for (var x = MinX; x <= MaxX; x++)
                {
                    var coordROI = new Coord(x, y);
                    TotalDists[x, y] = CalcTotalDist(coordROI);
                    var locId = FindNearestLocation(coordROI);
                    if (locId != null)
                    {
                        if (CoordsAreaInROI.ContainsKey(locId.Value))
                        {
                            CoordsAreaInROI[locId.Value] += 1;
                        }
                        else
                        {
                            CoordsAreaInROI.Add(locId.Value, 1);
                        }
                    }
                }
            }
        }

        public int TotalDistanceRegionArea(int maxDist)
        {
            var area = 0;
            for (int y = TotalDists.MinY; y <= TotalDists.MaxY; y++)
            {
                for (int x = TotalDists.MinX; x <= TotalDists.MaxX; x++)
                {
                    var dist = TotalDists[x, y];
                    if (dist < maxDist)
                    {
                        area += 1;
                    }
                }
            }
            return area;    
        }

        private int CalcTotalDist(Coord coordROI)
        {
            var dist = 0;
            foreach (var coordLoc in Locations.Values)
            {
                dist += Coord.ManhattanDistance(coordROI, coordLoc);
            }

            return dist;
        }

        private int? FindNearestLocation(Coord coord)
        {
            var nearestId = Locations.First().Key;
            var nearestDist = Coord.ManhattanDistance(Locations.First().Value, coord);
            bool tie = false;
             
            foreach (var kv in Locations.Skip(1))
            {
                var dist = Coord.ManhattanDistance(kv.Value, coord);
                if (dist < nearestDist)
                {
                    nearestId = kv.Key;
                    nearestDist = dist;
                    tie = false;
                }
                else if(dist == nearestDist)
                {
                    tie = true;
                }
            }

            if (tie)
            {
                return null;
            }
            else
            {
                return nearestId;
            }
        }

        public bool HasInfiniteArea(int locId)
        {
            var locCoord = Locations[locId];

            var top = new Coord(locCoord.x, locCoord.y + Diameter);
            if (FindNearestLocation(top) == locId)
            {
                return true;
            }

            var left = new Coord(locCoord.x - Diameter, locCoord.y);
            if (FindNearestLocation(left) == locId)
            {
                return true;
            }

            var bot = new Coord(locCoord.x, locCoord.y - Diameter);
            if (FindNearestLocation(bot) == locId)
            {
                return true;
            }

            var right = new Coord(locCoord.x + Diameter, locCoord.y);
            if (FindNearestLocation(right) == locId)
            {
                return true;
            }

            return false;
        }

        public int? Area(int locId)
        {
            if (HasInfiniteArea(locId))
            {
                return null;
            }
            else
            {
                return CoordsAreaInROI[locId];
            }
        }

        public int LargestFiniteArea()
        {
            return CoordsAreaInROI.Where(kv => !HasInfiniteArea(kv.Key)).Max(kv => kv.Value);
        }
    }


    public class RectangleOfInterest
    {
        public int MinX { get; private set; }
        public int MinY { get; private set; }
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }
        public int Width { get; private set; } 
        public int Heigth { get; private set; }

        private readonly int[,] roi;

        public RectangleOfInterest(int minX, int minY, int maxX, int maxY)
        {
            Width = maxX + 1 - minX;
            Heigth = maxY + 1 - minY;
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
            roi = new int[Width, Heigth];
        }
        
        public int this[int x, int y]
        {
            get
            {
                return roi[x - MinX, y - MinY];
            }
            set
            {
                roi[x - MinX, y - MinY] = value;
            }
        }
    }

}
