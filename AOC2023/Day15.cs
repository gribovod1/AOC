using AnyThings;

namespace AOC2023
{
    internal class Day15 : DayPattern<List<(Int64 hash, string input)>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(',', StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (int row = 0; row < text.Length; row++)
                data.Add((GetHash(text[row]), text[row]));
        }

        int GetHash(string value)
        {
            int hash = 0;
            for (int i = 0; i < value.Length; i++)
                hash = ((hash + value[i]) * 17) % 256;
            return hash;
        }

        public override string PartOne()
        {
            Int64 result = 0;
            for (int i = 0; i < data.Count; ++i)
                result += data[i].hash;
            return result.ToString();
        }

        public override string PartTwo()
        {
            Int64 result = 0;
            (List<string> Label, List<int> Focus)[] Tube = new (List<string> Label, List<int> Focus)[256];
            var split = new char[] { '=', '-' };
            for (int l = 0; l < data.Count; ++l)
            {
                var commandd = data[l].input.Split(split, StringSplitOptions.RemoveEmptyEntries);
                var label = commandd[0];
                var box = GetHash(label);
                if (Tube[box].Label == null)
                {
                    Tube[box].Label = new List<string>();
                    Tube[box].Focus = new List<int>();
                }
                var index = Tube[box].Label.IndexOf(label);
                if (data[l].input.Contains('='))
                {
                    if (index >= 0)
                        Tube[box].Focus[index] = int.Parse(commandd[1]);
                    else
                    {
                        Tube[box].Label.Add(label);
                        Tube[box].Focus.Add(int.Parse(commandd[1]));
                    }
                }
                else
                    if (index >= 0)
                    {
                        Tube[box].Label.RemoveAt(index);
                        Tube[box].Focus.RemoveAt(index);
                    }
            }

            for (int b = 0; b < Tube.Length; ++b)
                if (Tube[b].Label != null)
                    for (int i = 0; i < Tube[b].Label.Count; ++i)
                        result += (b + 1) * (i + 1) * Tube[b].Focus[i];
            return result.ToString();
        }
    }
}