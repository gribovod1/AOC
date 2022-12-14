using AnyThings;

namespace AOC2022
{
    internal class Day11 : DayPattern<List<Monkey>>
    {
        public override void Parse(string text)
        {
            this.text = text;
            var lines = text.Split(Environment.NewLine + Environment.NewLine);
            data = new List<Monkey>();
            foreach (var s in lines)
                data.Add(new Monkey(s, trimmedLoad));
        }

        public override string PartOne()
        {
            for (var i = 0; i < 20; ++i)
                foreach (var m in data)
                    m.Round(data);
            var a = new List<UInt64>();
            foreach (var m in data)
                a.Add(m.Activity);
            a.Sort();
            return (a[a.Count - 1] * a[a.Count - 2]).ToString();
        }

        public override string PartTwo()
        {
            trimmedLoad = false;
            Parse(text);
            for (var i = 0; i < 10000; ++i)
                foreach (var m in data)
                    m.Round(data);
            var a = new List<UInt64>();
            foreach (var m in data)
                a.Add(m.Activity);
            a.Sort();
            return (a[a.Count - 1] * a[a.Count - 2]).ToString();
        }

        bool trimmedLoad = true;
        string text = string.Empty;
    }

    class Monkey
    {
        public void addItem(IItem item)
        {
            items.Add(item);
        }

        public void Round(List<Monkey> monkeys)
        {
            foreach (var item in items)
            {
                ++Activity;
                item.UpdateWorry(Operation, OperationValue);
                var dest = item.Test(Rule) ? DestTrue : DestFalse;
                monkeys[dest].addItem(item);
            }
            items.Clear();
        }

        public Monkey(string text, bool trimed)
        {
            var ss = text.Split(Environment.NewLine);
            var itms = ss[1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 2; i < itms.Length; ++i)
                items.Add(trimed ? new ItemTrimmed(int.Parse(itms[i])) : new Item(int.Parse(itms[i])));
            Operation = ss[2].IndexOf('*') > 0 ? 1 : 0;
            var opers = ss[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (opers[5] == "old")
            {
                Operation = 2;
            }
            else
            {
                OperationValue = int.Parse(opers[5]);
            }
            var tests = ss[3].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Rule = int.Parse(tests[tests.Length - 1]);
            var destTrues = ss[4].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            DestTrue = int.Parse(destTrues[destTrues.Length - 1]);
            var destFalse = ss[5].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            DestFalse = int.Parse(destFalse[destFalse.Length - 1]);
        }

        List<IItem> items = new List<IItem>();
        int Rule;
        int OperationValue;
        int Operation;

        int DestTrue;
        int DestFalse;

        public UInt64 Activity;
    }

    interface IItem
    {
        void UpdateWorry(int Operation, int OperationValue);
        bool Test(int Rule);
    }

    class ItemTrimmed : IItem
    {
        public ItemTrimmed(int worry)
        {
            this.worry = worry;
        }

        int worry;

        public bool Test(int Rule)
        {
            return worry % Rule == 0;
        }

        public void UpdateWorry(int Operation, int OperationValue)
        {
            switch (Operation)
            {
                case 0:
                    {
                        worry += OperationValue;
                        break;
                    }
                case 1:
                    {
                        worry *= OperationValue;
                        break;
                    }
                case 2:
                    {
                        worry *= worry;
                        break;
                    }
            }
            Trim();
        }

        void Trim()
        {
            worry = (int)Math.Floor(worry / 3.0);
        }
    }

    class Item : IItem
    {
        public Item(int worry)
        {
            this.worry = new ModuleInteger(worry);
        }

        public bool Test(int Rule)
        {
            return worry.IsSimpleDivided(Rule);
        }

        public void UpdateWorry(int Operation, int OperationValue)
        {
            switch (Operation)
            {
                case 0:
                    {
                        worry += OperationValue;
                        break;
                    }
                case 1:
                    {
                        worry *= OperationValue;
                        break;
                    }
                case 2:
                    {
                        worry *= worry;
                        break;
                    }
            }
        }

        ModuleInteger worry;
    }
}