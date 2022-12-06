using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DayType = System.Collections.Generic.List<AOC2021.Day8Record>;

namespace AOC2021
{
    class Day8Record
    {
        List<string> digitCodes = new List<string>();
        List<string> output = new List<string>();
        public Day8Record(string s)
        {
            var s1 = s.Split('|');
            var ssInput = s1[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var si in ssInput)
                digitCodes.Add(si);
            var ssOutput = s1[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var si in ssOutput)
                output.Add(si);
        }

        public int simpleCount()
        {
            int result = 0;
            foreach (var si in output)
                if (si.Length == 2 || si.Length == 4 || si.Length == 3 || si.Length == 7)
                    ++result;
            return result;
        }

        internal int DecodeOutput()
        {
            var digits = getDigits();
            var result = 0;
            for (var o = 0; o < output.Count; ++o)
                result = result * 10 + digits[output[o].Sort()];
            return result;
        }

        private Dictionary<string, int> getDigits()
        {
            string[] result = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

            var count2List = getAllByCount(digitCodes, 2);
            var count3List = getAllByCount(digitCodes, 3);
            var count4List = getAllByCount(digitCodes, 4);
            var count5List = getAllByCount(digitCodes, 5);
            var count6List = getAllByCount(digitCodes, 6);
            var count7List = getAllByCount(digitCodes, 7);

            result[1] = count2List[0];
            result[4] = count4List[0];
            result[7] = count3List[0];
            result[8] = count7List[0];

            var index = findByCrossCount(count5List, result[1], result[1].Length);
            result[3] = count5List[index];
            count5List.RemoveAt(index);

            index = findByCrossCount(count5List, result[4], 3);
            result[5] = count5List[index];
            count5List.RemoveAt(index);
            result[2] = count5List[0];

            index = findByCrossCount(count6List, result[3], result[3].Length);
            result[9] = count6List[index];
            count6List.RemoveAt(index);

            index = findByCrossCount(count6List, result[1], result[1].Length);
            result[0] = count6List[index];
            count6List.RemoveAt(index);
            result[6] = count6List[0];

            var d = new Dictionary<string, int>();
            for (var rIndex = 0; rIndex < result.Length; ++rIndex)
                d.Add(result[rIndex].Sort(), rIndex);
            return d;
        }

        int findByCrossCount(List<string> input, string pattern, int countCross)
        {
            var result = string.Empty;
            for (var s = 0; s < input.Count; ++s)
                if (input[s].Intersect(pattern).Count() == countCross)
                    return s;
            return -1;
        }

        List<string> getAllByCount(List<string> input, int v)
        {
            List<string> result = new List<string>();
            for (var s = 0; s < input.Count; ++s)
                if (input[s].Length == v)
                    result.Add(input[s]);
            return result;
        }
    }

    class Day8 : DayPattern<DayType>
    {
        public override void Parse(string path)
        {
            data = new DayType();

            var ss = File.ReadAllLines(path);
            foreach (var s in ss)
                data.Add(new Day8Record(s));
        }

        public override string PartOne()
        {
            var result = 0;
            foreach (var r in data)
                result += r.simpleCount();
            return result.ToString();
        }

        public override string PartTwo()
        {
            var result = 0;
            foreach (var r in data)
                result += r.DecodeOutput();
            return result.ToString();
        }
    }
}
