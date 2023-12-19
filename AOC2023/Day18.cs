using System.Text;
using AnyThings;

namespace AOC2023
{
    internal class Day18 : DayPattern<(char d, int l, int c)[]>
    {
        (int d, int l)[]? data2;
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new (char d, int l, int c)[text.Length];
            data2 = new (int d, int l)[text.Length];
            for (var s = 0; s < text.Length; ++s)
            {
                var row = text[s].Split(new char[] { ' ', '#', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                data[s] = (row[0][0], int.Parse(row[1]), int.Parse(row[2], System.Globalization.NumberStyles.HexNumber));

                data2[s] = (row[2][5] - '0', int.Parse(row[2].Substring(0, 5), System.Globalization.NumberStyles.HexNumber));
            }
        }

        public override string PartOne()
        {
            /* Сохраняет координаты стен и производит заполнение до этих стен через последовательную проверку соседних клеток
             */

            HashSet<(int x, int y)> Lagoon = new();
            (int x, int y) current = (0, 0);
            Lagoon.Add(current);
            foreach (var row in data)
            {
                for (int i = 0; i < row.l; ++i)
                {
                    switch (row.d)
                    {
                        case 'R':
                            {
                                current = (current.x + 1, current.y);
                                break;
                            }
                        case 'D':
                            {
                                current = (current.x, current.y + 1);
                                break;
                            }
                        case 'L':
                            {
                                current = (current.x - 1, current.y);
                                break;
                            }
                        case 'U':
                            {
                                current = (current.x, current.y - 1);
                                break;
                            }
                    }
                    Lagoon.Add(current);
                }
            }

            current = (1, 1);
            Queue<(int x, int y)> points = new();
            points.Enqueue(current);
            while (points.Count > 0)
            {
                var c = points.Dequeue();
                if (CheckAndAdd(Lagoon, points, c))
                {
                    Lagoon.Add(c);
                }
            }

            Int64 result = Lagoon.Count;
            return result.ToString();
        }

        private bool CheckAndAdd(HashSet<(int x, int y)> lagoon, Queue<(int x, int y)> points, (int x, int y) c)
        {
            if (lagoon.Contains(c)) return false;
            points.Enqueue((c.x + 1, c.y));
            points.Enqueue((c.x - 1, c.y));
            points.Enqueue((c.x, c.y + 1));
            points.Enqueue((c.x, c.y - 1));
            return true;
        }

        public override string PartTwo()
        {
            /* Создаёт список горизонтальных лучей при проходе по инструкциям.
             * Потом обрабатывает каждую линию, сортируя узловые точки по координате x.
             * При этом заполнением будет промежуток между лучом, начинающимся в меньшей точке направленным вправо
             * и начинающимся в следующей точке направленным влево.
             */

            Dictionary<Int64, List<(Int64 x, bool toRight)>> Rays = new();
            (Int64 x, Int64 y) current = (0, 0);
            foreach (var go in data2)
            {
                switch (go.d)
                {
                    case  0:
                        {
                            AddHorizontalRay(Rays, current.y, current.x, current.x + go.l);
                            current = (current.x + go.l, current.y);
                            break;
                        }
                    case 1:
                        {
                            AddVerticalLine(Rays, current.x, current.y, current.y + go.l);
                            current = (current.x, current.y + go.l);
                            break;
                        }
                    case 2:
                        {
                            AddHorizontalRay(Rays, current.y, current.x, current.x - go.l);
                            current = (current.x - go.l, current.y);
                            break;
                        }
                    case 3:
                        {
                            AddVerticalLine(Rays, current.x, current.y, current.y - go.l);
                            current = (current.x, current.y - go.l);
                            break;
                        }
                }
            }

            Int64 result = 0;
            foreach (var r in Rays)
                result += rayParse(r.Value);
            return result.ToString();
        }

        Int64 rayParse(List<(Int64 x, bool toRight)> rays)
        {
            Int64 result = 0;
            rays.Sort(RaySorter);
            int index = 0;
            while (index < rays.Count)
            {
                var ray = rays[index];
                if (ray.toRight)
                {
                    if (index > 0 && rays[index - 1].x == rays[index].x && !rays[index - 1].toRight)
                        result--;
                    while (index < rays.Count && rays[index].toRight)
                        ++index;
                    if (index < rays.Count)
                        result += rays[index].x - ray.x + 1;
                }
                ++index;
            }
            return result;
        }

        int RaySorter((Int64 x, bool toRight) x, (Int64 x, bool toRight) y)
        {
            int result = x.x.CompareTo(y.x);
            if (result == 0)
                return x.toRight.CompareTo(y.toRight);
            return result;
        }

        private void AddVerticalLine(Dictionary<long, List<(Int64 x, bool toRight)>> Rays, long x, long start, long end)
        {
            bool toRight = end < start;
            Int64 yMin = Math.Min(start, end);
            Int64 yMax = Math.Max(start, end);
            for (long y = yMin; y <= yMax; ++y)
            {
                if (!Rays.ContainsKey(y))
                    Rays.Add(y, new List<(Int64 x, bool isStart)>());
                Rays[y].Add((x, toRight));
            }
        }

        private void AddHorizontalRay(Dictionary<long, List<(Int64 x, bool toRight)>> Rays, long y, long start, long end)
        {
            Int64 xMin = Math.Min(start, end);
            Int64 xMax = Math.Max(start, end);
            if (!Rays.ContainsKey(y))
                Rays.Add(y, new List<(Int64 x, bool isStart)>());
            Rays[y].Add((xMin, true));
            Rays[y].Add((xMax, false));
        }
    }
}