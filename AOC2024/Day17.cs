using AnyThings;
using System.Text;

namespace AOC2024
{
    internal class Day17 : DayPattern<(long A, long B, long C, List<int> Program, string ProgramText)>
    {
        public override void Parse(string singleText)
        {
            data = new();
            data.Program = new();

            var RegistersAndProgram = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var rText = RegistersAndProgram[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data.A = int.Parse(rText[0].Split(' ')[2]);
            data.B = int.Parse(rText[1].Split(' ')[2]);
            data.C = int.Parse(rText[2].Split(' ')[2]);
            data.ProgramText = RegistersAndProgram[1].Remove(0, 9);
            var program = data.ProgramText.Split(new string[] { "Program: ", "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int r = 0; r < program.Length; r++)
                data.Program.Add(int.Parse(program[r]));
        }

        public override string PartOne()
        {
            return Execute(data.A, data.B, data.C, data.Program);
        }



        string Execute(long A, long B, long C, List<int> Program)
        {
            StringBuilder result = new();
            int CommandAddress = 0;
            while (CommandAddress < Program.Count)
            {
                var cmd = Program[CommandAddress];
                var operand = Program[CommandAddress + 1];
                switch (cmd)
                {
                    case 0:
                        {
                            int value = operand;
                            if (value == 4) value = (int)A;
                            else if (value == 5) value = (int)B;
                            else if (value == 6) value = (int)C;
                            A = A >> value;
                            break;
                        }
                    case 1:
                        {
                            B = B ^ operand;
                            break;
                        }
                    case 2:
                        {
                            long value = operand;
                            if (value == 4) value = A;
                            else if (value == 5) value = B;
                            else if (value == 6) value = C;
                            B = value % 8;
                            break;
                        }
                    case 3:
                        {
                            if (A != 0)
                            {
                                CommandAddress = operand;
                                continue;
                            }
                            break;
                        }
                    case 4:
                        {
                            B = B ^ C;
                            break;
                        }
                    case 5:
                        {
                            long value = operand;
                            if (value == 4) value = A;
                            else if (value == 5) value = B;
                            else if (value == 6) value = C;
                            if (result.Length > 0) result.Append(',');
                            result.Append(value % 8);
                            break;
                        }
                    case 6:
                        {
                            int value = operand;
                            if (value == 4) value = (int)A;
                            else if (value == 5) value = (int)B;
                            else if (value == 6) value = (int)C;
                            B = A >> value;
                            break;
                        }
                    case 7:
                        {
                            int value = operand;
                            if (value == 4) value = (int)A;
                            else if (value == 5) value = (int)B;
                            else if (value == 6) value = (int)C;
                            C = A >> value;
                            break;
                        }
                }
                CommandAddress += 2;
            }
            return result.ToString();
        }
        public override string PartTwo()
        {
            long result = 0;
            result = BitsFind();
            return result.ToString();
        }

        long BitsFind()
        {
            for (long i = 0; i < 1024; ++i)
            {
                var r = BitsFind(i, 1);
                if (r > 0)
                    return r;
            }
            return 0;
        }

        long BitsFind(long result, int lastCount)
        {
            if (lastCount > data.Program.Count) return 0;
            var currentResult = Execute(result, 0, 0, data.Program);
            if (!CheckOutput(currentResult, lastCount)) return 0;
            else if (lastCount == data.Program.Count) return result;
            for (long i = 0; i < 1024; ++i)
            {
                var r = BitsFind((result << 3) + i, lastCount + 1);
                if (r > 0)
                    return r;
            }
            return 0;
        }

        bool CheckOutput(string x, int lastCount)
        {
            if (x.Length > data.ProgramText.Length || x.Length != 2 * lastCount - 1) return false;
            for (int i = 0; i < x.Length; ++i)
                if (x[i] != data.ProgramText[data.ProgramText.Length - x.Length + i]) return false;
            return true;
        }
    }
}
/*

 Подбираем 3 бита, такие, чтобы получилось последнее, не подобранное число, если получилось, то проверяем, со всеми ранее полученными числами
 */