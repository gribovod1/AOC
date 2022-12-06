using AOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AOC2020
{
    public static class Day7
    {
        public static void exec()
        {
            var rules = File.ReadAllLines(@"Data\day7.txt");
            var one = partOne(rules);
            var two = partTwo(rules);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public static int partOne(string[] rules)
        {
            var s = new HashSet<string>();
            var containBags = new HashSet<string>();
            containBags.Add("shiny gold");
            var added = 0;
            do
            {
                added = 0;
                foreach (var r in rules)
                {
                    var bags = Regex.Replace(r, "[0-9]", "", RegexOptions.IgnoreCase).Split(new string[] { ",", " contain ", ".", " bags", " bag" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var b in bags)
                    {
                        var t = b.Trim();
                        if (containBags.Contains(t))
                        {
                            if (s.Add(bags[0]))
                            {
                                containBags.Add(bags[0]);
                                added++;
                            }
                        }
                    }
                }
            } while (added > 0);
            s.Remove("shiny gold");
            return s.Count;
        }

        class Bag
        {
            List<KeyValuePair<string, int>> Bags;

            public Bag(string containBags)
            {
                Bags = new List<KeyValuePair<string, int>>();
                var bags = containBags.Split(new string[] { ",", " contain ", ".", " bags", " bag" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var b in bags)
                {
                    var numBag = b.Trim();
                    var index = numBag.IndexOf(" ");
                    if (int.TryParse(numBag.Substring(0, index), out int count))
                    {
                        var bag = numBag.Substring(index + 1, numBag.Length - index - 1);
                        Bags.Add(new KeyValuePair<string, int>(bag, count));
                    }
                }
            }

            public int Summary(Dictionary<string, Bag> bags)
            {
                int result = 0;
                foreach (var b in Bags)
                {
                    result += (bags[b.Key].Summary(bags) + 1) * b.Value;
                }
                return result;
            }
        }

        public static int partTwo(string[] rules)
        {
            var bags = new Dictionary<string, Bag>();
            Bag sgold = null;
            foreach (var r in rules)
            {
                var ss = " bags contain";
                var startIndex = r.IndexOf(ss);
                if (startIndex > 0)
                {
                    var b = new Bag(r.Substring(startIndex + ss.Length, r.Length - startIndex - ss.Length));
                    bags.Add(r.Substring(0, startIndex), b);
                    if (r.IndexOf("shiny gold") == 0)
                        sgold = b;
                }
            }
            return sgold.Summary(bags);
        }
    }
}