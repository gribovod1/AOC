using AnyThings;

namespace AOC2022
{
    internal class Day13 : DayPattern<List<(Range13, Range13)>>
    {
        public override void Parse(string path)
        {
            var text = File.ReadAllText(path).Split(Environment.NewLine + Environment.NewLine);
            data = new List<(Range13, Range13)>();
            foreach (var pt in text)
            {
                var ss = pt.Split(Environment.NewLine);
                data.Add((new Range13(ss[0]), new Range13(ss[1])));
            }
        }

        public override string PartOne()
        {
            var result = 0;
            for (var i = 0; i < data.Count; ++i)
            {
                var c = data[i].Item1.Compare(data[i].Item2);
                if (c > 0)
                    result += i + 1;
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            List<Range13> work = new List<Range13>();
            foreach (var pt in data)
            {
                work.Add(pt.Item1);
                work.Add(pt.Item2);
            }
            work.Add(new Range13("[[2]]"));
            work.Add(new Range13("[[6]]"));
            work.Sort();
            int index1 = 0;
            int index2 = 0;
            for (var i = 0; i < work.Count; ++i)
            {
                if (work[i].Source == "[[2]]")
                    index1 = i + 1;
                else if (work[i].Source == "[[6]]")
                    index2 = i + 1;
            }
            return (index1 * index2).ToString();
        }
    }

    class Range13 : IComparable
    {
        public int Compare(Range13 range)
        {
            if (!IsRange())
            {
                if (!range.IsRange())
                {
                    if (value > range.value) return -1;
                    if (value < range.value) return 1;
                    return 0;
                }
                ToRange();
            }
            if (!range.IsRange())
                range.ToRange();
            var i = 0;
            for (; i < ranges.Count; ++i)
                if (i >= range.ranges.Count)
                    return -1;
                else
                {
                    var c = ranges[i].Compare(range.ranges[i]);
                    if (c != 0) return c;
                }
            if (i < range.ranges.Count)
                return 1;
            return 0;
        }

        public int CompareTo(object? obj)
        {
            return -Compare(obj as Range13);
        }

        void LoadFromString(string v, ref int position)
        {
            var start = position;
            if (string.IsNullOrEmpty(v))
                return;
            ranges = new List<Range13>();
            ++position;
            while (position < v.Length)
            {
                if (v[position] == '[')
                    ranges.Add(new Range13(v, ref position));
                else if (v[position] == ',')
                    ++position;
                else if (v[position] == ']')
                {
                    Source = v.Substring(start, position - start + 1);
                    ++position;
                    return;
                }
                else
                {
                    var end = v.IndexOf(',', position);
                    var end2 = v.IndexOf(']', position);
                    if (end == -1 || end > end2)
                        end = end2;
                    ranges.Add(new Range13(int.Parse(v.Substring(position, end - position))));
                    position = end;
                }
            }
        }

        bool IsRange()
        {
            return ranges != null;
        }

        void ToRange()
        {
            ranges = new List<Range13> { new Range13(value) };
        }

        public Range13(int? value)
        {
            this.value = value;
        }

        public Range13(string v, ref int position)
        {
            LoadFromString(v, ref position);
        }

        public Range13(string v)
        {
            int pos = 0;
            LoadFromString(v, ref pos);
        }

        public List<Range13>? ranges = null;

        public int? value;

        public string Source;
    }
}