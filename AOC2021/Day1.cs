using AOC;
using System;
using System.IO;

namespace AOC2021
{
    public class Day1
    {
        public void Exec()
        {
            var sNumbers = File.ReadAllLines(@"Data\day1.txt");
            var one = 0;
            var two = 0;
            var pNumber = int.Parse(sNumbers[0]);
            for (var i = 1; i < sNumbers.Length; ++i)
            {
                var nCurrent = int.Parse(sNumbers[i]);
                if (nCurrent > pNumber)
                    ++one;
                pNumber = nCurrent;
            }
           pNumber = summ(sNumbers, 0);
            for (var i = 1; i < sNumbers.Length - 2; ++i)
            {
                var nCurrent = summ(sNumbers, i);
                if (nCurrent > pNumber)
                    ++two;
                pNumber = nCurrent;
            }
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        int summ(string[] source, int index)
        {
            return int.Parse(source[index]) + int.Parse(source[index + 1]) + int.Parse(source[index + 2]);
        }
    }
}