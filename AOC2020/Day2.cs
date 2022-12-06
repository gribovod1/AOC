using AOC;
using System;
using System.IO;
using System.Linq;

namespace AOC2020
{
    class Day2
    {
        static void exec()
        {
            var passwords = File.ReadAllLines("data.txt");
            var countOne = 0;
            var countTwo = 0;
            for (var i = 0; i < passwords.Length; i++)
            {
                var split_space = passwords[i].Split(' ');
                var branch = split_space[0].Split('-');
                var firstNumber = int.Parse(branch[0]);
                var secondNumber = int.Parse(branch[1]);
                var symbol = split_space[1][0];
                var pass = split_space[2];

                var count_symbol = pass.Sum((char c) => c == symbol ? 1 : 0);
                if (count_symbol >= firstNumber && count_symbol <= secondNumber)
                    ++countOne;

                if ((pass[firstNumber - 1] == symbol) ^ (pass[secondNumber - 1] == symbol))
                    ++countTwo;
            }
            Console.WriteLine($"Part One: {countOne} Part Two: {countTwo}");
            Clipboard.SetText(countTwo.ToString());
            Console.ReadKey();
        }
    }
}
