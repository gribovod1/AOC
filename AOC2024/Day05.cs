using AnyThings;

namespace AOC2024
{
    public class Day5Item
    {
        public int Value;
        public HashSet<Day5Item> Children = new();
        public HashSet<Day5Item> Parents = new();

        public Day5Item(int Value)
        {
            this.Value = Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    internal class Day05 : DayPattern<(string[] rules, string[] updates)>
    {
        long result2 = 0;
        public override void Parse(string singleText)
        {
            var parts = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            data.rules = parts[0].Split(Environment.NewLine);
            data.updates = parts[1].Split(Environment.NewLine);
        }

        void addChildren(Day5Item parent, Day5Item Child)
        {
            if (parent.Children.Add(Child))
                foreach (var c in Child.Children)
                    addChildren(parent, c);
        }

        void addParent(Day5Item parent, Day5Item Child)
        {
            if (Child.Parents.Add(parent))
                foreach (var p in parent.Parents)
                    addParent(p, Child);
        }

        public override string PartOne()
        {
            long result = 0;

            List<Day5Item> rules = new();
            Dictionary<int, Day5Item> map = new();

            foreach (var update in data.updates)
            {
                rules.Clear();
                map.Clear();

                var numbersText = update.Split(',');
                var numbers = new List<int>();
                foreach (var nt in numbersText)
                {
                    var n = int.Parse(nt);
                    numbers.Add(n);
                    map.Add(n, new Day5Item(n));
                }

                foreach (var r in data.rules)
                {
                    var items = r.Split('|');
                    var p = int.Parse(items[0]);
                    var c = int.Parse(items[1]);
                    if (map.ContainsKey(p) && map.ContainsKey(c))
                    {
                        addChildren(map[p], map[c]);
                        addParent(map[p], map[c]);
                    }
                }

                if (CheckUpdate(numbers, map))
                    result += numbers[numbers.Count / 2];
                else
                {
                    foreach (var i in map.Values)
                        if (i.Parents.Count == i.Children.Count)
                        {
                            result2 += i.Value;
                            break;
                        }
                }
            }
            return result.ToString();
        }

        bool CheckUpdate(List<int> numbers, Dictionary<int, Day5Item> map)
        {
            for (var i = 0; i < numbers.Count; ++i)
            {
                if (map.ContainsKey(numbers[i]))
                {
                    var item = map[numbers[i]];
                    for (var i1 = 0; i1 < numbers.Count; ++i1)
                        if (i1 < i)
                        {
                            if (map[numbers[i]].Children.Contains(map[numbers[i1]]))
                                return false;
                        }
                        else if (i1 > i)
                        {
                            if (map[numbers[i]].Parents.Contains(map[numbers[i1]]))
                                return false;
                        }
                }
            }
            return true;
        }

        public override string PartTwo()
        {
            return result2.ToString();
        }
    }
}