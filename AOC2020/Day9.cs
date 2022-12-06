using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public static class Day9
    {
        public static void Exec()
        {
            var numbersText = File.ReadAllLines(@"Data\day9.txt");
            var numbers = new List<ulong>();
            foreach (var s in numbersText)
                numbers.Add(ulong.Parse(s));
            var one = partOne(numbers.ToArray());
            var two = partTwo(numbers.ToArray(), one);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public static int partOne(ulong[] numbers)
        {
            for (int i = 25; i < numbers.Length; ++i)
            {
                if (!check(numbers, i))
                    return i;
            }
            return 0;
        }

        static bool check(ulong[] numbers, int index)
        {
            for (var j = index - 25; j < index; ++j)
            {
                for (var j1 = j + 1; j1 < index; ++j1)
                {
                    if (numbers[j] + numbers[j1] == numbers[index])
                        return true;
                }
            }
            return false;
        }

        public static ulong partTwo(ulong[] numbers, int index)
        {
            for (var i = 0; i < index; i++)
            {
                ulong summ = numbers[i];
                ulong min = numbers[i];
                ulong max = numbers[i];
                for (var j = i + 1; j < index; j++)
                {
                    summ += numbers[j];
                    if (min > numbers[j])
                        min = numbers[j];
                    if (max < numbers[j])
                        max = numbers[j];
                    if (summ > numbers[index])
                        break;
                    if (summ == numbers[index])
                    {
                        Console.WriteLine($"min: {min} max: {max}");
                        return min + max;
                    }
                }
            }
            return 0;
        }
    }
}