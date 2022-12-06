using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 10.15    11.01*/
/* 13.02    */

namespace AOC2018
{
    public static class Day22
    {
        public static void Exec()
        {
            var depth = 510;
            var targetX = 10;
            var targetY = 10;
            var one = partOne(depth,targetX,targetY);
            var two = 0;

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public static int partOne(int depth, int tX, int tY)
        {
            var risk = 0;
            var map = new int[tX + 1, tY + 1];
            for (var y = 0; y <= tY; ++y)
                for (var x = 0; x <= tX; ++x)
                    risk += getType(map, depth, tX, tY, x, y);
            return risk;
        }

        public static int getType(int[,] map, int depth, int tX, int tY, int x, int y)
        {
            int geoIndex = 0;
            if ((x == 0 && y == 0) ||
                (x == tX && y == tY))
                geoIndex = 0;
            else if (x == 0)
                geoIndex = y * 48271;
            else if (y == 0)
                geoIndex = x * 16807;
            else
                geoIndex = map[x - 1, y] * map[x, y - 1];
            map[x, y] = (geoIndex + depth) % 20183;
            return map[x, y] % 3;
        }
        
        public static ulong partTwo(int depth, int tX, int tY)
        {
            var Xsize = tX * 2 + 1;
            var Ysize = tY * 2 + 1;
            var map = new int[Xsize, Ysize];
            for (var y = 0; y < Ysize; ++y)
                for (var x = 0; x < Xsize; ++x)
                    getType(map, depth, tX, tY, x, y);
             
            for (var y = 0; y < Ysize; ++y)
                for (var x = 0; x < Xsize; ++x)
                {
                    map[x, y] = map[x, y] % 3;
                }

            return 0;
        }
    }
}
