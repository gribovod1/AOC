using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;

using Parsed = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, int>>;

namespace AOC2021
{
    public class Pattern_Commands_Input : DayPattern<Parsed>
    {
        public override void Parse(string path)
        {
            var strings = File.ReadAllLines(path);
            data = new Parsed();
            foreach (var s in strings)
            {
                var ss = s.Split(new char[] { ' ', (char)9 }, StringSplitOptions.RemoveEmptyEntries);
                data.Add(new KeyValuePair<string, int>(ss[0], int.Parse(ss[1])));
            }
        }

       public override string PartOne()
        {
            foreach (var c in data)
            {
                switch (c.Key)
                {
                    case "CMD":
                        {
                            break;
                        }
                }
            }
            return 0.ToString();
        }

        public override string PartTwo()
        {
            foreach (var c in data)
            {
                switch (c.Key)
                {
                    case "CMD":
                        {
                            break;
                        }
                }
            }
            return 0.ToString();
        }
    }
}