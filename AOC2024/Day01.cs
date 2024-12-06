using AnyThings;

namespace AOC2024
{
    internal class Day01 : DayPattern<List<List<int>>>
    {
        public override void Parse(string singleText)
        {
            data = new();
            data.Add(new List<int>());
            data.Add(new List<int>());
            List<string> text = singleText.Split(Environment.NewLine).ToList();
            foreach(var p in text)
            {
                var items = p.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                data[0].Add(int.Parse(items[0]));
                data[1].Add(int.Parse(items[1]));
            }
        }

        public override string PartOne()
        {
            data[0].Sort();
            data[1].Sort();
            long result = 0;
            for (int i = 0; i < data[0].Count; ++i)
            {
                result += Math.Abs(data[0][i] - data[1][i]);
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            int i0 = 0;
            int i1 = 0;
            while (i0 < data[0].Count)
            {
                int value = data[0][i0];
                int count0 = 0;
                int count1 = 0;

                while (i1 < data[1].Count && value > data[1][i1])
                {
                    ++i1;
                }
                while (i1 < data[1].Count && value == data[1][i1])
                {
                    ++i1;
                    ++count1;
                }
                while (i0 < data[0].Count && value == data[0][i0])
                {
                    ++i0;
                    ++count0;
                }
                result += value * count0 * count1;
            }
            return result.ToString();
        }
    }
}