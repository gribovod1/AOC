using AnyThings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AOC2021
{
    class ImageConverter
    {
        public HashSet<Point> image = new HashSet<Point>();
        public bool[] matrix;
        public bool unknownPixelsIsLight = false;
        Rectangle knownArea;
        public void setLight(HashSet<Point> lights, int x, int y, bool enable)
        {
            var p = new Point(x, y);
            if (enable)
                lights.Add(p);
            else
                lights.Remove(p);
        }

        internal void convert()
        {
            HashSet<Point> lights = new HashSet<Point>();
            knownArea = new Rectangle(knownArea.X - 1, knownArea.Y - 1, knownArea.Width + 2, knownArea.Height + 2);
            for (var y = knownArea.Top - 1; y <= knownArea.Bottom + 1; ++y)
                for (var x = knownArea.Left - 1; x <= knownArea.Right + 1; ++x)
                {
                    int index = 0;
                    for (var bit = 0; bit < 9; ++bit)
                        index = (index << 1) | getBitValue(x, y, bit);
                    setLight(lights, x, y, matrix[index]);
                }
            var extraSpaceIndex = unknownPixelsIsLight ? 0b111111111 : 0b000000000;
            unknownPixelsIsLight = matrix[extraSpaceIndex];
            image = lights;
        }

        private int getBitValue(int x, int y, int bit)
        {
            var p = new Point(x + (bit % 3 - 1), y + (bit / 3 - 1));
            if (p.X < knownArea.Left || p.X > knownArea.Right || p.Y < knownArea.Top || p.Y > knownArea.Bottom)
                return unknownPixelsIsLight ? 1 : 0;
            return image.Contains(p) ? 1 : 0;
        }

        public void calcCurrentSize()
        {
            var minx = int.MaxValue;
            var miny = int.MaxValue;
            var maxx = int.MinValue;
            var maxy = int.MinValue;
            foreach (var p in image)
            {
                if (p.X < minx)
                    minx = p.X;
                if (p.X > maxx)
                    maxx = p.X;
                if (p.Y < miny)
                    miny = p.Y;
                if (p.Y > maxy)
                    maxy = p.Y;
            }
            knownArea = new Rectangle(minx, miny, maxx - minx + 1, maxy - miny + 1);
        }

        public void show()
        {
            Console.Write(knownArea);
            var xOffset = 3 - knownArea.Left;
            var yOffset = 3 - knownArea.Top;
            for (var y = knownArea.Top; y < Math.Min(Console.BufferHeight - yOffset, knownArea.Bottom); ++y)
                for (var x = knownArea.Left; x < Math.Min(Console.BufferWidth - xOffset, knownArea.Right); ++x)
                {
                    Console.SetCursorPosition(x + xOffset, y + yOffset);
                    Console.Write(".");
                }
            foreach (var p in image)
                if (Console.BufferWidth > p.X + xOffset && Console.BufferHeight > p.Y + yOffset)
                {
                    Console.SetCursorPosition(p.X + xOffset, p.Y + yOffset);
                    Console.Write("#");
                }

            Console.SetCursorPosition(0, 1);
        }
    }

    class ImageConverter2
    {
        public HashSet<Point> image = new HashSet<Point>();
        public bool[] matrix;

        public void setLight(HashSet<Point> lights, int x, int y, bool enable)
        {
            var p = new Point(x, y);
            if (enable)
                lights.Add(p);
            else
                lights.Remove(p);
        }

        internal void convert()
        {
            HashSet<Point> lights = new HashSet<Point>();
            var rect = getCurrentSize();
            for (var y = rect.Top - 1; y < rect.Bottom + 1; ++y)
                for (var x = rect.Left - 1; x < rect.Right + 1; ++x)
                {
                    int index = 0;
                    for (var bit = 0; bit < 9; ++bit)
                        index = (index << 1) | getBitValue(x, y, bit);
                    setLight(lights, x, y, matrix[index]);
                }
            image = lights;
        }

        public void show()
        {
            var rect = getCurrentSize();
            Console.Write(rect);
            var xOffset = 3 - rect.Left;
            var yOffset = 3 - rect.Top;
            for (var y = rect.Top; y < rect.Bottom; ++y)
                for (var x = rect.Left; x < rect.Right; ++x)
                {
                    Console.SetCursorPosition(x + xOffset, y + yOffset);
                    Console.Write(".");
                }
            foreach (var p in image)
            {
                Console.SetCursorPosition(p.X + xOffset, p.Y + yOffset);
                Console.Write("#");
            }

            Console.SetCursorPosition(0, 1);
        }

        private int getBitValue(int x, int y, int bit)
        {
            var p = new Point(x + (bit % 3 - 1), y + (bit / 3 - 1));
            return image.Contains(p) ? 1 : 0;
        }

        private Rectangle getCurrentSize()
        {
            var minx = int.MaxValue;
            var miny = int.MaxValue;
            var maxx = int.MinValue;
            var maxy = int.MinValue;
            foreach (var p in image)
            {
                if (p.X < minx)
                    minx = p.X;
                if (p.X > maxx)
                    maxx = p.X;
                if (p.Y < miny)
                    miny = p.Y;
                if (p.Y > maxy)
                    maxy = p.Y;
            }
            return new Rectangle(minx, miny, maxx - minx + 1, maxy - miny + 1);
        }
    }

    class Day20 : DayPattern<ImageConverter>
    {
        public override void Parse(string path)
        {
            var ss = File.ReadAllText(path).Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            data = new ImageConverter();
            data.matrix = new bool[ss[0].Length];
            for (var i = 0; i < ss[0].Length; ++i)
                data.matrix[i] = ss[0][i] == '#';

            var imageText = ss[1].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (var y = 0; y < imageText.Length; ++y)
                for (var x = 0; x < imageText[y].Length; ++x)
                    data.setLight(data.image, x, y, imageText[y][x] == '#');
            data.calcCurrentSize();
        }

        public override string PartOne()
        {
            data.convert();
            data.convert();
            return data.image.Count.ToString();
        }

        public override string PartTwo()
        {
            for (var i = 0; i < 48; ++i)
                data.convert();

            return data.image.Count.ToString();
        }
    }
}