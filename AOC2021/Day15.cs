using AnyThings;
using System.Collections.Generic;

namespace AOC2021
{
    class Day15 : DayPattern<int[,]>
    {
        public override void ParseFile(string path)
        {
            data = AnyThings.Parse.ParseToIntMap(path);
        }

        public override string PartOne()
        {
            var tmp = new int[data.GetLength(0), data.GetLength(1)];
            for (var yc = 0; yc < data.GetLength(1); ++yc)
                for (var xc = 0; xc < data.GetLength(0); ++xc)
                    tmp[xc, yc] = int.MaxValue;
            tmp[0, 0] = 0;

            int minIndex = 0;
            List<KeyValuePair<int, int>> checkList = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(0, 0) };
            while (checkList.Count > 0)
            {
                checkPoint(tmp, checkList[minIndex].Key, checkList[minIndex].Value, checkList);
                checkList.RemoveAt(minIndex);
                if (checkList.Count > 0)
                {
                    var min = int.MaxValue;
                    for (var i = 0; i < checkList.Count; ++i)
                        if (tmp[checkList[i].Key, checkList[i].Value] < min)
                        {
                            minIndex = i;
                            min = tmp[checkList[i].Key, checkList[i].Value];
                        }
                }
            } 
            return (tmp[tmp.GetLength(0) - 1, tmp.GetLength(1) - 1] * -1).ToString();
        }

        public bool checkPoint(int[,] tmp, int x, int y, List<KeyValuePair<int,int>> checkList)
        {
            var result = false;
            if (tmp[x, y] < 0 || tmp[x, y] == int.MaxValue)
                return false;

            if (x < tmp.GetLength(0) - 1 && tmp[x + 1, y] >= 0)
                result |= checkNextPoint(tmp, tmp[x, y], x + 1, y, checkList);
            if (x > 0 && tmp[x - 1, y] >= 0)
                result |= checkNextPoint(tmp, tmp[x, y], x - 1, y, checkList);
            if (y < tmp.GetLength(1) - 1 && tmp[x, y + 1] >= 0)
                result |= checkNextPoint(tmp, tmp[x, y], x, y + 1, checkList);
            if (y > 0 && tmp[x, y - 1] >= 0)
                result |= checkNextPoint(tmp, tmp[x, y], x, y - 1, checkList);
            tmp[x, y] *= -1;
            return result;
        }

        bool checkNextPoint(int[,] tmp, int value, int x, int y, List<KeyValuePair<int, int>> checkList)
        {
            if (tmp[x, y] < int.MaxValue)
            {
                var l = value + data[x, y];
                if (l < tmp[x, y])
                {
                    tmp[x, y] = l;
                    return true;
                }
            }
            else
            {
                tmp[x, y] = value + data[x, y];
                checkList.Add(new KeyValuePair<int, int>(x, y));
                return true;
            }
            return false;
        }

        public override string PartTwo()
        {
            var newData = new int[data.GetLength(0) * 5, data.GetLength(1) * 5];
            for (var dy = 0; dy < 5; ++dy)
                for (var dx = 0; dx < 5; ++dx)
                {
                    for (var x = 0; x < data.GetLength(0); ++x)
                        for (var y = 0; y < data.GetLength(1); ++y)
                        {
                            var v = data[x, y] + dx + dy;
                            if (v > 9)
                                v = v - 9;
                            newData[dx * data.GetLength(0) + x, dy * data.GetLength(1) + y] = v;
                        }
                }
            data = newData;
            return PartOne();
        }
    }
}