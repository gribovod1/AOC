using AnyThings;
using System.Text;

namespace AOC2024
{
    internal class Day22 : DayPattern<List<long>>
    {
        public override void Parse(string singleText)
        {
            data = new();
            var lines = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in lines)
                data.Add(long.Parse(item));
        }

        long Calc(long number)
        {
            var result = number << 6;
            result = result ^ number;
            result = result % 16777216;

            var result2 = result >> 5;
            result2 = result2 ^ result;
            result2 = result2 % 16777216;

            var result3 = result2 << 11;
            result3 = result3 ^ result2;
            result3 = result3 % 16777216;
            return result3;
        }

        long Calc(long number, long steps)
        {
            for (var i = 0; i < steps; ++i)
                number = Calc(number);
            return number;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="steps"></param>
        /// <returns>пары: подстрока(первый экземпляр) - количество бананов</returns>
        Dictionary<string, int> CalcDescription(long number, long steps, HashSet<string> uniqueSequences)
        {
            StringBuilder sb = new();
            sb.Append((char)((number % 10) + 'O'));
            var prev_number = number;
            Dictionary<string, int> result = new();
            var i = 0;
            for (i = 0; i < 3; ++i)
            {
                number = Calc(prev_number);
                sb.Append((char)((number % 10) - (prev_number % 10) + 'O'));
                prev_number = number;
            }
            for (; i < steps; ++i)
            {
                number = Calc(prev_number);
                sb.Append((char)((number % 10) - (prev_number % 10) + 'O'));
                var sub = sb.ToString(sb.Length - 4, 4);
                if (!result.ContainsKey(sub))
                {
                    result.Add(sub, (int)(number % 10));
                    uniqueSequences.Add(sub);
                }
                prev_number = number;
            }
            return result;
        }

        public override string PartOne()
        {
            long result = 0;
            foreach (var item in data)
                result += Calc(item, 2000);
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            List<Dictionary<string, int>> list = new();
            HashSet<string> subs = new();
            foreach (var item in data)
                list.Add(CalcDescription(item, 2000, subs));
            foreach (var s in subs)
            {
                long value = 0;
                foreach (var mon in list)
                    if (mon.ContainsKey(s))
                        value += mon[s];
                if (result < value)
                    result = value;
            }
            return result.ToString();
        }
    }
}