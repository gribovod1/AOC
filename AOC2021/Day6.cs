using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;

using Day6Type = System.Collections.Generic.List<int>;

namespace AOC2021
{
    class Day6 : DayPattern<Day6Type>
    {
        public override void Parse(string path)
        {
            data = new Day6Type();
            var ss = File.ReadAllText(path).Split(',');
            foreach (var s in ss)
                data.Add(int.Parse(s));
        }

        public override string PartOne()
        {
            var fishInDay = new List<UInt64>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (var c in data)
                ++fishInDay[c];
            for (var i = 0; i < 80; ++i)
            {
                var count = fishInDay[0];
                fishInDay.RemoveAt(0);
                fishInDay.Add(count);
                fishInDay[6] += count;
            }
            UInt64 summ = 0;
            foreach (var c in fishInDay)
                summ += c;
            return summ.ToString();
        }

        public override string PartTwo()
        {
            var fishInDay = new List<UInt64>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (var c in data)
                ++fishInDay[c];
            for (var i = 0; i < 256; ++i)
            {
                var count = fishInDay[0];
                fishInDay.RemoveAt(0);
                fishInDay.Add(count);
                fishInDay[6] += count;
            }
            UInt64 summ = 0;
            foreach (var c in fishInDay)
                summ += c;
            return summ.ToString();
        }
    }
}
