using AnyThings;

namespace AOC2023
{
    internal class Day09 : DayPattern<List<List<Int64>>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (int i = 0; i < text.Length; ++i)
            {
                data.Add(new());
                var seq = text[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in seq)
                    data[i].Add(Int64.Parse(s));
            }
        }

        public override string PartOne()
        {
            /*
            Рекурсивно передаётся последовательность разниц между числами. Результат - сумма текущей последней разницы чисел и результата со следующего уровня
             */
            Int64 result = 0;
            foreach (var s in data)
                result += GetExtrapolation(s, false);
            return result.ToString();
        }

        public Int64 GetExtrapolation(List<Int64> numbers, bool first)
        {
            bool IsNull = true;
            foreach (var n in numbers)
                if (n != 0)
                {
                    IsNull = false;
                    break;
                }
            if (IsNull)
                return 0;
            List<Int64> diff = new();
            for (int i = 1; i < numbers.Count; ++i)
                diff.Add(numbers[i] - numbers[i - 1]);
            return first ? numbers[0] - GetExtrapolation(diff, first) : numbers[numbers.Count - 1] + GetExtrapolation(diff, first);
        }

        public override string PartTwo()
        {
            /*
             Рекурсивно передаётся последовательность разниц между числами. Результат - разница текущей первой разницы чисел и результата со следующего уровня
            */
            Int64 result = 0;
            foreach (var s in data)
                result += GetExtrapolation(s, true);
            return result.ToString();
        }
    }
}