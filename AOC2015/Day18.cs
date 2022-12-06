using AOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2015
{
    public class Day18
    {
        public void Exec()
        {
            var numbersText = File.ReadAllLines(@"Data\day18.txt");
                       
            var one = partOne(numbersText);
            var two = partTwo(numbersText);
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        int partOne(string[] source)
        {
            var field = parse(source);
            var new_field = parse(source);
            for (var i = 0; i < 100; ++i)
            {
                iteration_one(field, new_field);
                var t = field;
                field = new_field;
                new_field = t;
            }
            return calcLights(field);
        }

        private int[,] parse(string[] source)
        {
            int iCount = source.Length;
            int jCount = source[0].Length;
            int[,] result = new int[iCount, jCount];
            for (var i = 0; i < iCount; ++i)
                for (var j = 0; j < jCount; ++j)
                    result[i, j] = source[i][j] == '#' ? 1 : 0;
            return result;
        }

        private void iteration_one(int[,] field, int[,] new_field, bool anchor_fix = false)
        {
            for (var i = 0; i < field.GetLength(0); ++i)
                for (var j = 0; j < field.GetLength(1); ++j)
                {
                    var count = environment(field, i, j);
                    if (anchor_fix && ((i == 0 && j == 0) ||
                        (i == field.GetLength(0) - 1 && j == 0) ||
                        (i == field.GetLength(0) - 1 && j == field.GetLength(1) - 1) ||
                        (i == 0 && j == field.GetLength(1) - 1)))
                    {
                        new_field[i, j] = 1;
                    }
                    else
                    {
                        if (field[i, j] != 0)
                        {
                            if (count != 2 && count != 3)
                            {
                                new_field[i, j] = 0;
                            }
                            else
                            {
                                new_field[i, j] = 1;
                            }
                        }
                        else
                        {
                            if (count == 3)
                            {
                                new_field[i, j] = 1;
                            }
                            else
                            {
                                new_field[i, j] = 0;
                            }
                        }
                    }
                }
        }

        private int environment(int[,] field, int i, int j)
        {
            int result = 0;
            result += getValue(field, i - 1, j - 1);
            result += getValue(field, i - 1, j - 0);
            result += getValue(field, i - 1, j + 1);
            result += getValue(field, i - 0, j - 1);
            result += getValue(field, i + 0, j + 1);
            result += getValue(field, i + 1, j - 1);
            result += getValue(field, i + 1, j - 0);
            result += getValue(field, i + 1, j + 1);
            return result;
        }

        private int getValue(int[,] field, int i, int j)
        {
            if (i >= field.GetLength(0) ||
                i < 0 ||
                j >= field.GetLength(1) ||
                j < 0)
                return 0;
            return field[i, j];
        }

        private int calcLights(int[,] field)
        {
            int result = 0;
            for (var i = 0; i < field.GetLength(0); ++i)
                for (var j = 0; j < field.GetLength(1); ++j)
                    result += field[i, j];
            return result;
        }

        int partTwo(string[] source)
        {
            var field = parse(source);
            var new_field = parse(source);
            field[0, 0] = 1;
            field[field.GetLength(0) - 1, 0] = 1;
            field[field.GetLength(0) - 1, field.GetLength(1) - 1] = 1;
            field[0, field.GetLength(1) - 1] = 1;
            for (var i = 0; i < 100; ++i)
            {
                iteration_one(field, new_field, true);
                var t = field;
                field = new_field;
                new_field = t;
            }
            return calcLights(field);
        }

    }
}
