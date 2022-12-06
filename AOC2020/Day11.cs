using AOC;
using System;
using System.IO;

namespace AOC2020
{
    public class Day11
    {
        public void Exec()
        {
            var numbersText = File.ReadAllLines(@"Data\day11.txt");
            var one = partOne(numbersText);
            var two = partTwo(numbersText);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(one.ToString());
        }

        public int partOne(string[] numbersText)
        {
            bool mod = false;
            do
            {
                numbersText = Place(numbersText, out mod);
            }
            while (mod);
            var result = 0;
            for (var y = 0; y < numbersText.Length; ++y)
                for (var x = 0; x < numbersText[y].Length; ++x)
                    result += numbersText[y][x] == '#' ? 1 : 0;
            return result;
        }

        public string[] Place(string[] numbersText, out bool mod)
        {
            var places = new string[numbersText.Length];
            mod = false;
            for (var y = 0; y < numbersText.Length; ++y)
            {
                for (var x = 0; x < numbersText[y].Length; ++x)
                {
                    if (numbersText[y][x] == 'L')
                    {
                        var cF = countFree(numbersText, y, x);
                        if (cF == 8)
                        {
                            places[y] += '#';
                            mod = true;
                            continue;
                        }
                    }
                    else if (numbersText[y][x] == '#')
                    {
                        var cF = countFree(numbersText, y, x);
                        if (cF <= 4)
                        {
                            places[y] += 'L';
                            mod = true;
                            continue;
                        }
                    }
                    places[y] += numbersText[y][x];
                }
            }
            return places;
        }

        int countFree(string[] numbersText, int y, int x)
        {
            var result = 0;
            result += isFree(numbersText, y - 1, x - 1, 0, 0) ? 1 : 0;
            result += isFree(numbersText, y, x - 1, 0, 0) ? 1 : 0;
            result += isFree(numbersText, y + 1, x - 1, 0, 0) ? 1 : 0;
            result += isFree(numbersText, y + 1, x, 0, 0) ? 1 : 0;
            result += isFree(numbersText, y + 1, x + 1, 0, 0) ? 1 : 0;
            result += isFree(numbersText, y, x + 1, 0, 0) ? 1 : 0;
            result += isFree(numbersText, y - 1, x + 1, 0, 0) ? 1 : 0;
            result += isFree(numbersText, y - 1, x, 0, 0) ? 1 : 0;
            return result;
        }

        bool isFree(string[] numbersText, int y, int x, int dy, int dx)
        {
            if (y < 0 ||
                x < 0 ||
                y >= numbersText.Length ||
                x >= numbersText[y].Length ||
                numbersText[y][x] == 'L')
                return true;
            if (numbersText[y][x] == '#') return false;
            if (dy == 0 && dx == 0)
                return true;
            return isFree(numbersText, y + dy, x + dx, dy, dx);
        }

        public int partTwo(string[] numbersText)
        {
            bool mod = false;
            do
            {
                numbersText = Place2(numbersText, out mod);
            }
            while (mod);
            var result = 0;
            for (var y = 0; y < numbersText.Length; ++y)
                for (var x = 0; x < numbersText[y].Length; ++x)
                    result += numbersText[y][x] == '#' ? 1 : 0;
            return result;
        }

        public string[] Place2(string[] numbersText, out bool mod)
        {
            var places = new string[numbersText.Length];
            mod = false;
            for (var y = 0; y < numbersText.Length; ++y)
            {
                for (var x = 0; x < numbersText[y].Length; ++x)
                {
                    if (numbersText[y][x] == 'L')
                    {
                        var cF = countFreeLine(numbersText, y, x);
                        if (cF == 8)
                        {
                            places[y] += '#';
                            mod = true;
                            continue;
                        }
                    }
                    else if (numbersText[y][x] == '#')
                    {
                        var p = 8 - countFreeLine(numbersText, y, x);
                        if (p >= 5)
                        {
                            places[y] += 'L';
                            mod = true;
                            continue;
                        }
                    }
                    places[y] += numbersText[y][x];
                }
            }
            return places;
        }

        int countFreeLine(string[] numbersText, int y, int x)
        {
            var result = 0;
            result += isFree(numbersText, y - 1, x - 1, -1, -1) ? 1 : 0;
            result += isFree(numbersText, y, x - 1, 0, -1) ? 1 : 0;
            result += isFree(numbersText, y + 1, x - 1, 1, -1) ? 1 : 0;
            result += isFree(numbersText, y + 1, x, 1, 0) ? 1 : 0;
            result += isFree(numbersText, y + 1, x + 1, 1, 1) ? 1 : 0;
            result += isFree(numbersText, y, x + 1, 0, 1) ? 1 : 0;
            result += isFree(numbersText, y - 1, x + 1, -1, 1) ? 1 : 0;
            result += isFree(numbersText, y - 1, x, -1, 0) ? 1 : 0;
            return result;
        }
    }
}