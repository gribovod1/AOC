using AOC;
using System;
using System.Collections.Generic;

namespace AOC2020
{
    public class Day15
    {
        public void Exec()
        {
            var numbers = new int[] { 0, 3, 6 };

            var one = partOne(numbers);
            var two = partTwo(numbers);
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public int partOne(int[] numbers)
        {
            return play(numbers, 2020);
        }

        public int partTwo(int[] numbers)
        {
            return 0;
            return play(numbers, 30000000);
        }

        int play(int[] numbers, int stepCount)
        {
            var step = 0;
            var d = new Dictionary<int, int>();
            for (step = 0; step < numbers.Length; ++step)
                d.Add(numbers[step], step + 1);
            var lastNumber = numbers[numbers.Length - 1];
            for (step = numbers.Length + 1; step <= stepCount; ++step)
            {
                addStep(d, lastNumber, step);
                lastNumber = getAge(d, lastNumber, step);
            }
            return lastNumber;
        }

        int getAge(Dictionary<int, int> storage, int number, int step)
        {
            if (storage.ContainsKey(number))
                return step - 1 - storage[number];
            return 0;
        }

        void addStep(Dictionary<int, int> storage, int number, int step)
        {
            if (!storage.ContainsKey(number))
                storage.Add(number, step);
            else
                storage[number] = step;
        }
    }
}