using AnyThings;
using System.Drawing;
using System.Linq;
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
            {
                if (fullCheck(s))
                    result += s.result;
            }
            return result.ToString();
        }

        private bool fullCheck(Day07Item s)
        {
            return treeCheck(s, s.operands[0], 1);
        }

        private bool treeCheck(Day07Item s, long currentResult, int index)
        {
            if (currentResult == s.result && index == s.operands.Count) return true;
            if (currentResult > s.result) return false;
            if (index >= s.operands.Count) return false;
            if (treeCheck(s, currentResult * s.operands[index], index + 1))
                return true;
            return (treeCheck(s, currentResult + s.operands[index], index + 1));
        }

        public override string PartTwo()
        {
            long result = 0;
            foreach (var s in data)
            {
                if (fullCheck2(s))
                    result += s.result;
            }
            return result.ToString();
        }

        private bool fullCheck2(Day07Item s)
        {
            return treeCheck2(s, s.operands[0], 1, false);
        }

        private bool treeCheck2(Day07Item s, long currentResult, int index, bool usedSplit)
        {
            if (index >= s.operands.Count) return currentResult == s.result;
            if (treeCheck2(s, currentResult * s.operands[index], index + 1, usedSplit))
                return true;
            if (treeCheck2(s, currentResult + s.operands[index], index + 1, usedSplit))
                return true;
            if (!usedSplit)
                return treeCheck2(s,long.Parse( currentResult.ToString() + s.operands[index].ToString()), index + 1, true);
            return false;
            if (!usedSplit)
                for (int c = 1; c < s.resultText.Length; c++)
                    if (long.Parse(s.resultText.Substring(0, c)) == currentResult)
                    {
                        if (treeCheck2((long.Parse(s.resultText.Substring(c)), s.resultText.Substring(c), s.operands.GetRange(index, s.operands.Count - index)), s.operands[index], 1, true))
                            return true;
                        return false;
                    }
            return false;
        }

    }
}