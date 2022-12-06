using AOC;
using System;
using System.Globalization;
using System.IO;

namespace AOC2020
{
    class Day4
    {
        static void exec()
        {
            var lines = File.ReadAllText("data.txt");
            var ps = lines.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var countOne = 0;
            var countTwo = 0;
            for (var i = 0; i < ps.Length; ++i)
            {
                if (ps[i].IndexOf("byr:") >= 0 &&
                    ps[i].IndexOf("iyr:") >= 0 &&
                    ps[i].IndexOf("eyr:") >= 0 &&
                    ps[i].IndexOf("hgt:") >= 0 &&
                    ps[i].IndexOf("hcl:") >= 0 &&
                    ps[i].IndexOf("ecl:") >= 0 &&
                    ps[i].IndexOf("pid:") >= 0)
                {
                    ++countOne;
                    var ss = ps[i].Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    if (check(ss))
                        ++countTwo;
                }
            }
            Clipboard.SetText(countTwo.ToString());
            Console.WriteLine($"countOne: {countOne} countTwo: {countTwo}");
            Console.ReadKey();
        }

        static bool check(string[] ss)
        {
            for (var f = 0; f < ss.Length; ++f)
            {
                var fn = ss[f].Split(':');
                switch (fn[0])
                {
                    case "byr":
                        {
                            int n = 0;
                            if (!int.TryParse(fn[1], out n) || n < 1920 || n > 2002)
                                return false;
                            break;
                        }
                    case "iyr":
                        {
                            int n = 0;
                            if (!int.TryParse(fn[1], out n) || n < 2010 || n > 2020)
                                return false;
                            break;
                        }
                    case "eyr":
                        {
                            int n = 0;
                            if (!int.TryParse(fn[1], out n) || n < 2020 || n > 2030)
                                return false;
                            break;
                        }
                    case "hgt":
                        {
                            int iCM = fn[1].IndexOf("cm");
                            int iIn = fn[1].IndexOf("in");
                            if (iCM > 0)
                            {
                                int n = 0;
                                if (!int.TryParse(fn[1].Substring(0, iCM), out n) || n < 150 || n > 193)
                                    return false;
                            }
                            else if (iIn > 0)
                            {
                                int n = 0;
                                if (!int.TryParse(fn[1].Substring(0, iIn), out n) || n < 59 || n > 76)
                                    return false;
                            }
                            else
                                return false;
                            break;
                        }
                    case "hcl":
                        {
                            if (fn[1][0] != '#' || fn[1].Length != 7)
                                return false;
                            int n = 0;
                            if (!int.TryParse(fn[1].Substring(1, 6), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out n))
                                return false;
                            break;
                        }
                    case "ecl":
                        {
                            if (fn[1] != "amb" &&
                                fn[1] != "blu" &&
                                fn[1] != "brn" &&
                                fn[1] != "gry" &&
                                fn[1] != "grn" &&
                                fn[1] != "hzl" &&
                                fn[1] != "oth")
                                return false;
                            break;
                        }
                    case "pid":
                        {
                            int n = 0;
                            if (!int.TryParse(fn[1], out n) || fn[1].Length != 9)
                                return false;
                            break;
                        }
                }
            }
            return true;
        }
    }
}
