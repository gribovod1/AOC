using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2021
{
    class Day3 : DayPattern<string[]>
    {
        public override void ParseFile(string path)
        {
            data= File.ReadAllLines(path);
        }

        public override string PartOne()
        {
            int[] current = new int[data[0].Length];
            foreach (var s in data)
                for (var c = 0; c < s.Length; ++c)
                    if (s[c] == '1')
                        ++current[c];

            uint g = 0;
            uint e = 0;
            for (var c = 0; c < current.Length; ++c)
            {
                if (current[c] > data.Length - current[c])
                    g |= ((uint)1 << (current.Length - 1 - c));
                else
                    e |= ((uint)1 << (current.Length - 1 - c));
            }

            return (e * g).ToString();
        }

        public override string PartTwo()
        {
            List<string> gList = new List<string>(data);
            List<string> eList = new List<string>(data);
            for (var c = 0; c < data[0].Length; ++c)
            {
                if (gList.Count > 1)
                    gList = filter(gList, true, c);
                if (eList.Count > 1)
                    eList = filter(eList, false, c);
            }
            return (Convert.ToUInt32(gList[0], 2) * Convert.ToUInt32(eList[0], 2)).ToString();
        }

        List<string> filter(List<string> data, bool isHi, int pos)
        {
            int count_1 = 0;
            foreach (var s in data)
                if (s[pos] == '1')
                    ++count_1;

            char c = isHi
                ? (count_1 >= data.Count - count_1 ? '1' : '0')
                : (count_1 >= data.Count - count_1 ? '0' : '1');

            List<string> result = new List<string>();
            foreach (var s in data)
                if (s[pos] == c)
                    result.Add(s);
            return result;
        }
    }
}
