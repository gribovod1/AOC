using AOC;
using System;
using System.IO;

namespace AOC2020
{
    class Day3
    {
        static void exec()
        {
            var lines = File.ReadAllLines("data.txt");
            ulong count = calcPattern(1, 1, lines) * calcPattern(3, 1, lines) * calcPattern(5, 1, lines) * calcPattern(7, 1, lines) * calcPattern(1, 2, lines);
            Clipboard.SetText(count.ToString());
            Console.WriteLine($"count: {count}");
            Console.ReadKey();
        }

        static ulong calcPattern(int right, int down, string[] forest)
        {
            ulong count = 0;
            for (var i = 0; i < forest.Length; i += down)
            {
                if (forest[i][(i / down * right) % forest[i].Length] != '.')
                    ++count;
            }
            return count;
        }
    }
}
