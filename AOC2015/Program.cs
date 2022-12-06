﻿using CurrentDay = AOC2015.Day22;

namespace AOC2015
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDay = new CurrentDay();
            currentDay.Exec($"Data\\{currentDay.GetType().Name}.txt");
        }
    }
}
