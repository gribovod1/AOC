using AnyThings;

namespace AOC2024
{
    internal class Day08 : DayPattern<(char[][] map, HashSet<(int x, int y)> antinodes, Dictionary<char, List<(int x, int y)>> types)>
    {
        public override void Parse(string singleText)
        {
            var ss = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data.map = new char[ss.Length][];
            data.types = new();
            data.antinodes = new();
            for (var s = 0; s < ss.Length; ++s)
            {
                data.map[s] = ss[s].ToCharArray();
                for (int c = 0; c < data.map[s].Length; ++c)
                    if (data.map[s][c] != '.')
                    {
                        if (!data.types.ContainsKey(data.map[s][c]))
                            data.types.Add(data.map[s][c], new List<(int x, int y)>());
                        data.types[data.map[s][c]].Add((c, s));
                    }
            }
        }

        public override string PartOne()
        {
            long result = 0;
            foreach (var t in data.types)
            {
                for (var i = 0; i < t.Value.Count; ++i)
                    for (var i1 = i + 1; i1 < t.Value.Count; ++i1)
                        setAntinode(t.Value[i], t.Value[i1], t.Key);
            }
            result = data.antinodes.Count;
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            foreach (var t in data.types)
            {
                for (var i = 0; i < t.Value.Count; ++i)
                    for (var i1 = i + 1; i1 < t.Value.Count; ++i1)
                        setAntinode2(t.Value[i], t.Value[i1], t.Key);
            }
            result = data.antinodes.Count;
            return result.ToString();
        }

        private void setAntinode((int x, int y) value1, (int x, int y) value2, char t)
        {
            int dx = (value2.x - value1.x);
            int dy = (value2.y - value1.y);
            if (checkCoord((value2.x + dx, value2.y + dy), t))
                data.antinodes.Add((value2.x + dx, value2.y + dy));
            if (checkCoord((value1.x - dx, value1.y - dy), t))
                data.antinodes.Add((value1.x - dx, value1.y - dy));
        }

        private void setAntinode2((int x, int y) value1, (int x, int y) value2, char t)
        {
            int dx = (value2.x - value1.x);
            int dy = (value2.y - value1.y);

            int i = 0;
            while (checkCoord((value2.x + dx * i, value2.y + dy * i), t))
            {
                data.antinodes.Add((value2.x + dx * i, value2.y + dy * i));
                ++i;
            }
            i = 0;
            while (checkCoord((value1.x - dx * i, value1.y - dy * i), t))
            {
                data.antinodes.Add((value1.x - dx * i, value1.y - dy * i));
                ++i;
            }
        }

        bool checkCoord((int x, int y) coord, char t)
        {
            return coord.x >= 0 && coord.y >= 0 && coord.x < data.map[0].Length && coord.y < data.map.Length;
        }
    }
}