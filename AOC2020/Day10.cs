using AOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public static class Day10
    {
        public static void Exec()
        {
            var numbersText = File.ReadAllLines(@"Data\day10.txt");
            var numberList = new List<ulong>() { 0 };
            foreach (var s in numbersText)
                numberList.Add(ulong.Parse(s));
            numberList.Sort();
            numberList.Add(numberList[numberList.Count - 1] + 3);
            var numbers = numberList.ToArray();
            var one = partOne(numbers);
            var two = countExcludeVariations(numbers, 1);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public static int partOne(ulong[] numbers)
        {
            var count1 = 0;
            var count3 = 0;
            for (int i = 1; i < numbers.Length; ++i)
            {
                if (numbers[i] - numbers[i - 1] == 1)
                    ++count1;
                if (numbers[i] - numbers[i - 1] == 3)
                    ++count3;
            }
            return count1 * count3;
        }

        static ulong countExcludeVariations(ulong[] numbers, int index)
        {
            if (index == numbers.Length - 1)
                return 1;
            if (numbers[index + 1] - numbers[index - 1] > 3)
                return countExcludeVariations(numbers, index + 1);
            var endExclude = findEndIndex(numbers, index);
            ulong localVariationsCount = calcBrute(numbers, index, endExclude);
            return localVariationsCount * countExcludeVariations(numbers, endExclude);
        }

        static int findEndIndex(ulong[] numbers, int index)
        {
            if (numbers[index + 1] - numbers[index - 1] > 3)
                return index;
            return findEndIndex(numbers, index + 1);
        }

        static ulong calcBrute(ulong[] numbers, int index, int endIndex)
        {
            if (index >= endIndex)
                return 1;
            if (numbers[index + 1] - numbers[index - 1] <= 3)
                return calcBrute(excludeClone(numbers, index), index, endIndex - 1) + calcBrute(numbers, index + 1, endIndex);
            else
                return calcBrute(numbers, index + 1, endIndex);
        }

        static ulong[] excludeClone(ulong[] numbers, int excludeIndex)
        {
            var result = new ulong[numbers.Length - 1];
            for (var i = 0; i < result.Length; ++i)
                result[i] = i < excludeIndex ? numbers[i] : numbers[i + 1];
            return result;
        }
    }
}