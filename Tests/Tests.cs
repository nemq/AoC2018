using NUnit.Framework;

namespace AoC2018Tests
{
    public class Day3Tests
    {
        [Test]
        public void ParseClaim()
        {
            var line = "#342 @ 430,394: 17x16";
            var claim = new AoC2018.Day3.Claim(line);
            Assert.AreEqual(claim.Id, 342);
            Assert.AreEqual(claim.Left, 430);
            Assert.AreEqual(claim.Top, 394);
            Assert.AreEqual(claim.Width, 17);
            Assert.AreEqual(claim.Height, 16);
        }
    }

    public class Day4Tests
    {
        [Test]
        public void ParseRecord()
        {
            var line = @"[1518-11-12 23:47] Guard #2777 begins shift";
            var record = new AoC2018.Day4.Record(line);
            Assert.AreEqual(1518, record.Date.Year);
            Assert.AreEqual(11, record.Date.Month);
            Assert.AreEqual(12, record.Date.Day);
            Assert.AreEqual(23, record.Date.Hour);
            Assert.AreEqual(47, record.Date.Minute);
            Assert.AreEqual("Guard #2777 begins shift", record.Message);
        }

        [Test]
        public void FallAsleepMessage()
        {
            var line = @"[1518-05-22 00:01] falls asleep";
            var record = new AoC2018.Day4.Record(line);
            Assert.AreEqual(AoC2018.Day4.Record.MessageType.FallAsleep, record.Type);
        }

        [Test]
        public void BeginShiftMessage()
        {
            var line = @"[1518-10-23 23:59] Guard #2503 begins shift";
            var record = new AoC2018.Day4.Record(line);
            Assert.AreEqual(AoC2018.Day4.Record.MessageType.BeginShift, record.Type);
            Assert.AreEqual(2503, record.GuardId);
        }

        [Test]
        public void WakeUpMessage()
        {
            var line = @"[1518-08-27 00:58] wakes up";
            var record = new AoC2018.Day4.Record(line);
            Assert.AreEqual(AoC2018.Day4.Record.MessageType.WakesUp, record.Type);
        }
    }

    public class Day5Tests
    {
        [Test]
        public void Reacts()
        {
            Assert.IsTrue(AoC2018.Day5.Polymer.Reacts('a', 'A'));
            Assert.IsTrue(AoC2018.Day5.Polymer.Reacts('A', 'a'));
            Assert.IsTrue(AoC2018.Day5.Polymer.Reacts('z', 'Z'));
            Assert.IsTrue(AoC2018.Day5.Polymer.Reacts('Z', 'z'));
            Assert.IsFalse(AoC2018.Day5.Polymer.Reacts('a', 'a'));
            Assert.IsFalse(AoC2018.Day5.Polymer.Reacts('A', 'A'));
            Assert.IsFalse(AoC2018.Day5.Polymer.Reacts('a', 'b'));
            Assert.IsFalse(AoC2018.Day5.Polymer.Reacts('A', 'B'));
            Assert.IsFalse(AoC2018.Day5.Polymer.Reacts('z', 'z'));
            Assert.IsFalse(AoC2018.Day5.Polymer.Reacts('Z', 'Z'));
        }

        [Test]
        public void SingleReduce()
        {
            var reduced = AoC2018.Day5.Polymer.Reduce("dabAcCaCBAcCcaDA");
            Assert.AreEqual("dabAaCBAcaDA", reduced);
            reduced = AoC2018.Day5.Polymer.Reduce(reduced);
            Assert.AreEqual("dabCBAcaDA", reduced);
        }

        [Test]
        public void PolymerReduce()
        {
            var polymer = new AoC2018.Day5.Polymer("dabAcCaCBAcCcaDA");
            polymer.Reduce();
            Assert.AreEqual("dabCBAcaDA", polymer.Units);
        }

        [Test]
        public void RemoveUnit()
        {
            var polymer = new AoC2018.Day5.Polymer("dabAcCaCBAcCcaDA");
            polymer.RemoveUnit('a');
            Assert.AreEqual("dbcCCBcCcD", polymer.Units);

            polymer = new AoC2018.Day5.Polymer("dabAcCaCBAcCcaDA");
            polymer.RemoveUnit('A');
            Assert.AreEqual("dbcCCBcCcD", polymer.Units);
        }
    }

    public class Day6Tests
    {
        private AoC2018.Day6.Grid grid;

        [SetUp]
        public void SetUp()
        {
            var coords = AoC2018.Day6.Day6.ReadCoords(AoC2018.Utilities.InputPath(6, true));
            grid = new AoC2018.Day6.Grid(coords);
        }

        [Test]
        public void ReadCoords()
        {
            var coords = AoC2018.Day6.Day6.ReadCoords(AoC2018.Utilities.InputPath(6, true));
            Assert.AreEqual(6, coords.Count);
            Assert.AreEqual(new AoC2018.Day6.Coord(1, 1), coords[0]);
            Assert.AreEqual(new AoC2018.Day6.Coord(8, 9), coords[coords.Count - 1]);
        }

        [Test]
        public void GridProperties()
        {
            Assert.AreEqual(1, grid.MinX);
            Assert.AreEqual(1, grid.MinY);
            Assert.AreEqual(8, grid.MaxX);
            Assert.AreEqual(9, grid.MaxY);
            Assert.AreEqual(15, grid.Diameter);
        }

        [Test]
        public void InfiniteArea()
        {
            Assert.IsTrue(grid.HasInfiniteArea(1)); //A
            Assert.IsTrue(grid.HasInfiniteArea(2)); //B
            Assert.IsTrue(grid.HasInfiniteArea(3)); //C
            Assert.IsTrue(grid.HasInfiniteArea(6)); //F
        }

        [Test]
        public void Area()
        {
            Assert.AreEqual(9,grid.Area(4)); //D
            Assert.AreEqual(17,grid.Area(5)); //E
        }

        [Test]
        public void LaregestFiniteArea()
        {
            Assert.AreEqual(17, grid.LargestFiniteArea());
        }

        [Test]
        public void TotalDist()
        {
            Assert.AreEqual(30, grid.TotalDists[4, 3]);
        }

        [Test]
        public void TotalDistArea()
        {
            Assert.AreEqual(16, grid.TotalDistanceRegionArea(32));
        }
    }

    public class Day7Tests
    {
        private AoC2018.Day7.Graph graph = new AoC2018.Day7.Graph();

        [SetUp]
        public void SetUp()
        {
            graph.ReadFromFile(AoC2018.Utilities.InputPath(7, true));
        }

        [Test]
        public void ParseLine()
        {
            const string line = "Step C must be finished before step A can begin.";
            var e = graph.ParseEdge(line);
            Assert.IsNotNull(e);
            Assert.AreEqual('C', e.Source);
            Assert.AreEqual('A', e.Target);
        }

        [Test]
        public void FindRoot()
        {
            var roots = graph.FindRoots();
            Assert.AreEqual(1, roots.Count);
            Assert.AreEqual('C', roots[0]);
        }

        [Test]
        public void FindRoute()
        {
            var res = graph.FindRoute();
            Assert.AreEqual("CABDFE", res.Item1);
        }
        
        [Test]
        public void FindRouteTime()
        {
            var res = graph.FindRoute(2);
            Assert.AreEqual(258, res.Item2);
        }
    }

    public class Day8Tests
    {
        private AoC2018.Day8.Node node;

        [SetUp]
        public void SetUp()
        {
            var path = AoC2018.Utilities.InputPath(8, true);
            node = AoC2018.Day8.Node.ReadFromFile(path);
        }

        [Test]
        public void RootHeaderMetadataCount()
        {
            Assert.AreEqual(3, node.MetadataCount);
        }

        [Test]
        public void RootHeaderChildCount()
        {
            Assert.AreEqual(2, node.ChildrenCount);
        }

        [Test]
        public void RootMetadataCount()
        {
            Assert.AreEqual(3, node.Metadata.Count);
        }

        [Test]
        public void RootChildCount()
        {
            Assert.AreEqual(2, node.Children.Count);
        }

        [Test]
        public void RootChildrenHeaders()
        {
            var firstChild = node.Children[0];
            Assert.AreEqual(0, firstChild.ChildrenCount);
            Assert.AreEqual(3, firstChild.MetadataCount);
            var secondChild = node.Children[1];
            Assert.AreEqual(1, secondChild.ChildrenCount);
            Assert.AreEqual(1, secondChild.MetadataCount);
        }

        [Test]
        public void MetadataSum()
        {
            Assert.AreEqual(138, node.MetadataSum());
        }

        [Test]
        public void Value()
        {
            Assert.AreEqual(66, node.Value());
        }
    }
}