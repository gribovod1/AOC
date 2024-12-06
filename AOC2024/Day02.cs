using AnyThings;

namespace AOC2024
{
    internal class Day02 : DayPattern<List<List<int>>>
    {
        public override void Parse(string singleText)
        {
            data = new();
            List<string> text = singleText.Split(Environment.NewLine).ToList();
            foreach (var p in text)
            {
                data.Add(new List<int>());
                var items = p.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in items)
                    data[data.Count - 1].Add(int.Parse(s));
            }
        }

        public override string PartOne()
        {
            long result = 0;
            for (int i = 0; i < data.Count; ++i)
                if (checkReport(data[i]))
                    ++result;
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            for (int i = 0; i < data.Count; ++i)
                if (checkReport2(data[i]))
                    ++result;
            return result.ToString();
        }

        bool checkReport(List<int> report)
        {
            bool up = report[0] < report[1];
            for (int l = 1; l < report.Count; ++l)
            {
                int c = report[l];
                int p = report[l - 1];
                if (Math.Abs(c - p) < 1 || Math.Abs(c - p) > 3) return false;
                if (up)
                {
                    if (c < p) return false;
                }
                else
                {
                    if (c > p) return false;
                }
            }
            return true;
        }

        bool checkReport2(List<int> report)
        {
            for (int i = 0; i < report.Count; ++i)
            {
                List<int> l = report.ToList();
                l.RemoveAt(i);
                if (checkReport(l))
                    return true;
            }
            return false;
        }
    }
}