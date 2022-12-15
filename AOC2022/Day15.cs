using AnyThings;

namespace AOC2022
{
    internal class Day15 : DayPattern<List<(int, int, int, int)>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine).ToList();
            data = new();
            foreach (var s in text)
            {
                var ss = s.Split(new char[] { ' ', ':', ',', '=' }, StringSplitOptions.RemoveEmptyEntries);
                data.Add(new(int.Parse(ss[3]), int.Parse(ss[5]), int.Parse(ss[11]), int.Parse(ss[13])));
            }
        }

        public override string PartOne()
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            foreach (var d in data)
            {
                var p = CalcRepresent(d, TargetLine);
                if (p.Item1 < minX)
                    minX = p.Item1;
                if (p.Item2 > maxX)
                    maxX = p.Item2;
            }
            return (maxX - minX).ToString();
        }

        public override string PartTwo()
        {
            foreach (var d in data)
            {
                var r = FindUncoveredNearPerimeter(d);
                if (r.Item3)
                    return ((UInt64)r.Item1 * (UInt64)MaxCoordinate + (UInt64)r.Item2).ToString();
            }
            return "Sorry";
        }

        (int, int) CalcRepresent((int, int, int, int) coords, int target)
        {
            var lenght = Math.Abs(coords.Item3 - coords.Item1) + Math.Abs(coords.Item4 - coords.Item2);
            var lX = lenght - Math.Abs(coords.Item2 - target);
            var min = coords.Item1 - lX;
            var max = coords.Item1 + lX;
            return (min, max);
        }

        (int, int, bool) FindUncoveredNearPerimeter((int, int, int, int) coords)
        {
            var lenght = Math.Abs(coords.Item3 - coords.Item1) + Math.Abs(coords.Item4 - coords.Item2);
            for (var i = coords.Item2 - lenght - 1; i < coords.Item2 + lenght + 1; ++i)
            {
                var dx = (lenght + 1) - Math.Abs(i - coords.Item2);
                if (IsUncovered((coords.Item1 + dx, i)))
                    return (coords.Item1 + dx, i, true);
                if (IsUncovered((coords.Item1 - dx, i)))
                    return (coords.Item1 - dx, i, true);
            }
            return (-1, -1, false);
        }

        bool IsUncovered((int, int) target)
        {
            if (target.Item1 < 0 || target.Item1 > MaxCoordinate || target.Item2 < 0 || target.Item2 > MaxCoordinate) return false;
            foreach (var d in data)
                if (InArea(d, target)) return false;
            return true;
        }

        bool InArea((int, int, int, int) coords, (int, int) target)
        {
            var lenght = Math.Abs(coords.Item3 - coords.Item1) + Math.Abs(coords.Item4 - coords.Item2);
            var lenghtToTarget = Math.Abs(target.Item1 - coords.Item1) + Math.Abs(target.Item2 - coords.Item2);
            return lenghtToTarget <= lenght;
        }

        static int TargetLine = 2000000;
        static int MaxCoordinate = TargetLine * 2;
    }
}