using AnyThings;

namespace AOC2022
{
    internal class Day03 : DayPattern<List<string>>
    {
        public override void ParseFile(string path)
        {
            data = File.ReadAllText(path).Split(Environment.NewLine).ToList();
        }

        public int getPoints(char c)
        {
            if (c < 'a')
                return c - 'A' + 27;
            return c - 'a' + 1;
        }

        public override string PartOne()
        {
            int result = 0;
            foreach (var r in data)
            {
                result += getPoints(r.Substring(0, r.Length / 2).Intersect(r.Substring(r.Length / 2, r.Length / 2)).First());
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            int result = 0;
            for (var i = 0; i < data.Count; i += 3)
            {
                result += getPoints(data[i].Intersect(data[i + 1]).Intersect(data[i + 2]).First());
            }
            return result.ToString();
        }
    }
}