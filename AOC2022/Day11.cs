using AnyThings;
using System.Drawing;
using System.Numerics;

namespace AOC2022
{
    class Item
    {
        List<(Monkey, BigInteger)> path;

        BigInteger Start;

        public void ShowPath()
        {
            Console.Write(Start + ": ");
            var pp = 0;
            for (var p=0;p<path.Count;++p)
                if (path[p].Item1.Operation==2)
            {
                Console.Write((p - pp) + ",");
                    pp = p;
            }
            Console.WriteLine();
        }

        public Item(Monkey m, int worry)
        {
            Start = worry;
            path = new List<(Monkey, BigInteger)>() { (m, worry) };
        }

        BigInteger FindCycleValue()
        {
            if (path.Count > 0 && path[path.Count - 1].Item1.Operation == 2)
            {
                for (var i = 0; i < path.Count; i++)
                    if (path[i].Item1.Operation == 2)
                    {
                    //    return path[i].Item2;
                    }
            }
            return -1;



            int result = -1;
            int count = 1;

            while (count < path.Count / 2)
            {
                var i = 0;
                for (; i < count; ++i)
                {
                    if (path[path.Count - 1 - i].Item1 != path[path.Count - 1 - (i + count)].Item1)
                        break;
                }
                if (i > count)
                    result = count;
                ++count;
            }
            if (result > 0)
                return path[path.Count - result * 2].Item2;
            return -1;
        }

        void TrimTwo()
        {
            var cycleValue = FindCycleValue();
            if (cycleValue == -1)
                return;
            worry = cycleValue;
        }

        BigInteger worry
        {
            get
            {
                return path[path.Count - 1].Item2;
            }

            set
            {
                path.Add((path[path.Count - 1].Item1, value));
                path.RemoveAt(path.Count - 2);
            }
        }
        void TrimOne()
        {
            //   worry = (int)Math.Floor(worry / 3.0);
        }

        public void ToNextMonkey(Monkey m)
        {
            path.Add((m, worry));
        }

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
            TrimTwo();
        }
    }

    class Monkey
    {
        List<Item> items = new List<Item>();

        public void ShowItems()
        {
            foreach (var i in items)
                i.ShowPath();
            Console.WriteLine();
        }

        int Rule;

        int OperationValue;
        public int Operation;

        int DestTrue;
        int DestFalse;
        int Index;

        public override string ToString()
        {
            return Index.ToString();
            return "M" + Rule;
        }

        public void addItem(Item item)
        {
            items.Add(item);
            item.ToNextMonkey(this);
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

        public Monkey(string text, int index)
        {
            Index = index;
            var ss = text.Split(Environment.NewLine);
            var itms = ss[1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 2; i < itms.Length; ++i)
                items.Add(new Item(this, int.Parse(itms[i])));
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

        public UInt64 Activity;
    }

    internal class Day11 : DayPattern<List<Monkey>>
    {
        public override void Parse(string path)
        {
            var text = File.ReadAllText(path).Split(Environment.NewLine + Environment.NewLine);
            data = new List<Monkey>();
            foreach (var s in text)
                data.Add(new Monkey(s, data.Count));
        }

        public override string PartOne()
        {
            InitData();
            for (var i = 0; i < 400; ++i)
            {
                foreach (var m in data)
                    m.Round(data);
            }
            var a = new List<UInt64>();
            foreach (var m in data)
            {
                a.Add(m.Activity);
                m.ShowItems();
            }
            a.Sort();
            return (a[a.Count - 1] * a[a.Count - 2]).ToString();
        }

        public override string PartTwo()
        {
            InitData();

            return 0.ToString();
        }

        public override void InitData()
        {

        }
    }
}