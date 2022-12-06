using AnyThings;
using System.Collections.Generic;

namespace AOC2021
{
    class Day9 : DayPattern<char[,]>
    {
        public override void Parse(string path)
        {
            data= AnyThings.Parse.PathToCharMap(path);
        }

        public override string PartOne()
        {
            var result = 0;
            for (var y = 0; y < data.GetLength(0); ++y)
                for (var x = 0; x < data.GetLength(1); ++x)
                    if (isLow(data, x, y))
                        result += 1 + (data[x,y] - 0x30);
            return result.ToString();
        }
        public override string PartTwo()
        {
            var bs = new List<int>();
            for (var y = 0; y < data.GetLength(0); ++y)
                for (var x = 0; x < data.GetLength(1); ++x)
                    if (isLow(data, x, y))
                        bs.Add(calcBasin(data, x, y));
            bs.Sort();
            return (bs[bs.Count - 1] * bs[bs.Count - 2] * bs[bs.Count - 3]).ToString();
        }

        bool isLow(char[,] data, int x, int y)
        {
            var value = data[x, y];
            if (value >= '9')
                return false;
            if (y > 0 && data[x, y - 1] <= value)
                return false;
            if (y < data.GetLength(0) - 1 && data[x, y + 1] <= value)
                return false;
            if (x > 0 && data[x - 1, y] <= value)
                return false;
            if (x < data.GetLength(1) - 1 && data[x + 1, y] <= value)
                return false;
            return true;
        }

        int calcBasin(char[,] data, int x, int y)
        {
            var value = data[x, y];
            if (value >= '9')
                return 0;
            data[x, y] = 'x';
            int result = 1;
            if (y > 0)
                result += calcBasin(data, x, y - 1);
            if (y < data.GetLength(0) - 1)
                result += calcBasin(data, x, y + 1);
            if (x > 0)
                result += calcBasin(data, x - 1, y);
            if (x < data.GetLength(1) - 1)
                result += calcBasin(data, x + 1, y);
            return result;
        }

    }
}