using AOC;
using System;
using System.IO;

namespace AOC2015
{
    public class Day1
    {
        public void Exec()
        {
            var steps = File.ReadAllLines(@"Data\day1.txt");
            var two = 0;
            var floor = 0;
            for(var i=0;i<steps[0].Length;++i)
            {
                floor += steps[0][i] == '(' ? 1 : -1;
                if (floor == -1 && two == 0)
                    two = i + 1;
            }
            Console.WriteLine($"partOne: {floor} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }
    }
}