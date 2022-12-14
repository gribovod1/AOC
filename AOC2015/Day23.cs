using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2015
{
    internal class Day23 : DayPattern<List<string>>
    {
        public override void ParseFile(string path)
        {
            data = File.ReadAllText(path).Split(new string[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        int compute(int a, int b)
        {
            int cp = 0;
            while (cp >= 0 && cp < data.Count)
            {
                var ss = data[cp].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                switch (ss[0])
                {
                    case "hlf":
                        {
                            if (ss[1] == "a")
                                a /= 2;
                            else if (ss[1] == "b")
                                b /= 2;
                            ++cp;
                            break;
                        }
                    case "tpl":
                        {
                            if (ss[1] == "a")
                                a *= 3;
                            else if (ss[1] == "b")
                                b *= 3;
                            ++cp;
                            break;
                        }
                    case "inc":
                        {
                            if (ss[1] == "a")
                                a += 1;
                            else if (ss[1] == "b")
                                b += 1;
                            ++cp;
                            break;
                        }
                    case "jmp":
                        {
                            cp += int.Parse(ss[1]);
                            break;
                        }
                    case "jie":
                        {
                            if ((ss[1] == "a" && ((a & 1) == 0)) || (ss[1] == "b" && ((b & 1) == 0)))
                                cp += int.Parse(ss[2]);
                            else ++cp;
                            break;
                        }
                    case "jio":
                        {
                            if (((ss[1] == "a") && (a == 1)) || ((ss[1] == "b") && (b == 1)))
                                cp += int.Parse(ss[2]);
                            else ++cp;
                            break;
                        }
                }
            }
            return b;
        }

        public override string PartOne()
        {
            return compute(0,0).ToString();
        }

        public override string PartTwo()
        {
            return compute(1, 0).ToString();
        }
    }
}