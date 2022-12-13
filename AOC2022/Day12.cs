using AnyThings;
using System.Drawing;
using System.Numerics;

namespace AOC2022
{
    internal class Day12 : DayPattern<int[,]>
    {
        public override void Parse(string path)
        {
            var text = File.ReadAllText(path).Split(Environment.NewLine);
            data = new int[text[0].Length, text.Length];
            for (var x = 0; x < text[0].Length; ++x)
                for (var y = 0; y < text.Length; ++y)
                    data[x, y] = text[y][x];
        }

        public override string PartOne()
        {
            var work = new int[data.GetLength(0), data.GetLength(1)];
            for (var x = 0; x < data.GetLength(0); ++x)
                for (var y = 0; y < data.GetLength(1); ++y)
                    work[x, y] = data[x, y];

            (int xS, int yS) = FindStart('S');
            (int xE, int yE) = FindStart('E');
            work[xS, yS] = -'a';
            work[xE, yE] = 'z';

            return WaterFind(work, xE, yE).ToString();
        }

        public override string PartTwo()
        {
            var work = new int[data.GetLength(0), data.GetLength(1)];
            for (var x = 0; x < data.GetLength(0); ++x)
                for (var y = 0; y < data.GetLength(1); ++y)
                    if (data[x, y] == 'a')
                        work[x, y] = -data[x, y];
                    else work[x, y] = data[x, y];

            (int xS, int yS) = FindStart('S');
            (int xE, int yE) = FindStart('E');
            work[xS, yS] = -'a';
            work[xE, yE] = 'z';

            return WaterFind2(work, xE, yE).ToString();
        }

        private (int x, int y) FindStart(char c)
        {
            for (var x = 0; x < data.GetLength(0); ++x)
                for (var y = 0; y < data.GetLength(1); ++y)
                    if (data[x, y] == c) return (x, y);
            return (0, 0);
        }

        void GetMinLength(ref int currentMinlength, int x, int y, int xEnd, int yEnd, HashSet<(int, int)> path)
        {
            if (x == xEnd && y == yEnd)
            {
                if (path.Count < currentMinlength)
                    currentMinlength = path.Count + 1;
                return;
            }
            if (x > 0 && data[x - 1, y] - data[x, y] < 2 && !path.Contains((x - 1, y)))
            {
                var h = new HashSet<(int, int)>(path);
                h.Add((x - 1, y));
                GetMinLength(ref currentMinlength, x - 1, y, xEnd, yEnd, h);
            }
            if (x < data.GetLength(0) - 1 && data[x + 1, y] - data[x, y] < 2 && !path.Contains((x + 1, y)))
            {
                var h = new HashSet<(int, int)>(path);
                h.Add((x + 1, y));
                GetMinLength(ref currentMinlength, x + 1, y, xEnd, yEnd, h);
            }
            if (y > 0 && data[x, y - 1] - data[x, y] < 2 && !path.Contains((x, y - 1)))
            {
                var h = new HashSet<(int, int)>(path);
                h.Add((x, y));
                GetMinLength(ref currentMinlength, x, y - 1, xEnd, yEnd, h);
            }
            if (y < data.GetLength(1) - 1 && data[x, y + 1] - data[x, y] < 2 && !path.Contains((x, y + 1)))
            {
                var h = new HashSet<(int, int)>(path);
                h.Add((x, y + 1));
                GetMinLength(ref currentMinlength, x, y + 1, xEnd, yEnd, h);
            }
        }

        int WaterFind(int[,] work, int xEnd, int yEnd)
        {
            var result = 0;
            while (work[xEnd, yEnd] != -'z')
            {
                var newwork = new int[work.GetLength(0), work.GetLength(1)];
                for (var x = 0; x < work.GetLength(0); ++x)
                    for (var y = 0; y < work.GetLength(1); ++y)
                        newwork[x, y] = work[x, y];
                for (var x = 0; x < work.GetLength(0); ++x)
                    for (var y = 0; y < work.GetLength(1); ++y)
                    {
                        if (work[x, y] < 0)
                        {
                            if (x > 0 && work[x - 1, y] > 0 && work[x - 1, y] + work[x, y] < 2)
                            {
                                newwork[x - 1, y] = -work[x - 1, y];
                            }
                            if (x < work.GetLength(0) - 1 && work[x + 1, y] > 0 && work[x + 1, y] + work[x, y] < 2)
                            {
                                newwork[x + 1, y] = -work[x + 1, y];
                            }
                            if (y > 0 && work[x, y - 1] > 0 && work[x, y - 1] + work[x, y] < 2)
                            {
                                newwork[x, y - 1] = -work[x, y - 1];
                            }
                            if (y < work.GetLength(1) - 1 && work[x, y + 1] > 0 && work[x, y + 1] + work[x, y] < 2)
                            {
                                newwork[x, y + 1] = -work[x, y + 1];
                            }
                        }
                    }
                work = newwork;
                ++result;
            }
            return result;
        }

        int WaterFind2(int[,] work, int xEnd, int yEnd)
        {
            var result = 0;
            while (work[xEnd, yEnd] != -'z')
            {
                var newwork = new int[work.GetLength(0), work.GetLength(1)];
                for (var x = 0; x < work.GetLength(0); ++x)
                    for (var y = 0; y < work.GetLength(1); ++y)
                        newwork[x, y] = work[x, y];
                for (var x = 0; x < work.GetLength(0); ++x)
                    for (var y = 0; y < work.GetLength(1); ++y)
                    {
                        if (work[x, y] < 0)
                        {
                            if (x > 0 && work[x - 1, y] > 0 && work[x - 1, y] + work[x, y] < 2)
                            {
                                newwork[x - 1, y] = -work[x - 1, y];
                            }
                            if (x < work.GetLength(0) - 1 && work[x + 1, y] > 0 && work[x + 1, y] + work[x, y] < 2)
                            {
                                newwork[x + 1, y] = -work[x + 1, y];
                            }
                            if (y > 0 && work[x, y - 1] > 0 && work[x, y - 1] + work[x, y] < 2)
                            {
                                newwork[x, y - 1] = -work[x, y - 1];
                            }
                            if (y < work.GetLength(1) - 1 && work[x, y + 1] > 0 && work[x, y + 1] + work[x, y] < 2)
                            {
                                newwork[x, y + 1] = -work[x, y + 1];
                            }
                        }
                    }
                work = newwork;
                ++result;
            }
            return result;
        }
    }
}