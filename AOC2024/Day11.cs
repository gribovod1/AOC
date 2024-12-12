using AnyThings;

namespace AOC2024
{
    internal class Day11 : DayPattern<Dictionary<long, long>>
    {
        public override void Parse(string singleText)
        {
            string[] nText = singleText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            data = new();
            foreach (var t in nText)
                AddNumber(data, long.Parse(t), 1);
        }

        public override string PartOne()
        {
            long result = 0;
            var numbers = data;
            Dictionary<long, Action<long, long, Dictionary<long, long>>> Functions = new();
            for (int i = 0; i < 25; ++i)
                numbers = ProcessStones(numbers, Functions);
            foreach (var n in numbers)
                result += n.Value;
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            var numbers = data;
            Dictionary<long, Action<long, long, Dictionary<long, long>>> Functions = new();
            for (int i = 0; i < 75; ++i)
                numbers = ProcessStones(numbers, Functions);
            foreach (var n in numbers)
                result += n.Value;
            return result.ToString();
        }

        Dictionary<long, long> ProcessStones(Dictionary<long, long> NumbersCount, Dictionary<long, Action<long, long, Dictionary<long, long>>> Functions)
        {
            Dictionary<long, long> result = new();
            foreach (var s in NumbersCount)
            {
                if (!Functions.ContainsKey(s.Key))
                {
                    if (s.Key == 0)
                        Functions.Add(s.Key, Rule1);
                    else
                    {
                        var t = s.Key.ToString();
                        if ((t.Length % 2) == 0)
                            Functions.Add(s.Key, Rule2);
                        else
                            Functions.Add(s.Key, Rule3);
                    }
                }
                Functions[s.Key](s.Key, NumbersCount[s.Key], result);
            }
            return result;
        }

        void Rule1(long number, long count, Dictionary<long, long> NumbersCount)
        {
            AddNumber(NumbersCount, 1, count);
        }

        void Rule2(long number, long count, Dictionary<long, long> NumbersCount)
        {
            var t = number.ToString();
            AddNumber(NumbersCount, long.Parse(t.Substring(0, t.Length / 2)), count);
            AddNumber(NumbersCount, long.Parse(t.Substring(t.Length / 2)), count);
        }

        void Rule3(long number, long count, Dictionary<long, long> NumbersCount)
        {
            AddNumber(NumbersCount, number * 2024 , count);
        }

        void AddNumber(Dictionary<long, long> NumbersCount, long number, long count)
        {
            if (NumbersCount.ContainsKey(number))
                NumbersCount[number] += count;
            else
                NumbersCount.Add(number, count);
        }
    }
}