using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2018
{
    using GuardId = UInt32;
    using MinutesHistogram = Dictionary<int, int>;
    public class Day4 : IDay
    {
        private Dictionary<GuardId, MinutesHistogram> guardsNaps = new Dictionary<GuardId, MinutesHistogram>();

        public Day4()
        {
            GuardId guardId = 0;
            int startMinute = 0, endMinute = 0, duration = 0;
            var records = ReadRecordsSorted(Utilities.InputPath(4));
            foreach (var record in records.Values)
            {
                switch (record.Type)
                {
                    case Record.MessageType.BeginShift:
                        guardId = (GuardId) record.GuardId;
                        guardsNaps.TryAdd(guardId, new MinutesHistogram());
                        break;
                    case Record.MessageType.FallAsleep:
                        startMinute = record.Date.Minute;
                        break;
                    case Record.MessageType.WakesUp:
                        endMinute = record.Date.Minute;
                        duration = endMinute - startMinute;
                        var histogram = guardsNaps[guardId];
                        foreach (var m in Enumerable.Range(startMinute, duration))
                        {
                            if (histogram.ContainsKey(m))
                            {
                                histogram[m] += 1;
                            }
                            else
                            {
                                histogram.Add(m, 1);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        public string FirstPuzzle()
        {
            var guardsTotalAsleepTime = guardsNaps.Select(
                kv => new KeyValuePair<GuardId, int>(kv.Key, kv.Value.Values.Sum()));
            var longestTotalAsleepGuardId = guardsTotalAsleepTime.Aggregate(
                (curr, max) => curr.Value > max.Value ? curr : max).Key;
            var mostOftenSleptMinute = guardsNaps[longestTotalAsleepGuardId].Aggregate(
                (curr, max) => curr.Value > max.Value ? curr : max).Key;

            return (longestTotalAsleepGuardId * mostOftenSleptMinute).ToString();
        }

        public string SecondPuzzle()
        {

            var gaurdsMostSleptMinute  = guardsNaps.
                Where(kv => kv.Value.Count != 0).
                Select(kv => (kv.Key, kv.Value.Aggregate(
                    (l, r) => l.Value > r.Value ? l : r)));

            var guardWithMostSleptMinute = gaurdsMostSleptMinute.
                Aggregate((l, r) => l.Item2.Value > r.Item2.Value ? l : r);

            var guardId = guardWithMostSleptMinute.Item1;
            var mostSleptMinute = guardWithMostSleptMinute.Item2.Key;
            return (guardId * mostSleptMinute).ToString();
        }

        private SortedList<DateTime, Record> ReadRecordsSorted(string path)
        {
            var records = new SortedList<DateTime, Record>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var record = new Record(reader.ReadLine());
                    records.Add(record.Date, record);
                }
            }

            return records;
        }

        struct Nap
        {
            public int StartMin;
            public int StopMin;
            public int Duration;
        }

        public class Record
        {
            //[1518-11-12 23:47] Guard #2777 begins shift"
            private static readonly string RecordPattern =
                @"\[(?<year>\d+)\-" +
                @"(?<month>\d+)\-" +
                @"(?<day>\d+)\s" +
                @"(?<hour>\d+):" +
                @"(?<minute>\d+)\]\s" +
                @"(?<message>[\w\s#]+)";
            private static readonly Regex RecordRegex = new Regex(RecordPattern);
            private static readonly string WakeUpMessage = "wakes up";
            private static readonly string FallAsleepMessage = "falls asleep";
            private static readonly string BeginShiftPattern = @"Guard #(?<id>\d+) begins shift";
            private static readonly Regex BeginShiftRegex = new Regex(BeginShiftPattern);

            public enum MessageType { BeginShift, FallAsleep, WakesUp };
            public Record(string record)
            {
                var match = RecordRegex.Match(record);
                if (!match.Success)
                {
                    throw new ArgumentException("Invalid record: " + record);
                }

                var year = int.Parse(match.Groups["year"].Value);
                var month = int.Parse(match.Groups["month"].Value);
                var day = int.Parse(match.Groups["day"].Value);
                var hour = int.Parse(match.Groups["hour"].Value);
                var minute = int.Parse(match.Groups["minute"].Value);
                Message = match.Groups["message"].Value;
                ParseMessage(Message);
                Date = new DateTime(year, month, day, hour, minute, 0);
            }

            private void ParseMessage(string message)
            {
                if (BeginShiftRegex.IsMatch(message))
                {
                    Type = MessageType.BeginShift;
                    var id = BeginShiftRegex.Match(message).Groups["id"].Value;
                    GuardId = int.Parse(id);
                }
                else if (message.Equals(FallAsleepMessage))
                {
                    Type = MessageType.FallAsleep;
                }
                else if (message.Equals(WakeUpMessage))
                {
                    Type = MessageType.WakesUp;
                }
                else
                {
                    throw new ArgumentException("Invalid message: " + message);
                }
            }

            public string Message { get; set; }
            public MessageType Type { get; set; }
            public int GuardId { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
