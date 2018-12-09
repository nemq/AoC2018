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
}