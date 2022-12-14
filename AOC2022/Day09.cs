using AnyThings;
using System.Drawing;

namespace AOC2022
{
    internal class Day09 : DayPattern<List<KeyValuePair<char,int>>>
    {
        public override void ParseFile(string path)
        {
            var text = File.ReadAllText(path).Split(Environment.NewLine).ToList();
            data = new List<KeyValuePair<char, int>>();
            foreach(var s in text)
            {
                var sc = s.Split(' ');
                data.Add(new KeyValuePair<char, int>(sc[0][0], int.Parse(sc[1])));
            }
        }

        public override string PartOne()
        {
            InitPositions();
            Rope = new Point[2];
            for (var i = 0; i < data.Count; ++i)
                Move(data[i]);
            return positions.Count.ToString();
        }

        public override string PartTwo()
        {
            InitPositions();
            Rope = new Point[10];
            for (var i = 0; i < data.Count; ++i)
                Move(data[i]);
            return positions.Count.ToString();
        }

        void Move(KeyValuePair<char, int> step)
        {
            for (var i = 0; i < step.Value; ++i)
            {
                switch (step.Key)
                {
                    case 'L':
                        {
                            MoveHead(Rope[0].X - 1, Rope[0].Y);
                            break;
                        }
                    case 'U':
                        {
                            MoveHead(Rope[0].X, Rope[0].Y - 1);
                            break;
                        }
                    case 'R':
                        {
                            MoveHead(Rope[0].X + 1, Rope[0].Y);
                            break;
                        }
                    case 'D':
                        {
                            MoveHead(Rope[0].X, Rope[0].Y + 1);
                            break;
                        }
                }
            }
        }

        void MoveHead(int x, int y)
        {
            Rope[0].X = x;
            Rope[0].Y = y;
            for (var i = 1; i < Rope.Length; ++i)
                MoveTail(i);
        }


        int Dist(int number)
        {
            var p1 = Rope[number - 1];
            var p2 = Rope[number];
            if (Math.Abs(p1.X - p2.X) <= 1 && Math.Abs(p1.Y - p2.Y) <= 1) return 1;
            if (p1.X != p2.X && p1.Y != p2.Y) return 3;
            return 2;
        }

        void MoveTail(int number)
        {
            var p1 = Rope[number - 1];
            var p2 = Rope[number];
            var distanceType = Dist(number);
            if (distanceType < 2) return;
            if (distanceType < 3)
            {
                if (p1.X < p2.X)
                    --p2.X;
                else if (p1.X > p2.X)
                    ++p2.X;
                else if (p1.Y < p2.Y)
                    --p2.Y;
                else if (p1.Y > p2.Y)
                    ++p2.Y;
            }
            else
            {
                if (p1.X > p2.X && p1.Y > p2.Y)
                {
                    ++p2.X;
                    ++p2.Y;
                }
                else if (p1.X > p2.X && p1.Y < p2.Y)
                {
                    ++p2.X;
                    --p2.Y;
                }
                else if (p1.X < p2.X && p1.Y > p2.Y)
                {
                    --p2.X;
                    ++p2.Y;
                }
                else if (p1.X < p2.X && p1.Y < p2.Y)
                {
                    --p2.X;
                    --p2.Y;
                }
            }
            Rope[number] = p2;
            if (number == Rope.Length - 1)
                AddPosition(p2);
        }

        void InitPositions()
        {
            positions.Clear();
            positions.Add(0);
        }

        void AddPosition(Point coord)
        {
            positions.Add(((Int64)coord.X << 32) + coord.Y);
        }

        readonly HashSet<Int64> positions = new HashSet<long>();
        Point[] Rope;
    }
}