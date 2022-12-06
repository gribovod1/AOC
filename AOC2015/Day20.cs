using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2015
{
    public class Day20
    {
        public void Exec()
        {
            var startTime = DateTime.Now;
            var one = partOne();
            var two = partTwo();
            var endTime = DateTime.Now;
            Console.WriteLine($"partOne: {one} partTwo: {two} time: {(endTime - startTime).TotalSeconds}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        int partOne()
        {
            var FINDED = 36000000;
            var summArray = new int[2000000];
            var minHouse = int.MaxValue;
            for (var i = 1; i <= summArray.Length; ++i)
            {
                for (var j = i; j <= summArray.Length; j += i)
                {
                    summArray[j-1] += i * 10;
                    if (summArray[j-1] >= FINDED)
                    {
                        if (minHouse > j)
                        {
                            minHouse = j;
                            Console.WriteLine($"Домик {minHouse} получил счастье");
                        }
                    }
                }
                if (minHouse <= i)
                {
                    return minHouse;
                }
            }
            return minHouse;
        }

        int partTwo()
        {
            var FINDED = 36000000;
            var summArray = new int[2000000];
            var minHouse = int.MaxValue;
            for (var i = 1; i <= summArray.Length; ++i)
            {
                var houseCounter = 50;
                for (var j = i; j <= summArray.Length && houseCounter > 0; j += i)
                {
                    summArray[j - 1] += i * 11;
                    if (summArray[j - 1] >= FINDED)
                    {
                        if (minHouse > j)
                        {
                            minHouse = j;
                            Console.WriteLine($"Домик {minHouse} получил счастье");
                        }
                    }
                    --houseCounter;
                }
                if (minHouse <= i)
                {
                    return minHouse;
                }
            }
            return minHouse;
        }
    }
}