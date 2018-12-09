using NUnit.Framework;
using AoC2018;

namespace AoC2018Tests
{
    public class Day3Tests
    {
        [Test]
        public void ParseClaim()
        {
            var line = "#342 @ 430,394: 17x16";
            var claim = new Day3.Claim(line);
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
            var record = new Day4.Record(line);
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
            var record = new Day4.Record(line);
            Assert.AreEqual(Day4.Record.MessageType.FallAsleep, record.Type);
        }

        [Test]
        public void BeginShiftMessage()
        {
            var line = @"[1518-10-23 23:59] Guard #2503 begins shift";
            var record = new Day4.Record(line);
            Assert.AreEqual(Day4.Record.MessageType.BeginShift, record.Type);
            Assert.AreEqual(2503, record.GuardId);
        }

        [Test]
        public void WakeUpMessage()
        {
            var line = @"[1518-08-27 00:58] wakes up";
            var record = new Day4.Record(line);
            Assert.AreEqual(Day4.Record.MessageType.WakesUp, record.Type);
        }
    }

    public class Day5Tests
    {
        [Test]
        public void Reacts()
        {
            Assert.IsTrue(Day5.Polymer.Reacts('a', 'A'));
            Assert.IsTrue(Day5.Polymer.Reacts('A', 'a'));
            Assert.IsTrue(Day5.Polymer.Reacts('z', 'Z'));
            Assert.IsTrue(Day5.Polymer.Reacts('Z', 'z'));
            Assert.IsFalse(Day5.Polymer.Reacts('a', 'a'));
            Assert.IsFalse(Day5.Polymer.Reacts('A', 'A'));
            Assert.IsFalse(Day5.Polymer.Reacts('a', 'b'));
            Assert.IsFalse(Day5.Polymer.Reacts('A', 'B'));
            Assert.IsFalse(Day5.Polymer.Reacts('z', 'z'));
            Assert.IsFalse(Day5.Polymer.Reacts('Z', 'Z'));
        }

        [Test]
        public void SingleReduce()
        {
            var reduced = Day5.Polymer.Reduce("dabAcCaCBAcCcaDA");
            Assert.AreEqual("dabAaCBAcaDA", reduced);
            reduced = Day5.Polymer.Reduce(reduced);
            Assert.AreEqual("dabCBAcaDA", reduced);
        }

        [Test]
        public void PolymerReduce()
        {
            var polymer = new Day5.Polymer("dabAcCaCBAcCcaDA");
            polymer.Reduce();
            Assert.AreEqual("dabCBAcaDA", polymer.Units);
        }

        [Test]
        public void RemoveUnit()
        {
            var polymer = new Day5.Polymer("dabAcCaCBAcCcaDA");
            polymer.RemoveUnit('a');
            Assert.AreEqual("dbcCCBcCcD", polymer.Units);

            polymer = new Day5.Polymer("dabAcCaCBAcCcaDA");
            polymer.RemoveUnit('A');
            Assert.AreEqual("dbcCCBcCcD", polymer.Units);
        }
    }
}