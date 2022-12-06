using AnyThings;
using System;

namespace AOC2021
{
    class DeepArea
    {
        public long Left;
        public long Top;
        public long Right;
        public long Bottom;

        public DeepArea(int left, int right, int top, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }

    class Day17 : DayPattern<DeepArea>
    {
        bool inArea(long x, long y)
        {
            return x >= data.Left && x <= data.Right && y >= data.Bottom && y <= data.Top;
        }

        void step(ref long dx, ref long dy, ref long x, ref long y)
        {
            x += dx;
            y += dy;
            --dy;
            if (dx > 0)
                --dx;
        }

        bool fleet(long x, long y)
        {
            return x > data.Right || y < data.Bottom;
        }

        bool checkSpeed(long dx, long dy, ref long maxY)
        {
            long x = 0;
            long y = 0;
            while (!fleet(x, y))
            {
                step(ref dx, ref dy, ref x, ref y);
                if (y > maxY)
                    maxY = y;
                if (inArea(x, y))
                    return true;
                if (dx == 0 && (x < data.Left || x > data.Right))
                    return false;
            }
            return false;
        }

        public override void Parse(string path)
        {
            data = new DeepArea(207, 263, -63, -115);
        }

        public override string PartOne()
        {
            long result = 0;
            for (long dx = 0; dx < data.Left; ++dx)
                for (long dy = 0; dy < -data.Bottom; ++dy)
                {
                    long maxY = 0;
                    if (checkSpeed(dx, dy, ref maxY))
                        result = Math.Max(result, maxY);
                }
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            long maxY = 0;
            for (long dx = 0; dx <= data.Right; ++dx)
                for (long dy = data.Bottom; dy < -data.Bottom; ++dy)
                    if (checkSpeed(dx, dy, ref maxY))
                        ++result;
            return result.ToString();
        }
    }
}