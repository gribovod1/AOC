using AnyThings;

namespace AOC2023
{
    internal class Day13 : DayPattern<List<(int[] vertical, int[] horizontal, int sizeV, int sizeH)>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            foreach (var s in text)
            {
                var row = s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                int[] vertical = new int[row[0].Length];
                int sizeV = row.Length;
                for (int i = 0; i < vertical.Length; i++)
                {
                    int value = 0;
                    for (int j = 0; j < row.Length; j++)
                        value |= (int)((row[j][i] == '#' ? 1 : 0) << (row.Length - 1 - j));
                    vertical[i] = value;
                }
                int[] horizontal = new int[row.Length];
                int sizeH = row[0].Length;
                for (int i = 0; i < horizontal.Length; i++)
                {
                    int value = 0;
                    for (int j = 0; j < row[0].Length; j++)
                        value |= (int)((row[i][j] == '#' ? 1 : 0) << (row[0].Length - 1 - j));
                    horizontal[i] = value;
                }
                data.Add((vertical, horizontal, sizeV, sizeH));
            }
        }

        public override string PartOne()
        {
            Int64 result = 0;
            for (int s = 0; s < data.Count; ++s)
                result += 100 * GetValue(data[s].horizontal) + GetValue(data[s].vertical);
            return result.ToString();
        }

        int GetValue(int[] lines)
        {
            int index;
            for (index = 0; index < lines.Length - 1; ++index)
                if (CheckCenter(lines, index))
                    if (!UseRepair)
                        break;
            if (index < lines.Length - 1)
                return index + 1;
            return 0;
        }

        bool UseRepair = false;
        bool CheckCenter(int[] lines, int index)
        {
            int m = 1;
            bool needRepairStatus = UseRepair;
            for (int i = index; i >= 0 && i + m < lines.Length; --i)
            {
                if (lines[i] != lines[i + m])
                {
                    if (UseRepair)
                    {
                        if (IsOneDifferent(lines[i], lines[i + m]))
                            UseRepair = false;
                        else
                        {
                            if (needRepairStatus)
                                UseRepair = true;
                            return false;
                        }
                    }
                    else
                    {
                        if (needRepairStatus)
                            UseRepair = true;
                        return false;
                    }
                }
                m += 2;
            }
            return true;
        }

        bool IsOneDifferent(int x, int y)
        {
            return OneCount(x ^ y) == 1;
        }

        int OneCount(int number)
        {
            int result = 0;
            for (int i = 0; i < 32; ++i)
                if (((number >> i) & 1) != 0)
                    ++result;
            return result;
        }

        public override string PartTwo()
        {
            Int64 result = 0;
            for (int s = 0; s < data.Count; ++s)
            {
                UseRepair = true;
                int h = 100 * GetValue(data[s].horizontal);
                if (UseRepair)
                    result += GetValue(data[s].vertical);
                else
                    result += h;
            }
            return result.ToString();
        }
    }
}