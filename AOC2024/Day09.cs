using AnyThings;

namespace AOC2024
{
    internal class Day09 : DayPattern<string[]>
    {
        public override void Parse(string singleText)
        {
            data = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string PartOne()
        {
            long result = 0;
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            return result.ToString();
        }
    }
}