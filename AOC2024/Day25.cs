using AnyThings;
using System.Text;
using System.Xml.Linq;

namespace AOC2024
{
    internal class Day25 : DayPattern<(List<int> locks, List<int> keys)>
    {
        public override void Parse(string singleText)
        {
            var LocksAndKeys = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            data.locks = new();
            data.keys = new();
            foreach (var s in LocksAndKeys)
            {
                var lines = s.Split(Environment.NewLine);
                int value = 0;
                for (var r = 1; r < lines.Length - 1; r++)
                    for (var c = 0; c < lines[r].Length; c++)
                        if (lines[r][c] == '#')
                            value += (1 << c * 4);
                if (lines[0] == "#####")
                    data.locks.Add(value);
                else
                    data.keys.Add(value);
            }
        }

        /* Выделим каждому столбцу по 4 бита в числе и при сложении в каждой тетраде не должно быть числа больше 5 */
        public override string PartOne()
        {
            long result = 0;
            for (int l = 0; l < data.locks.Count; ++l)
                for (int k = 0; k < data.keys.Count; ++k)
                    if (check(data.locks[l] + data.keys[k]))
                        ++result;
            return result.ToString();
        }

        bool check(int value)
        {
            for (int i = 0; i < 5; ++i)
                if (((value >> i * 4) & 0b1111) > 5) return false;
            return true;
        }

        public override string PartTwo()
        {
            long result = 0;
            return result.ToString();
        }
    }
}