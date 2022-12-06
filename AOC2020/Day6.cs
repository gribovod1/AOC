using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public static class Day6
    {
        public static void exec()
        {
            var groups = File.ReadAllText(@"Data\day6.txt").Split(new string[] {Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var one = partOne(groups);
            var two = partTwo(groups);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public static int partOne(string[] groups)
        {
            int result = 0;
            foreach (var g in groups)
            {
                var s = new HashSet<char>();
                foreach (var c in g)
                {
                    if (c >= 'a' && c <= 'z')
                    {
                        s.Add(c);
                    }
                }
                result += s.Count;
            }
            return result;
        }

        public static int partTwo(string[] groups)
        {
            int result = 0;
            foreach (var g in groups)
            {
                HashSet<char> s = null;
                var first = true;
                var people = g.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var answers in people)
                {
                    var current = new HashSet<char>();
                    foreach (var c in answers)
                    {
                        if (c >= 'a' && c <= 'z')
                        {
                            current.Add(c);
                        }
                    }
                    if (first)
                    {
                        s = current;
                        first = false;
                    }
                    else
                        s.IntersectWith(current);
                }
                result += s.Count;
            }
            return result;
        }
    }
}
