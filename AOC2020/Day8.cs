using AOC;
using System;

namespace AOC2020
{
    public static class Day8
    {
        public static void Exec()
        {
            var console = new GameConsole();
            console.LoadProgramFromFile(@"Data\day8.txt");
            console.Run(() => console.CurrentProgram[console.CommandAddress].count > 0);
            var one = console.Accumulator;
            var two = PartTwo(console);

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public static int PartTwo(GameConsole console)
        {
            for (var i = 0; i < console.CurrentProgram.Count; ++i)
            {
                if (console.CurrentProgram[i].command == "jmp")
                {
                    console.CurrentProgram[i].command = "nop";
                    if (console.Run(() => console.CurrentProgram[console.CommandAddress].count > 0))
                        return console.Accumulator;
                    console.CurrentProgram[i].command = "jmp";
                }
                else if (console.CurrentProgram[i].command == "nop")
                {
                    console.CurrentProgram[i].command = "jmp";
                    if (console.Run(() => console.CurrentProgram[console.CommandAddress].count > 0))
                        return console.Accumulator;
                    console.CurrentProgram[i].command = "nop";
                }
            }
            return 0;
        }
    }
}