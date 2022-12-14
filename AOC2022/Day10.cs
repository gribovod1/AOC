using AnyThings;
using System.Drawing;

namespace AOC2022
{
    internal class Day10 : DayPattern<List<string>>
    {
        public override void ParseFile(string path)
        {
            data = File.ReadAllText(path).Split(Environment.NewLine).ToList();
        }

        public override string PartOne()
        {
            int result = 0;
            X = 1;
            for(var t = 0; t < Tacts[Tacts.Length - 1]+1; ++t)
            {
                if (Tacts.Contains(t))
                    result += t * X;
                Run(t);
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            X = 1;
            CurrCommand = 0;
            runningLenght = 0;
            for (var t = 0; t < 40 * 6; ++t)
            {
                Run(t);
                var y = t / 40;
                var x = t % 40;
                if (needDraw(x, y))
                {
                    (int left, int top) = Console.GetCursorPosition();
                    Console.SetCursorPosition(x + 5, y + 10);
                    Console.Write("#");
                    Console.SetCursorPosition(left, top);
                }
            }
            return 0.ToString();
        }

        private void Run(int t)
        {
            if (runningLenght > 0)
            {
                --runningLenght;
                if (runningLenght == 0)
                {
                    var s = data[CurrCommand].Split(' ');
                    if (s[0] == "addx")
                    {
                        X += int.Parse(s[1]);
                    }
                    ++CurrCommand;
                }
                else return;
            }
            if (CurrCommand >= data.Count)
                return;
            var ss = data[CurrCommand].Split(' ');
            if (ss[0] == "noop")
            {
                runningLenght = 1;
            }
            else if (ss[0] == "addx")
            {
                runningLenght = 2;
            }
        }

        bool needDraw(int x, int y)
        {
            return Math.Abs(X - x) < 2;
        }

        int[] Tacts = new int[] { 20, 60, 100, 140, 180, 220 };
        int X;
        int CurrCommand = 0;
        int runningLenght = 0;
    }
}