using AnyThings;

namespace AOC2023
{
    internal class Day01 : DayPattern<List<string>>
    {
        public override void Parse(string singleText)
        {
            data = singleText.Split(Environment.NewLine).ToList();
        }

        public override string PartOne()
        {
            /*
             Перебором проверяются все символы и выбираются цифры. Запоминается первая и последняя.
             */
            int result = 0;
            for (int i = 0; i < data.Count; ++i)
            {
                string s = data[i];
                int first = 0;
                int last = 0;
                for (int j = 0; j < s.Length; ++j)
                {
                    if (char.IsDigit(s[j]))
                    {
                        last = s[j] - '0';
                        if (first == 0)
                            first = last;

                    }
                }
                result += first * 10 + last;
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            /*
             Простым поиском в строке ищутся последовательно все девять слов с цифрами, первое и последнее устанавливаются как стартовые значения, после чего повторяется поиск из первой части.
             */
            int result = 0;
            for (int i = 0; i < data.Count; ++i)
            {
                string s = data[i];
                (int firstText, int lastText, int firstTextIndex, int lastTextIndex) = GetNumber(s);
                int first = firstText;
                int last = lastText;
                for (int j = 0; j < s.Length; ++j)
                {
                    if (char.IsDigit(s[j]))
                    {
                        if (j < firstTextIndex)
                        {
                            first = s[j] - '0';
                            firstTextIndex = j;
                        }
                        if (j > lastTextIndex)
                        {
                            last = s[j] - '0';
                            lastTextIndex = j;
                        }
                    }
                }
                result += first * 10 + last;
            }
            return result.ToString();
        }

        (int, int, int, int) GetNumber(string input)
        {
            int last = -1;
            int first = -1;
            int lastIndex = -1;
            int firstIndex = int.MaxValue;
            string[] numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            for (int n = 0; n < numbers.Length; n++)
            {
                int index = input.IndexOf(numbers[n]);
                if (index >= 0 && index < firstIndex)
                {
                    first = n + 1;
                    firstIndex = index;
                }
                index = input.LastIndexOf(numbers[n]);
                if (index >= 0 && index > lastIndex)
                {
                    last = n + 1;
                    lastIndex = index;
                }
            }
            return (first, last, firstIndex, lastIndex);
        }
    }
}