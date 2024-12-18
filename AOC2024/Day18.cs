using AnyThings;
using Coord = (int x, int y);

namespace AOC2024
{
    internal class Day18 : DayPattern<(Coord size, HashSet<Coord> bytes, List<Coord> AllBytes)>
    {
        public override void Parse(string singleText)
        {
            Coord END_COORDINATES = (70, 70);
            int START_COUNT = 1024;

            data = new();
            data.bytes = new();
            data.AllBytes = new();
            var t = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (int r = 0; r < t.Length; r++)
            {
                var c = t[r].Split(',');
                data.AllBytes.Add((int.Parse(c[0]), int.Parse(c[1])));
            }
            for (int r = 0; r < START_COUNT; r++)
                data.bytes.Add(data.AllBytes[r]);
            data.size = END_COORDINATES;
        }

        public override string PartOne()
        {
            return ByteTest().ToString();
        }

        public override string PartTwo()
        {
            int maxByte = data.AllBytes.Count - 1;
            int minByte = data.bytes.Count - 1;

            int current = minByte;
            do
            {
                int currentByte = (maxByte + minByte) / 2;
                if (currentByte > current)
                {
                    for (int b = current + 1; b <= currentByte; b++)
                        data.bytes.Add(data.AllBytes[b]);
                }
                else if (currentByte < current)
                {
                    for (int b = currentByte + 1; b <= current; b++)
                        data.bytes.Remove(data.AllBytes[b]);
                }
                current = currentByte;
                if (minByte < maxByte)
                {
                    if (ByteTest() != 0)
                    {
                        minByte = current;
                    }
                    else
                    {
                        maxByte = current;
                    }
                }
            }
            while (minByte + 1 < maxByte);
            if (minByte != maxByte && ByteTest() != 0)
                current = maxByte;
            return $"{data.AllBytes[current].x},{data.AllBytes[current].y}";
        }

        int ByteTest()
        {
            Dictionary<Coord, int> sand = new();
            sand.Add(data.size, 0);
            bool modified = true;
            while (!sand.ContainsKey((0, 0)) && modified)
            {
                modified = false;
                var ss = sand.ToList();
                foreach (var c in ss)
                {
                    if (AddNext(sand, (c.Key.x + 1, c.Key.y))) modified = true;
                    if (AddNext(sand, (c.Key.x - 1, c.Key.y))) modified = true;
                    if (AddNext(sand, (c.Key.x, c.Key.y + 1))) modified = true;
                    if (AddNext(sand, (c.Key.x, c.Key.y - 1))) modified = true;
                    sand[c.Key]++;
                }
            }
            return modified ? sand[(data.size.x, data.size.y)] : 0;
        }

        bool AddNext(Dictionary<Coord, int> sand, Coord next)
        {
            if (sand.ContainsKey(next) || data.bytes.Contains(next) || next.x < 0 || next.y < 0 || next.x > data.size.x || next.y > data.size.y)
                return false;
            sand.Add(next, 0);
            return true;
        }
    }
}