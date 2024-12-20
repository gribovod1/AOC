using AnyThings;
using Coord = (int x, int y);

namespace AOC2024
{
    internal class Day19 : DayPattern<(HashSet<string> stripes, List<string> patterns)>
    {
        public override void Parse(string singleText)
        {
            data = new();
            data.stripes = new();
            data.patterns = new();
            var sp = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var sText = sp[0].Split(", ", StringSplitOptions.RemoveEmptyEntries);
            var pText = sp[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (int s = 0; s < sText.Length; s++)
                data.stripes.Add(sText[s]);
            for (int p = 0; p < pText.Length; p++)
                data.patterns.Add(pText[p]);
        }

        public override string PartOne()
        {
            long result = 0;
            for (int p = 0; p < data.patterns.Count; ++p)
                if (Check(data.patterns[p], 0))
                    ++result;
            return result.ToString();
        }

        private bool Check(string v, int startIndex)
        {
            if (startIndex == v.Length) return true;
            for (int i = 1; i <= v.Length - startIndex; ++i)
            {
                var ss = v.Substring(startIndex, i);
                if (data.stripes.Contains(ss) && Check(v, startIndex + ss.Length)) return true;
            }
            return false;
        }



        bool CheckStart(string pattern, string text, int startIndex)
        {
            if (startIndex + pattern.Length > text.Length) return false;
            for (var i = 0; i < pattern.Length; ++i)
                if (text[startIndex + i] != pattern[i]) return false;
            return true;
        }

        public override string PartTwo()
        {
            long result = 0;
            return result.ToString();
        }
    }
}