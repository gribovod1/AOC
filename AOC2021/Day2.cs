using AOC;
using System;
using System.Collections.Generic;
using System.IO;

using Parsed = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, int>>;

namespace AOC2021
{
    public class Day2
    {
        private Parsed Parse(string[] strings)
        {
            Parsed result = new Parsed();
            foreach (var s in strings)
            {
                var ss = s.Split(new char[] { ' ', (char)9 }, StringSplitOptions.RemoveEmptyEntries);
                result.Add(new KeyValuePair<string, int>(ss[0], int.Parse(ss[1])));
            }
            return result;
        }

        private int partOne(Parsed data)
        {
            int x = 0;
            int y = 0;
            foreach(var c in data)
            {
                switch (c.Key)
                {
                    case "up":
                        {
                            y -= c.Value;
                            break;
                        }
                    case "forward":
                        {
                            x += c.Value;
                            break;
                        }
                    case "down":
                        {
                            y += c.Value;
                            break;
                        }
                }
            }
            return x*y;
        }

        private int partTwo(Parsed data)
        {
            int x = 0;
            int y = 0;
            int target = 0;
            foreach (var c in data)
            {
                switch (c.Key)
                {
                    case "up":
                        {
                            target -= c.Value;
                            break;
                        }
                    case "forward":
                        {
                            x += c.Value;
                            y += target * c.Value;
                            break;
                        }
                    case "down":
                        {
                            target += c.Value;
                            break;
                        }
                }
            }
            return x * y;
        }

        public void Exec()
        {
            var data = Parse(File.ReadAllLines(@"Data\day2.txt"));
            var one = partOne(data);
            var two = partTwo(data);
            Console.WriteLine($"{this.GetType().Name} partOne: {one} partTwo: {two}");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    {
                        Clipboard.SetText(one.ToString());
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        Clipboard.SetText(two.ToString());
                        break;
                    }
            }
        }
    }
}