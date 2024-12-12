using AnyThings;
using Coord = (int r, int c);

namespace AOC2024
{
    internal class Day13 : DayPattern<(char[][] map, HashSet<(int l, int c)> read)>
    {
        public override void Parse(string singleText)
        {
            string[] lines = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data.map = new char[lines.Length][];
            for (int l = 0; l < lines.Length; l++)
            {
                data.map[l] = lines[l].ToCharArray();
            }
            data.read = new();
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