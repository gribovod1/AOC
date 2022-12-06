using AOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day12
    {
        public void Exec()
        {
            var numbersText = File.ReadAllLines(@"Data\day12.txt");
            var one = partOne(numbersText);
            var two = partTwo(numbersText);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public long partOne(string[] commandsText)
        {
            long x = 0;
            long y = 0;
            long angle = 0;
            foreach(var  c in commandsText)
            {
                var d = c[0];
                var value = int.Parse(c.Substring(1, c.Length - 1));
                switch (d)
                {
                    case 'N':
                        {
                            y += value;
                            break;
                        }
                    case 'W':
                        {
                            x -= value;
                            break;
                        }
                    case 'S':
                        {
                            y -= value;
                            break;
                        }
                    case 'E':
                        {
                            x += value;
                            break;
                        }
                    case 'R':
                        {
                            angle -= value;
                            while (angle < 0)
                                angle += 360;
                            break;
                        }
                    case 'L':
                        {
                            angle += value;
                            while (angle >= 360)
                                angle -= 360;
                            break;
                        }
                    case 'F':
                        {
                            x += (int)(Math.Cos(angle * Math.PI / 180) * value);
                            y += (int)(Math.Sin(angle * Math.PI / 180) * value);
                            break;
                        }
                }
            }

            return Math.Abs(x)+Math.Abs(y);
        }

        public long partTwo(string[] commandsText)
        {
            long px = 10;
            long py = 1;
            long x = 0;
            long y = 0;
            foreach (var c in commandsText)
            {
                var d = c[0];
                var value = int.Parse(c.Substring(1, c.Length - 1));
                switch (d)
                {
                    case 'N':
                        {
                            py += value;
                            break;
                        }
                    case 'W':
                        {
                            px -= value;
                            break;
                        }
                    case 'S':
                        {
                            py -= value;
                            break;
                        }
                    case 'E':
                        {
                            px += value;
                            break;
                        }
                    case 'R':
                        {
                            var tx =(int)Math.Round(Math.Cos(value * Math.PI / 180) * px + Math.Sin(value * Math.PI / 180) * py);
                            var ty = (int)Math.Round(-Math.Sin(value * Math.PI / 180) * px + Math.Cos(value * Math.PI / 180) * py);
                            px = tx;
                            py = ty;
                            break;
                        }
                    case 'L':
                        {
                            var tx = (int)Math.Round(Math.Cos(value * Math.PI / 180) * px - Math.Sin(value * Math.PI / 180) * py);
                            var ty = (int)Math.Round(Math.Sin(value * Math.PI / 180) * px + Math.Cos(value * Math.PI / 180) * py);
                            px = tx;
                            py = ty;
                            break;
                        }
                    case 'F':
                        {
                            x += px * value;
                            y += py *value;
                            break;
                        }
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }
    }
}