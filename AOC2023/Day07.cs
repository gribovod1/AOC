using AnyThings;

namespace AOC2023
{
    class CardDay07
    {
        public List<char> Combination;
        public Int64 Bid;
        public int type;

        public CardDay07(List<char> combination, long bid, int type)
        {
            Combination = combination;
            Bid = bid;
            this.type = type;
        }
    }

    internal class Day07 : DayPattern<List<CardDay07>>
    {
        public override void Parse(string singleText)
        {
            var table = singleText.Split(Environment.NewLine);
            data = new();
            foreach (var item in table)
            {
                var description = item.Split(' ');
                data.Add(new CardDay07(description[0].ToList(), Int64.Parse(description[1]), 0));
            }
        }

        public override string PartOne()
        {
            // 
            Int64 result = 0;
            data.Sort(SortCombination);
            for (int i = 0; i < data.Count; ++i)
            {
                result += data[i].Bid * (i + 1);
            }
            return result.ToString();
        }

        int SortCombination(CardDay07 x, CardDay07 y)
        {
            if (x.type == 0)
                x.type = GetType(new(x.Combination));
            if (y.type == 0)
                y.type = GetType(new(y.Combination));
            if (x.type != y.type)
                return x.type.CompareTo(y.type);
            for (int i = 0; i < 5; ++i)
            {
                var c = SortCard(x.Combination[i], y.Combination[i]);
                if (c != 0)
                    return c;
            }
            return 0;
        }

        int GetType(List<char> Combination)
        {
            Combination.Sort(SortCard);
            List<int> counts = GetCounts(Combination);
            if (counts.Count == 1) return 7;
            if (counts.Count == 2)
            {
                // каре или фулл-хаус
                if (counts[0] == 4)
                    return 6;
                return 5;
            }
            if (counts.Count == 3)
            {// две пары или тройка
                if (counts[0] == 3)
                    return 4;
                return 3;
            }
            if (counts.Count == 4) return 2;
            if (counts.Count == 5) return 1;
            return 0;
        }

        private List<int> GetCounts(List<char> combination)
        {
            List<int> result = new();
            char c = combination[0];
            int start = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (combination[i] != c)
                {
                    result.Add(i - start);
                    c = combination[i];
                    start = i;
                }
            }
            result.Add(5 - start);
            result.Sort();
            result.Reverse();
            return result;
        }

        int SortCard(char x, char y)
        {
            if (x == y)
                return 0;
            if (char.IsDigit(x))
            {
                if (char.IsDigit(y))
                    return x.CompareTo(y);
                return -1;
            }
            else
            {
                if (char.IsDigit(y))
                    return 1;
                if (x == 'J')
                {
                    if (y == 'T')
                        return 1;
                    return -1;
                }
                if (y == 'J')
                {
                    if (x == 'T')
                        return -1;
                    return 1;
                }
                return -x.CompareTo(y);
            }
        }

        public override string PartTwo()
        {
            Int64 result = 0;
            for (int i = 0; i < data.Count; ++i)
                data[i].type = 0;
            data.Sort(SortCombination2);
            for (int i = 0; i < data.Count; ++i)
            {
                result += data[i].Bid * (i + 1);
            }
            return result.ToString();
        }

        int SortCombination2(CardDay07 x, CardDay07 y)
        {
            if (x.type == 0)
                x.type = GetType2(new(x.Combination));
            if (y.type == 0)
                y.type = GetType2(new(y.Combination));
            if (x.type != y.type)
                return x.type.CompareTo(y.type);
            for (int i = 0; i < 5; ++i)
            {
                var c = SortCard2(x.Combination[i], y.Combination[i]);
                if (c != 0)
                    return c;
            }
            return 0;
        }

        int GetType2(List<char> Combination)
        {
            Combination.Sort(SortCard2);
            List<int> counts = GetCounts2(Combination);
            if (counts.Count == 1) return 7;
            if (counts.Count == 2)
            {
                // каре или фулл-хаус
                if (counts[0] == 4)
                    return 6;
                return 5;
            }
            if (counts.Count == 3)
            {// две пары или тройка
                if (counts[0] == 3)
                    return 4;
                return 3;
            }
            if (counts.Count == 4) return 2;
            if (counts.Count == 5) return 1;
            return 0;
        }

        private List<int> GetCounts2(List<char> combination)
        {
            List<int> result = new();
            char c = combination[0];
            int start = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (combination[i] != c)
                {
                    result.Add(i - start);
                    c = combination[i];
                    start = i;
                }
            }
            result.Add(5 - start);
            var up = 0;
            if (combination[0] == 'J' && result.Count > 1)
            {
                up = result[0];
                result.RemoveAt(0);
            }
            result.Sort();
            result.Reverse();
            result[0] += up;
            return result;
        }

        int SortCard2(char x, char y)
        {
            if (x == y)
                return 0;
            if (x == 'J')
                return -1;
            if (y == 'J')
                return 1;
            if (char.IsDigit(x))
            {
                if (char.IsDigit(y))
                    return x.CompareTo(y);
                return -1;
            }
            else
            {
                if (char.IsDigit(y))
                    return 1;
                return -x.CompareTo(y);
            }
        }

    }
}