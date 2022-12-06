using AOC;
using System;

namespace AOC2020
{
    public class Day25
    {
        public void Exec()
        {
            ulong key = 2959251;
            ulong door = 4542595;

            ulong value = 1;
            ulong loopKey = 0;
            while (value != key)
            {
                value = (value * 7) % 20201227;
                ++loopKey;
            }

            value = 1;
            for (ulong i =0;i< loopKey; ++i)
                value = (value * door) % 20201227;

            Console.WriteLine($"key: {value}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(value.ToString());
        }
    }
}