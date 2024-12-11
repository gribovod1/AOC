using AnyThings;
using Coord = (int r, int c);

namespace AOC2024
{
    internal class Day10 : DayPattern<(string[] map, List<(int r, int c)> coords)>
    {
        public override void Parse(string singleText)
        {
            string[] map = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            List<(int r, int c)> coords = new();
            for (int r = 0; r < map.Length; ++r)
                for (int c = 0; c < map[r].Length; ++c)
                    if (map[r][c] == '0')
                        coords.Add((r, c));
            data = (map, coords);
        }

        public override string PartOne()
        {
            long result = 0;
            for (var c = 0; c < data.coords.Count; ++c)
                result += CalcTrace(data.coords[c]);
            return result.ToString();
        }

        int CalcTrace(Coord start)
        {
            HashSet<Coord> ends = new();
            Trace(ends, start, '/');
            return ends.Count;
        }

        bool Trace(HashSet<Coord> ends, Coord point, char prev)
        {
            if (OutCoord(point)) return false;
            char current = data.map[point.r][point.c];
            if (current - prev != 1) return false;
            if (current == '9') return ends.Add(point);
            Trace(ends, (point.r + 1, point.c), current);
            Trace(ends, (point.r, point.c + 1), current);
            Trace(ends, (point.r - 1, point.c), current);
            Trace(ends, (point.r, point.c - 1), current);
            return false;
        }

        private bool OutCoord((int r, int c) point)
        {
            return point.r < 0 || point.c < 0 || point.r >= data.map.Length || point.c >= data.map[0].Length;
        }

        public override string PartTwo()
        {
            long result = 0;
            List<(int r, int c)> coords = new();
            for (int r = 0; r < data.map.Length; ++r)
                for (int c = 0; c < data.map[r].Length; ++c)
                    if (data.map[r][c] == '0')
                        coords.Add((r, c));

            for (var c = 0; c < coords.Count; ++c)
                result += CalcTrace2(coords[c]);

            return result.ToString();
        }

        int CalcTrace2(Coord start)
        {
            return Trace2(start, '/');
        }

        int Trace2(Coord point, char prev)
        {
            int result = 0;
            if (OutCoord(point)) return 0;
            char current = data.map[point.r][point.c];
            if (current - prev != 1) return 0;
            if (current == '9') return 1;
            result += Trace2((point.r + 1, point.c), current);
            result += Trace2((point.r, point.c + 1), current);
            result += Trace2((point.r - 1, point.c), current);
            result += Trace2((point.r, point.c - 1), current);
            return result;
        }
    }
}