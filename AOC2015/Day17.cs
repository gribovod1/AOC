using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2015
{
    public static class Day17
    {
        public static void exec()
        {

            var boxesText = File.ReadAllLines(@"Data\day17.txt");
            var boxes = new List<int>();
            foreach (var s in boxesText)
            {
                boxes.Add(int.Parse(s));
            }
            boxes.Sort();
            var one = partOne(boxes);
            var two = partTwo(boxes);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public static int partOne(List<int> boxes)
        {
            var result = 0;
            for (var i = 1; i < 0x100000; ++i)
            {
                var size = 0;
                for(var b = 0; b < boxes.Count; ++b)
                {
                    size += boxes[b] * ((i >> b) & 1);
                    if (size > 150)
                        break;
                }
                if (size == 150)
                    result++;
            }
            return result;
        }

        public static int partTwo(List<int> boxes)
        {
            int[] count = new int[20];
            for (var i = 1; i < 0x100000; ++i)
            {
                var size = 0;
                for (var b = 0; b < boxes.Count; ++b)
                {
                    size += boxes[b] * ((i >> b) & 1);
                    if (size > 150)
                        break;
                }
                if (size == 150)
                    ++count[count1(i)];
            }
            foreach (var c in count)
                if (c > 0)
                    return c;
            return 0;
        }

        static int count1(int data)
        {
            int count = 0;
            while (data > 0)
            {
                ++count;
                data = data & (data - 1);
            }
            return count;
        }
    }
}