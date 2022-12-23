using AnyThings;

namespace AOC2022
{
    internal class Day21 : DayPattern<Dictionary<string, Monkey21>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine).ToList();
            data = new Dictionary<string, Monkey21>();
            foreach (var item in text)
            {
                var ss = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                data.Add(ss[0], new Monkey21(ss[0], ss[1]));
            }
        }

        public override string PartOne()
        {
            return data["root"].GetNumber(data).ToString();
        }

        public override string PartTwo()
        {
            var r = data["root"];
            var lM = new Stack<Monkey21>();
            r.GetWrongPath(data, lM);

            data["humn"].value = r.Reverse(data, lM, 0);
            if (data[data["root"].M1].GetNumber(data) == data[data["root"].M2].GetNumber(data))
                return data["humn"].value.ToString();
            return "ERROR";
        }
    }

    internal class Monkey21
    {
        public Int64 value;
        char Operation;
        public string M1;
        public string M2;
        public string Name;
        public Monkey21(string name, string v)
        {
            Name = name;
            var ss = v.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length == 1)
            {
                Operation = 'V';
                value = int.Parse(ss[0]);
            }
            else
            {
                M1 = ss[0];
                Operation = ss[1][0];
                M2 = ss[2];
            }
        }

        internal Int64 GetNumber(Dictionary<string, Monkey21> data)
        {
            if (Operation == 'V') return value;
            var o1 = data[M1].GetNumber(data);
            var o2 = data[M2].GetNumber(data);
            switch (Operation)
            {
                case '-':
                    return o1 - o2;
                case '+':
                    return o1 + o2;
                case '*':
                    return o1 * o2;
                case '/':
                    {
                        if (o1 % o2 != 0)
                        {
                            Console.WriteLine($"BREAK {o1 % o2}");
                        }
                        return o1 / o2;
                    }
            }
            throw new NotImplementedException();
        }

        public Int64 Reverse(Dictionary<string, Monkey21> data, Stack<Monkey21> wMonkeys, Int64 rootValue)
        {
            if (wMonkeys.Count == 0)
                return rootValue;
            var wM = wMonkeys.Pop();
            var gM = M1 == wM.Name ? M2 : M1;
            var goodValue = data[gM].GetNumber(data);

            Int64 nextValue = goodValue;
            if (Name != "root")
            {
                switch (Operation)
                {
                    case '+':
                        {
                            nextValue = rootValue - goodValue;
                            break;
                        }
                    case '-':
                        {
                            nextValue = M1 == wM.Name ? rootValue + goodValue : goodValue - rootValue;
                            break;
                        }
                    case '*':
                        {
                            nextValue = rootValue / goodValue;
                            break;
                        }
                    case '/':
                        {
                            nextValue = M1 == wM.Name ? rootValue * goodValue : goodValue / rootValue;
                            break;
                        }
                }
            }
            return wM.Reverse(data, wMonkeys, nextValue);
        }

        public bool GetWrongPath(Dictionary<string, Monkey21> data, Stack<Monkey21> wMonkeys)
        {
            if (Name == "humn")
                return true;
            if (Operation == 'V') return false;
            if (data[M1].GetWrongPath(data, wMonkeys))
            {
                wMonkeys.Push(data[M1]);
                return true;
            }
            if (data[M2].GetWrongPath(data, wMonkeys))
            {
                wMonkeys.Push(data[M2]);
                return true;
            }
            return false;
        }
    }
}