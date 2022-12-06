using AOC;
using System;

namespace AnyThings
{
    public abstract class DayPattern<T>
    {
        public T data;
        public abstract string PartOne();
        public abstract string PartTwo();
        public abstract void Parse(string path);

        public void Exec(string path)
        {
            Console.Title = $"Run {this.GetType().Name}";
            Parse(path);
            Console.WriteLine($"{this.GetType().Name}: ");
            var one = PartOne();
            var two = PartTwo();
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.D1)
                Clipboard.SetText(one);
            if (key == ConsoleKey.D2)
                Clipboard.SetText(two);
        }
    }
}
