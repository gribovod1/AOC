using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public class Day18
    {
        public void Exec()
        {
            var numbersText = File.ReadAllLines(@"Data\day18.txt");



            var one = partOne(numbersText);
            var two = partTwo(numbersText);
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        ulong calcExpression(string source)
        {
            var endSubExpression = source.IndexOf(')');
            while (endSubExpression >= 0)
            {
                var startSubExpression = source.LastIndexOf('(', endSubExpression);
                source = source.Substring(0, startSubExpression) + calcExpression(source.Substring(startSubExpression + 1, endSubExpression - startSubExpression - 1)) + source.Substring(endSubExpression + 1, source.Length - endSubExpression - 1);
                endSubExpression = source.IndexOf(')');
            }
            var ss = source.Split(' ');
            ulong result = ulong.Parse(ss[0]);
            for (var i = 2; i < ss.Length; i += 2)
            {
                if (ss[i - 1] == "*")
                {
                    result *= ulong.Parse(ss[i]);
                }
                else
                    result += ulong.Parse(ss[i]);
            }
            return result;
        }

        ulong partOne(string[] source)
        {
            ulong result = 0;
            for (var i = 0; i < source.Length; ++i)
                result += calcExpression(source[i]);
            return result;
        }

        ulong partTwo(string[] source)
        {
            ulong result = 0;
            for (var i = 0; i < source.Length; ++i)
                result += calcExpression2(source[i]);
            return result;
        }

        ulong calcExpression2(string source)
        {
            var endSubExpression = source.IndexOf(')');
            while (endSubExpression >= 0)
            {
                var startSubExpression = source.LastIndexOf('(', endSubExpression);
                source = source.Substring(0, startSubExpression) + calcExpression2(source.Substring(startSubExpression + 1, endSubExpression - startSubExpression - 1)) + source.Substring(endSubExpression + 1, source.Length - endSubExpression - 1);
                endSubExpression = source.IndexOf(')');
            }

            var ss = new List<string>(source.Split(' '));
            var i = 2;
            while (i < ss.Count)
            {
                if (ss[i - 1] == "+")
                {
                    ss[i] = (ulong.Parse(ss[i - 2]) + ulong.Parse(ss[i])).ToString();
                    ss.RemoveAt(i - 1);
                    ss.RemoveAt(i - 2);
                }
                else i += 2;
            }
            ulong result = ulong.Parse(ss[0]);
            for (i = 2; i < ss.Count; i += 2)
                result *= ulong.Parse(ss[i]);
            return result;
        }
    }
}