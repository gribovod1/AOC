using AnyThings;

namespace AOC2022
{
    internal class Day6 : DayPattern<string>
    {
        public override void Parse(string path)
        {
            data = File.ReadAllText(path);
        }

        public static bool Add(List<char> Data, char data, int count)
        {
            Data.Add(data);
            if (Data.Count < count) return false;
            if (Data.Count > count)
                Data.RemoveAt(0);
            HashSet<char> set = new();
            foreach (var c in Data)
            {
                if (set.Contains(c))
                    return false;
                set.Add(c);
            }
            return true;
        }

        public override string PartOne()
        {
            int counter = 0;
            List<char> chars = new();
            foreach (var c in data)
            {
                ++counter;
                if (Add(chars, c, 4))
                    break;
            }
            return counter.ToString();
        }

        static bool magic(string source)
        {
            uint val = 0;
            for (var i = 0; i < source.Length; ++i)
            {
                var s = 1u << (source[i] - 'a');
                if ((val & s) != 0)
                    return false;
                val |= s;
            }
            return true;
        }

        public override string PartTwo()
        {
            int counter = 0;

            for (var i = 0; i < data.Length - 14; ++i)
            {
                if (magic(data.Substring(i, 14)))
                    return (counter+14).ToString();
                ++counter;
            }
            return "NOT";
        }
    }
}