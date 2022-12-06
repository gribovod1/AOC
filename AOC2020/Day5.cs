using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public static class Day5
    {
        public static void exec()
        {
            var lines = File.ReadAllLines(@"Data\day5.txt");
            var countOne = 0;
            var ids = new HashSet<int>();
            for (var i = 0; i < 1024; ++i)
                ids.Add(i);
            foreach (var s in lines)
            {
                int id = Convert.ToInt32(s.Replace('B', '1').Replace('L', '1').Replace('F', '0').Replace('R', '0'), 2);
                if (countOne < id)
                    countOne = id;
                ids.Remove(id);
            }
            Console.WriteLine($"countOne: {countOne}");
            var prev = 0;
            foreach (var id in ids)
            {
                if (id > prev + 1)
                    Console.WriteLine($"id: {id}");
                prev = id;
            }
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                Clipboard.SetText(countOne.ToString());
            }
        }
    }
}