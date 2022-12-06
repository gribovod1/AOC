using AnyThings;
using System;
using System.IO;

using Day7Type = System.Collections.Generic.List<int>;

namespace AOC2021
{
    class Day7 : DayPattern<Day7Type>
    {
        public override void Parse(string path)
        {
            data = new Day7Type();
            var ss = File.ReadAllText(path).Split(',');
            foreach (var s in ss)
                data.Add(int.Parse(s));
            data.Sort();
        }

        public override string PartOne()
        {
            var max = data[data.Count - 1];
            var min = data[0];
            var result = int.MaxValue;
            for (var c = min; c <=max; ++c)
            {
                var summ = 0;
                foreach(var v in data)
                {
                    summ += Math.Abs(v - c);
                }
                if (summ < result)
                    result = summ;
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            var max = data[data.Count - 1];
            var min = data[0];
            var result = int.MaxValue;
            for (var c = min; c <= max; ++c)
            {
                var summ = 0;
                foreach (var v in data)
                {
                    summ += ((1 + Math.Abs(v - c)) * Math.Abs(v - c)) / 2;
                }
                if (summ < result)
                    result = summ;
            }
            return result.ToString();
        }
    }
}
