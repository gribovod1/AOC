using AnyThings;
using System.Drawing;
using Day06Coordinates = (int c, int r, int dc, int dr);
namespace AOC2024
{
    internal class Day06 : DayPattern<char[][]>
    {
        public override void Parse(string singleText)
        {
            var ss = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new char[ss.Length][];
            for (var i = 0; i < ss.Length; ++i)
                data[i] = ss[i].ToCharArray();
            start = FindGuard();
        }

        Day06Coordinates start;

        Day06Coordinates FindGuard()
        {
            for (int y = 0; y < data.Length; ++y)
                for (int x = 0; x < data[y].Length; ++x)
                {
                    if (data[y][x] == '^')
                        return (x, y, 0, -1);
                }
            throw new Exception();
        }

        public override string PartOne()
        {
            long result = 1;
            var coord = start;
            HashSet<Day06Coordinates> Traces = new();
            while (coord.c >= 0 && coord.c < data[0].Length && coord.r >= 0 && coord.r < data.Length)
            {
                if (step(Traces, ref coord))
                    ++result;
            }
            return result.ToString();
        }

        private bool step(HashSet<Day06Coordinates> Traces, ref Day06Coordinates coord)
        {
            if (checkOut(coord))
            {
                coord.c += coord.dc;
                coord.r += coord.dr;
                return false;
            }
            while (data[coord.r + coord.dr][coord.c + coord.dc] == '#')
            {
                if (coord.dc == 1)
                {
                    coord.dc = 0;
                    coord.dr = 1;
                }
                else if (coord.dr == 1)
                {
                    coord.dc = -1;
                    coord.dr = 0;
                }
                else if (coord.dc == -1)
                {
                    coord.dc = 0;
                    coord.dr = -1;
                }
                else if (coord.dr == -1)
                {
                    coord.dc = 1;
                    coord.dr = 0;
                }
                if (checkOut(coord))
                {
                    coord.c += coord.dc;
                    coord.r += coord.dr;
                    return false;
                }
            }
            coord.c += coord.dc;
            coord.r += coord.dr;
            Traces.Add(coord);
            MainTrace.Add(coord);
            if (data[coord.r][coord.c] == '.')
            {
                data[coord.r][coord.c] = 'X';
                return true;
            }
            return false;
        }

        private int stepCycleFind(HashSet<Day06Coordinates> Traces, ref Day06Coordinates coord)
        {
            if (checkOut(coord))
            {
                coord.c += coord.dc;
                coord.r += coord.dr;
                return -1;
            }
            while (data[coord.r + coord.dr][coord.c + coord.dc] == '#')
            {
                if (coord.dc == 1)
                {
                    coord.dc = 0;
                    coord.dr = 1;
                }
                else if (coord.dr == 1)
                {
                    coord.dc = -1;
                    coord.dr = 0;
                }
                else if (coord.dc == -1)
                {
                    coord.dc = 0;
                    coord.dr = -1;
                }
                else if (coord.dr == -1)
                {
                    coord.dc = 1;
                    coord.dr = 0;
                }
                if (checkOut(coord))
                {
                    coord.c += coord.dc;
                    coord.r += coord.dr;
                    return -1;
                }
            }
            coord.c += coord.dc;
            coord.r += coord.dr;
            return Traces.Add(coord) ? 0 : 1;
        }

        bool checkOut(Day06Coordinates coord)
        {
            return (coord.c + coord.dc < 0 || coord.c + coord.dc >= data[0].Length || coord.r + coord.dr < 0 || coord.r + coord.dr >= data.Length);
        }

        bool checkNewBarrier(int stepsCount)
        {
            var coord = start;
            HashSet<Day06Coordinates> Traces = new();
            data[MainTrace[stepsCount].r][MainTrace[stepsCount].c] = '#';
            try
            {
                while (coord.c >= 0 && coord.c < data[0].Length && coord.r >= 0 && coord.r < data.Length)
                {
                    int stepResult = stepCycleFind(Traces, ref coord);
                    if (stepResult == -1) return false;
                    if (stepResult == 1) return true;
                }
                return false;
            }
            finally
            {
                data[MainTrace[stepsCount].r][MainTrace[stepsCount].c] = '.';
            }
        }

        List<Day06Coordinates> MainTrace = new();

        public override string PartTwo()
        {
            long result = 0;
            for (int s = 0; s < MainTrace.Count; ++s)
                if (checkNewBarrier(s))
                    ++result;
            return result.ToString();
        }
    }
}