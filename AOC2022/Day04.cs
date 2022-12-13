using AnyThings;

namespace AOC2022
{
    class Range
    {
        public int OneStart;
        public int OneEnd;
        public int TwoStart;
        public int TwoEnd;

        public Range(string s)
        {
            var sp = s.Split(',');
            var n = sp[0].Split('-');
            OneStart = int.Parse(n[0]);
            OneEnd = int.Parse(n[1]);
            n = sp[1].Split('-');
            TwoStart = int.Parse(n[0]);
            TwoEnd = int.Parse(n[1]);
        }

        public bool isInner()
        {
            if ((OneStart >= TwoStart && OneStart <= TwoEnd) &&
                (OneEnd <= TwoEnd && OneEnd >= TwoStart))
                return true;
            if ((TwoStart >= OneStart && TwoStart <= OneEnd) &&
                (TwoEnd <= OneEnd && TwoEnd >= OneStart))
                return true;
            return false;
        }

        public bool isIntersect()
        {
            if ((OneStart >= TwoStart && OneStart <= TwoEnd) ||
                (TwoStart >= OneStart && TwoStart <= OneEnd))
                return true;
            if ((OneEnd >= TwoStart && OneEnd <= TwoEnd) ||
                (TwoEnd >= OneStart && TwoEnd <= OneEnd))
                return true;
            return false;
        }
    }

    internal class Day04 : DayPattern<List<Range>>
    {
        public override void Parse(string path)
        {
            var ss = File.ReadAllText(path).Split(Environment.NewLine);
            data = new List<Range>();
            foreach(var s in ss)
                data.Add(new Range(s));
        }

        public override string PartOne()
        {
            int result = 0;
            foreach (var r in data)
                if (r.isInner())
                    ++result;
            return result.ToString();
        }

        public override string PartTwo()
        {
            int result = 0;
            foreach (var r in data)
                if (r.isIntersect())
                    ++result;
            return result.ToString();
        }
    }
}