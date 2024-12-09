using AnyThings;
using Day07Item = (long result, string resultText, System.Collections.Generic.List<int> operands);

namespace AOC2024
{
    internal class Day07 : DayPattern<List<Day07Item>>
    {
        public override void Parse(string singleText)
        {
            var ss = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            foreach (var s in ss)
            {
                var rt = s.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var l = new List<int>();
                for (var nText = 1; nText < rt.Length; ++nText)
                    l.Add(int.Parse(rt[nText]));
                data.Add((long.Parse(rt[0]), rt[0], l));
            }
        }

        public override string PartOne()
        {
            long result = 0;
            foreach (var s in data)
                if (fullCheck(s, false))
                    result += s.result;
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            foreach (var s in data)
                if (fullCheck(s, true))
                    result += s.result;
            return result.ToString();
        }

        private bool fullCheck(Day07Item s, bool useSplit)
        {
            return treeCheck(s, s.operands[0], 1, useSplit);
        }

        private bool treeCheck(Day07Item s, long currentResult, int index, bool useSplit)
        {
            if (index == s.operands.Count) return currentResult == s.result;
            if (treeCheck(s, currentResult * s.operands[index], index + 1, useSplit))
                return true;
            if (treeCheck(s, currentResult + s.operands[index], index + 1, useSplit))
                return true;
            return useSplit && treeCheck(s, long.Parse(currentResult.ToString() + s.operands[index].ToString()), index + 1, useSplit);
        }
    }
}