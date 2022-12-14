using AnyThings;

namespace AOC2022
{
    internal class Day14 : DayPattern<Dictionary<(int, int), int>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine).ToList();

            data = new();
            var MaxXStone = 0;
            foreach (var s in text)
            {
                var ss = s.Split(" -> ");
                var pt = ss[0].Split(',');
                (int, int) p = (int.Parse(pt[0]), int.Parse(pt[1]));
                for (var i = 1; i < ss.Length; i++)
                {
                    pt = ss[i].Split(',');
                    var np = (int.Parse(pt[0]), int.Parse(pt[1]));
                    var dX = p.Item1 > np.Item1 ? -1 : 1;
                    var dY = p.Item2 > np.Item2 ? -1 : 1;
                    for (var x = p.Item1; x != np.Item1 + dX; x += dX)
                        for (var y = p.Item2; y != np.Item2 + dY; y += dY)
                        {
                            addStone(x, y);
                            if (y > MaxYStone)
                                MaxYStone = y;
                            if (x > MaxXStone)
                                MaxXStone = x;
                        }
                    p = np;
                }
            }
            for (var x = 0; x < MaxXStone * MaxYStone; ++x)
                addStone(x, MaxYStone + 2);
        }

        public override string PartOne()
        {
            var x = 500;
            var y = 0;
            while (MoveSand(x, y) != -1) ;
            return StopCount.ToString();
        }

        public override string PartTwo()
        {
            foreach (var d in data)
                if (d.Value == 2)
                    data.Remove(d.Key);
            MaxYStone = MaxYStone + 3;
            StopCount = 0;
            var x = 500;
            var y = 0;
            while (MoveSand(x, y) != 0) ;
            return StopCount.ToString();
        }

        void addStone(int x, int y)
        {
            if (!data.ContainsKey((x, y)))
                data.Add((x, y), 1);
        }

        void addSand(int x, int y)
        {
            data.Add((x, y), 2);
        }

        /// <returns>Move count or -1 if break sand</returns>
        int MoveSand(int x, int y)
        {
            if (y >= MaxYStone)
            {
                data.Remove((x, y));
                return -1;
            }
            if (!data.ContainsKey((x, y + 1)))
                return MoveSand(x, y, x, y + 1);
            if (!data.ContainsKey((x - 1, y + 1)))
                return MoveSand(x, y, x - 1, y + 1);
            if (!data.ContainsKey((x + 1, y + 1)))
                return MoveSand(x, y, x + 1, y + 1);
            ++StopCount;
            return 0;
        }

        int MoveSand(int xOld, int yOld, int xNew, int yNew)
        {
            data.Remove((xOld, yOld));
            addSand(xNew, yNew);
            var m = MoveSand(xNew, yNew);
            if (m >= 0)
                return m + 1;
            return -1;
        }

        int MaxYStone = int.MinValue;

        int StopCount;
    }
}