using AnyThings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DayType = AOC2021.Paper;

namespace AOC2021
{
    public class Paper
    {
       public List<Point> points = new List<Point>();
        public List<KeyValuePair<char, int>> instructions = new List<KeyValuePair<char, int>>();

        public void addPoint(int x, int y)
        {
            points.Add(new Point(x, y));
        }
        public void addInstruction(char type, int coord)
        {
            instructions.Add(new KeyValuePair<char, int>(type, coord));
        }

        bool ExistsPoint(Point p)
        {
            foreach (var p1 in points)
                if (p1.X == p.X && p1.Y == p.Y)
                    return true;
            return false;
        }
        internal void fold(int v)
        {
            if (instructions[v].Key == 'x')
            {
                for (var pIndex = 0; pIndex < points.Count; ++pIndex)
                {
                    if (points[pIndex].X > instructions[v].Value)
                    {
                        var p = new Point(2 * instructions[v].Value - points[pIndex].X, points[pIndex].Y);
                        points.RemoveAt(pIndex);
                        if (!ExistsPoint(p))
                            points.Add(p);
                        --pIndex;
                    }
                }
            }
            else
            {
                for (var pIndex = 0; pIndex < points.Count; ++pIndex)
                {
                    if (points[pIndex].Y > instructions[v].Value)
                    {
                        var p = new Point(points[pIndex].X, 2 * instructions[v].Value - points[pIndex].Y);
                        points.RemoveAt(pIndex);
                        if (!ExistsPoint(p))
                            points.Add(p);
                        --pIndex;
                    }
                }
            }
        }

        public void fold()
        {
            for (var i = 1; i < instructions.Count; ++i)
                fold(i);
        }

        internal object CountPoints()
        {
            return points.Count;
        }
    }

    class Day13 : DayPattern<DayType>
    {
        public override void Parse(string path)
        {
            data = new Paper();
            var text = File.ReadAllText(path);
            var ss = text.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in ss[0].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                var c = s.Split(',');
                data.addPoint(int.Parse(c[0]), int.Parse(c[1]));
            }
            foreach (var s in ss[1].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                var c = s.Split('=');
                if (c[0] == "fold along x")
                    data.addInstruction('x', int.Parse(c[1]));
                else
                    data.addInstruction('y', int.Parse(c[1]));
            }
        }

        public override string PartOne()
        {
            Console.WriteLine($"Before: {data.CountPoints().ToString()}");
            data.fold(0);
            return data.CountPoints().ToString();
        }

        public override string PartTwo()
        {
            var result = 0;
            data.fold();

            int maxy = 0;
            foreach(var p in data.points)
            {
                if (p.Y > maxy)
                    maxy = p.Y;
                Console.SetCursorPosition(p.X, p.Y + 3);
                Console.Write("#");
            }
            Console.SetCursorPosition(0, maxy + 10);
            return result.ToString();
        }
    }
}
