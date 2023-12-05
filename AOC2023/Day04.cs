using AnyThings;

namespace AOC2023
{
    internal class Day04 : DayPattern<List<(List<int> Win, HashSet<int> Numbers)>>
    {
        public override void Parse(string singleText)
        {
            var cardText = singleText.Split(Environment.NewLine);
            data = new();
            for (var i = 0; i < cardText.Length; i++)
            {
                var card = cardText[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                List<int> win = new();
                HashSet<int> numbers = new();
                int index = 2;
                while (index < card.Length && card[index] != "|")
                {
                    win.Add(int.Parse(card[index]));
                    ++index;
                }
                ++index;
                while (index < card.Length)
                {
                    numbers.Add(int.Parse(card[index]));
                    ++index;
                }
                data.Add((win, numbers));
            }

        }

        public override string PartOne()
        {
            // Перебором проверяются выигрышные номера в хеш-таблице номеров соответствующей карточки
            int result = 0;
            for (int i = 0; i < data.Count; ++i)
            {
                int count = 0;
                for (var w = 0; w < data[i].Win.Count; ++w)
                    if (data[i].Numbers.Contains(data[i].Win[w]))
                        count++;
                if (count > 0)
                    result += (int)Math.Pow(2, count - 1);
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            // Выигрышные номера складываются в очередь из которой достаются и считаются номера как в первой части.
            // Получается что-то вроде подсчёта дерева в ширину.
            Int64 result2 = data.Count;
            Queue<int> queue = new();
            for (int i = 0; i < data.Count; ++i)
                queue.Enqueue(i);

            while (queue.Count > 0)
            {
                int count = 0;
                int current = queue.Dequeue();
                for (var w = 0; w < data[current].Win.Count; ++w)
                    if (data[current].Numbers.Contains(data[current].Win[w]))
                        count++;
                for (int i = 1; i <= count; ++i)
                {
                    queue.Enqueue(current + i);
                    ++result2;
                }
            }
            return result2.ToString();
        }
    }
}