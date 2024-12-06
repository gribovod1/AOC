using AnyThings;

namespace AOC2024
{
    internal class Day03 : DayPattern<string>
    {
        public override void Parse(string singleText)
        {
            data = singleText;
        }

        bool isDigit(string dig)
        {
            for (int i = 0; i < dig.Length; ++i) if (!char.IsDigit(dig[i])) return false;
            return true;
        }

        public override string PartOne()
        {
            long result = 0;
            var muls = data.Split("mul(");
            foreach (var m in muls)
            {
                var sub = m.Split(")");
                if (sub.Length == 0) continue;
                if (sub[0].Length > 7 || sub[0].Length < 3) continue;
                var digs = sub[0].Split(",");
                if (digs.Length != 2 || digs[0].Length < 1 || digs[1].Length < 1 || digs[0].Length > 3 || digs[1].Length > 3) continue;
                if (isDigit(digs[0]) && isDigit(digs[1]))
                    result += int.Parse(digs[0]) * int.Parse(digs[1]);
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            int index = 0;
            while ((index = data.IndexOf("don't()")) >= 0)
            {
                int endIndex = data.IndexOf("do()", index);
                data = data.Remove(index, endIndex - index + 4);
            }
            return PartOne();
        }
    }
}