using AOC;
using System;
using System.IO;

namespace AnyThings
{
    public abstract class DayPattern<T>
    {
        public string Puzzle;
        public T data;
        public abstract string PartOne();
        public abstract string PartTwo();
        public virtual void ParseFile(string path)
        {
            Puzzle = File.ReadAllText(path);
            Parse(Puzzle);
        }
        public virtual void Parse(string text)
        {

        }

        public virtual void InitData()
        {

        }

        public void Exec(string path)
        {
            Console.Title = $"Run {this.GetType().Name}";
            ParseFile(path);
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
