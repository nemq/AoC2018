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
}