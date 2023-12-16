using AnyThings;
using System.Xml.Linq;

namespace AOC2023
{
    internal class Day11 : DayPattern<string[]>
    {
        public override void Parse(string singleText)
        {
            data = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string PartOne()
        {
            return GetDistance(2).ToString();
        }

        public override string PartTwo()
        {
            return GetDistance(1000000).ToString();
        }

        Int64 GetDistance(int EmptyMultiplier)
        {
            /*
            Первым проходом запоминаются координаты галактик и удаляются не пустые строки и столбцы
            Вторым, считается количество пустых строк и столбцов между парами галактик, что, с
            расстоянием между исходными координатами даёт расстояние между парой галактик.
             */
            Int64 result = 0;
            HashSet<int> rowIndex = new();
            for (int y = 0; y < data.Length; ++y) rowIndex.Add(y);
            HashSet<int> colIndex = new();
            for (int y = 0; y < data[0].Length; ++y) colIndex.Add(y);
            List<(int x, int y)> Coordinates = new();

            for (int y = 0; y < data.Length; ++y)
                for (int x = 0; x < data[y].Length; ++x)
                    if (data[y][x] != '.')
                    {
                        rowIndex.Remove(y);
                        colIndex.Remove(x);
                        Coordinates.Add((x, y));
                    }

            for (int i = 0; i < Coordinates.Count; ++i)
                for (int j = i + 1; j < Coordinates.Count; ++j)
                {
                    int countRowEmptySpace = 0;
                    int countColEmptySpace = 0;
                    int minX = Math.Min(Coordinates[i].x, Coordinates[j].x);
                    int minY = Math.Min(Coordinates[i].y, Coordinates[j].y);
                    int maxX = Math.Max(Coordinates[i].x, Coordinates[j].x);
                    int maxY = Math.Max(Coordinates[i].y, Coordinates[j].y);
                    var ci = colIndex.GetEnumerator();
                    while (ci.MoveNext())
                        if (ci.Current > minX && ci.Current < maxX) countColEmptySpace++;
                    var ri = rowIndex.GetEnumerator();
                    while (ri.MoveNext())
                        if (ri.Current > minY && ri.Current < maxY) countRowEmptySpace++;
                    result += maxX - minX + maxY - minY + countColEmptySpace * (EmptyMultiplier - 1) + countRowEmptySpace * (EmptyMultiplier - 1);
                }

            return result;
        }
    }
}