using AnyThings;

namespace AOC2022
{
    internal class Day01 : DayPattern<List<int>>
    {
        public override void ParseFile(string path)
        {
            var text = File.ReadAllText(path);
            var scanText = text.Split(Environment.NewLine);
            data = new List<int>();
            data.Add(0);
            for (var i = 0; i < scanText.Length; ++i)
                if (scanText[i].Length > 0)
                {
                    data[data.Count - 1] += int.Parse(scanText[i]);
                }
                else
                {
                    data.Add(0);
                }
        }

        public override string PartOne()
        {
            data.Sort();
            return data[data.Count - 1].ToString();
        }

        public override string PartTwo()
        {
            return (data[data.Count - 1] + data[data.Count - 2] + data[data.Count - 3]).ToString();
        }
    }
}