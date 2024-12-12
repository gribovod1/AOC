using AnyThings;
using Coord = (int r, int c);

namespace AOC2024
{
    internal class Day12 : DayPattern<(char[][] map, HashSet<(int l, int c)> read)>
    {
        public override void Parse(string singleText)
        {
            string[] lines = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data.map = new char[lines.Length][];
            for (int l = 0; l < lines.Length; l++)
            {
                data.map[l] = lines[l].ToCharArray();
            }
            data.read = new();
        }

        public override string PartOne()
        {
            long result = 0;
            for (int l = 0; l < data.map.Length; ++l)
                for (int c = 0; c < data.map[l].Length; ++c)
                    if (!data.read.Contains((l, c)))
                    {
                        long s = 0;
                        long p = 0;
                        GetArea(ref s, ref p, data.map[l][c], l, c);
                        result += s * p;
                    }
            return result.ToString();
        }

        private bool GetArea(ref long Square, ref long Perimeter, char type, int l, int c)
        {
            if (l < 0 || c < 0 || l >= data.map.Length || c >= data.map[0].Length) return false;
            if (data.map[l][c] != type) return false;
            if (data.read.Contains((l, c))) return true;
            data.read.Add((l, c));
            Perimeter += 4;
            ++Square;
            if (GetArea(ref Square, ref Perimeter, type, l, c + 1)) --Perimeter;
            if (GetArea(ref Square, ref Perimeter, type, l, c - 1)) --Perimeter;
            if (GetArea(ref Square, ref Perimeter, type, l + 1, c)) --Perimeter;
            if (GetArea(ref Square, ref Perimeter, type, l - 1, c)) --Perimeter;
            return true;
        }

        public override string PartTwo()
        {
            long result = 0;
            data.read.Clear();
            for (int l = 0; l < data.map.Length; ++l)
                for (int c = 0; c < data.map[l].Length; ++c)
                    if (!data.read.Contains((l, c)))
                    {
                        long s = 0;
                        HashSet<(int l, int c, int s)> p = new();
                        GetArea2(ref s, p, data.map[l][c], l, c);
                        result += s * p.Count;
                    }
            return result.ToString();
        }

        private bool GetArea2(ref long Square, HashSet<(int l, int c, int s)> Perimeter, char type, int l, int c)
        {
            if (l < 0 || c < 0 || l >= data.map.Length || c >= data.map[0].Length) return false;
            if (data.map[l][c] != type) return false;
            if (data.read.Contains((l, c))) return true;
            data.read.Add((l, c));
            ++Square;
            var p = FindStartPerimeter(l, c, 0); if (p.s >= 0) Perimeter.Add(p);
            p = FindStartPerimeter(l, c, 1); if (p.s >= 0) Perimeter.Add(p);
            p = FindStartPerimeter(l, c, 2); if (p.s >= 0) Perimeter.Add(p);
            p = FindStartPerimeter(l, c, 3); if (p.s >= 0) Perimeter.Add(p);

            GetArea2(ref Square, Perimeter, type, l, c + 1);
            GetArea2(ref Square, Perimeter, type, l, c - 1);
            GetArea2(ref Square, Perimeter, type, l + 1, c);
            GetArea2(ref Square, Perimeter, type, l - 1, c);
            return true;
        }

        /*
         
            начало
            v
            *-
            |A

            3
           2A0         
            1

         */

        (int l, int c, int s) FindStartPerimeter(int l, int c, int side)
        {
            char type = data.map[l][c];
            switch (side)
            {
                case 0:
                    {
                        if (!isThisArea(l, c + 1, type))
                        {
                            int startL = l;
                            while (isThisArea(startL - 1, c, type) && !isThisArea(startL - 1, c + 1, type))
                                --startL;
                            return (startL, c + 1, side);
                        }
                        return (l, c, -1);
                    }
                case 1:
                    {
                        if (!isThisArea(l + 1, c, type))
                        {
                            int startC = c;
                            while (isThisArea(l, startC - 1, type) && !isThisArea(l + 1, startC - 1, type))
                                --startC;
                            return (l + 1, startC, side);
                        }
                        return (l, c, -1);
                    }
                case 2:
                    {
                        if (!isThisArea(l, c - 1, type))
                        {
                            int startL = l;
                            while (isThisArea(startL - 1, c, type) && !isThisArea(startL - 1, c - 1, type))
                                --startL;
                            return (startL, c, side);
                        }
                        return (l, c, -1);
                    }
                case 3:
                    {
                        if (!isThisArea(l - 1, c, type))
                        {
                            int startC = c;
                            while (isThisArea(l, startC - 1, type) && !isThisArea(l - 1, startC - 1, type))
                                --startC;
                            return (l, startC, side);
                        }
                        return (l, c, -1);
                    }
            }
            return (l, c, -1);
        }

        bool isThisArea(int l, int c, char type)
        {
            if (l < 0 || c < 0 || l >= data.map.Length || c >= data.map[0].Length) return false;
            return data.map[l][c] == type;
        }
    }
}