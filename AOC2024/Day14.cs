using AnyThings;
using Coord = (long x, long y);
using Robot = ((long x, long y) position, (long x, long y) speed);

namespace AOC2024
{
    internal class Day14 : DayPattern<List<Robot>>
    {
        public override void Parse(string singleText)
        {
            string[] robots = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (int l = 0; l < robots.Length; l++)
            {
                var pText = robots[l].Split(new string[] { "p=", ",", " v=" }, StringSplitOptions.RemoveEmptyEntries);
                data.Add(((long.Parse(pText[0]), long.Parse(pText[1])),
                    (long.Parse(pText[2]), long.Parse(pText[3]))));
            }
        }

        public override string PartOne()
        {
            long result = 0;
            int[] q = new int[4];
            for (int l = 0; l < data.Count; ++l)
            {
                int qIndex = Calc((101, 103), data[l], 100);
                if (qIndex >= 0)
                    q[qIndex]++;
            }
            result = q[0] * q[1] * q[2] * q[3];
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            bool end = false;
            while (!end)
            {
                for (int r = 0; r < data.Count; ++r)
                    data[r] = move((101, 103), data[r]);
                ++result;

                if (NeedShow2())
                {
                    Console.Clear();
                    for (int r = 0; r < data.Count; ++r)
                    {
                        Console.SetCursorPosition((int)data[r].position.x, (int)data[r].position.y);
                        Console.Write('*');
                    }
                    Console.SetCursorPosition(0, 104);
                    Console.Write(result);

                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.E) end = true;
                }
            }
            return result.ToString();
        }

        int Calc(Coord size, Robot m, int steps)
        {
            Coord endPosition = ((m.position.x + m.speed.x * steps + size.x * steps) % size.x, (m.position.y + m.speed.y * steps + size.y * steps) % size.y);

            if (endPosition.x < size.x / 2)
            {
                if (endPosition.y < size.y / 2)
                    return 0;
                else if (endPosition.y > size.y / 2)
                    return 1;
            }
            else if (endPosition.x > size.x / 2)
            {
                if (endPosition.y < size.y / 2)
                    return 2;
                else if (endPosition.y > size.y / 2)
                    return 3;
            }
            return -1;
        }

        Robot move(Coord size, Robot m)
        {
            Coord endPosition = ((m.position.x + m.speed.x + size.x) % size.x, (m.position.y + m.speed.y + size.y) % size.y);
            return (endPosition, m.speed);
        }

        bool NeedShow()
        {
            int[] countsInColumn = new int[101];

            for (int r = 0; r < data.Count; ++r)
                countsInColumn[data[r].position.x]++;

            var lCount = countsInColumn.ToList();
            lCount.Sort();

            int count = 0;
            for (int c = lCount.Count * 2 / 3; c < lCount.Count; ++c)
                count += lCount[c];

            return count > data.Count * 0.8;
        }

        bool NeedShow2()
        {
            int count = 0;
            for (int r = 0; r < data.Count; ++r)
                if (data[r].position.x >= 23 && data[r].position.x <= 53 &&
                    data[r].position.y >= 25 && data[r].position.y <= 57)
                    ++count;
            return count > data.Count * 0.5;
        }
    }
}