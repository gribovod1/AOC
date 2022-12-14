using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2021

{
    class Command
    {
        public char o1;

        public Command(char o1)
        {
            this.o1 = o1;
        }

        public static Command Parse(string c)
        {
            var cs = c.Split(' ');
            switch (cs[0])
            {
                case "inp":
                    {
                        return new Command(cs[1][0]);
                    }
                case "add":
                    {
                        if (char.IsLetter(cs[2][0]))
                            return new CommandAddVar(cs[1][0], cs[2][0]);
                        else
                            return new CommandAddDig(cs[1][0], int.Parse(cs[2]));
                    }
                case "div":
                    {
                        if (char.IsLetter(cs[2][0]))
                            return new CommandDivVar(cs[1][0], cs[2][0]);
                        else
                            return new CommandDivDig(cs[1][0], int.Parse(cs[2]));
                    }
                case "mod":
                    {
                        if (char.IsLetter(cs[2][0]))
                            return new CommandModVar(cs[1][0], cs[2][0]);
                        else
                            return new CommandModDig(cs[1][0], int.Parse(cs[2]));
                    }
                case "mul":
                    {
                        if (char.IsLetter(cs[2][0]))
                            return new CommandMulVar(cs[1][0], cs[2][0]);
                        else
                            return new CommandMulDig(cs[1][0], int.Parse(cs[2]));
                    }
                case "eql":
                    {
                        if (char.IsLetter(cs[2][0]))
                            return new CommandEqlVar(cs[1][0], cs[2][0]);
                        else
                            return new CommandEqlDig(cs[1][0], int.Parse(cs[2]));
                    }
            }
            return null;
        }

        public virtual void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] = input();
        }
    }

    class CommandAddVar : Command
    {
        public char o2;

        public CommandAddVar(char o1, char v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] += Variables[o2];
        }
    }

    class CommandAddDig : Command
    {
        public int o2;

        public CommandAddDig(char o1, int v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] += o2;
        }
    }

    class CommandMulVar : Command
    {
        public char o2;

        public CommandMulVar(char o1, char v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] *= Variables[o2];
        }
    }

    class CommandMulDig : Command
    {
        public int o2;

        public CommandMulDig(char o1, int v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] *= o2;
        }
    }

    class CommandDivVar : Command
    {
        public char o2;

        public CommandDivVar(char o1, char v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] /= Variables[o2];
        }
    }

    class CommandDivDig : Command
    {
        public int o2;

        public CommandDivDig(char o1, int v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] /= o2;
        }
    }

    class CommandEqlVar : Command
    {
        public char o2;

        public CommandEqlVar(char o1, char v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] = Variables[o1] == Variables[o2] ? 1 : 0;
        }
    }

    class CommandEqlDig : Command
    {
        public int o2;

        public CommandEqlDig(char o1, int v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] = Variables[o1] == o2 ? 1 : 0;
        }
    }

    class CommandModVar : Command
    {
        public char o2;

        public CommandModVar(char o1, char v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] %= Variables[o2];
        }
    }

    class CommandModDig : Command
    {
        public int o2;

        public CommandModDig(char o1, int v) : base(o1)
        {
            o2 = v;
        }

        public override void run(Dictionary<char, long> Variables, Func<long> input)
        {
            Variables[o1] %= o2;
        }
    }

    class State
    {
        public Dictionary<char, long> Variables = new Dictionary<char, long>();

        List<Command> Program = new List<Command>();
        public State(string[] vs)
        {
            foreach (var c in vs)
            {
                //    if (c.IndexOf("x") < 0)
                Program.Add(Command.Parse(c));
            }
            Variables.Add('w', 0);
            Variables.Add('x', 0);
            Variables.Add('y', 0);
            Variables.Add('z', 0);
        }

        public int[] input = new int[14];
        int posInput = 13;
        public void run()
        {
            foreach (var c in Program)
                c.run(Variables, inputProgram);
        }

        void clear()
        {
            foreach (var v in Variables)
                Variables[v.Key] = 0;
        }
        public long inputProgram()
        {
            var i = input[posInput];
            if (posInput > 0)
                --posInput;
            else
                posInput = 13;
            return i;
        }

        public void outputProgram(long value)
        {
            Console.Write(value);
        }
    }

    class Day24 : DayPattern<State>
    {
        public override void ParseFile(string path)
        {
            data = new State(File.ReadAllLines(path));// AnyThings.Parse.ParseToCharMap(path);
        }
        public override string PartOne()
        {
            /*      ulong value = 99999999999999;
                   for (; value > 0; --value)
                  {
                      var temp = value;
                      data.input.Clear();
                      while ((temp > 0) && (temp % 10 != 0))
                      {
                          data.input.Push((int)(temp % 10));
                          temp /= 10;
                      }
                      if (temp != 0)
                          continue;
                      data.run();
                      if (data.Variables['x'] != 1)
                          break;
                      if (data.Variables['z'] == 0)
                          break;
                  }*/
            // data.invert_run();
            return getMaxCorrect().ToString();
        }

        int getMaxCorrect(int p = 13)
        {
            if (p == 0)
                for (var i = 9; i > 0; --i)
                {
                    data.input[p] = i;
                    data.run();
                    if (data.Variables['z'] == 0)
                    {
                        Console.Write(i.ToString());
                        return i;
                    }
                }
            else
            {
                for (var i = 9; i > 0; --i)
                {
                    data.input[p] = i;
                    if (getMaxCorrect(p - 1) != 0)
                    {
                        Console.Write(i.ToString());
                        return i;
                    }
                }
            }
            return 0;
        }

        public override string PartTwo()
        {
            long result = 0;
            return result.ToString();
        }
    }
}