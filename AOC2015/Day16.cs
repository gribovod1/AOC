using AOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AOC2015
{
    public static class Day16
    {
        public static void exec()
        {
            var needValues = new SortedList<string, int>
            {
                { "children", 3 },
                { "cats", 7 },
                { "samoyeds", 2 },
                { "pomeranians", 3 },
                { "akitas", 0 },
                { "vizslas", 0 },
                { "goldfish", 5 },
                { "trees", 3 },
                { "cars", 2 },
                { "perfumes", 1 }
            };

            var aunts = File.ReadAllLines(@"Data\day16.txt");
            var one = partOne(needValues, aunts);
            var two = partTwo(needValues, aunts);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public static int partOne(SortedList<string, int> properties, string[] aunts)
        {
            for (var i = 0; i < aunts.Length; ++i)
            {
                var ps = aunts[i]
                    .Replace(':', ' ')
                    .Replace(',', ' ')
                    .Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var p = 2;
                for (; p < ps.Length; p += 2)
                {
                    var value = int.Parse(ps[p + 1]);
                    if (properties[ps[p]] != value)
                        break;
                }
                if (p >= ps.Length)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public static int partTwo(SortedList<string, int> properties, string[] aunts)
        {
            var result = 0;
            for (var i = 0; i < aunts.Length; ++i)
            {
                var ps = aunts[i]
                    .Replace(':', ' ')
                    .Replace(',', ' ')
                    .Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var valid = true;
                for (var p = 2; p < ps.Length; p += 2)
                    valid &= check(properties, ps[p], int.Parse(ps[p + 1]));
                if (valid)
                {
                    Console.WriteLine($"{aunts[i]}");
                    result = i + 1;
                }
            }
            return result;
        }

        static bool check(SortedList<string, int> properties, string property, int value)
        {
            if (property == "cats" || property == "trees")
                return value > properties[property];
            else if (property == "pomeranians" || property == "goldfish")
                return value < properties[property];
            else
                return properties[property] == value;
        }
    }
}