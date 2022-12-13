using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2015
{
    internal class Day24 : DayPattern<List<int>>
    {
        public override void Parse(string path)
        {
            var text = File.ReadAllText(path).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            data = new List<int>();
            text.ForEach((x) => { data.Add(int.Parse(x)); });
          //  data.Reverse();
        }

        public override string PartOne()
        {
            var minCount = data.Count / 3;
            var minQuantum = int.MaxValue;
            GetMaxQuantum(data, null, ref minCount, ref minQuantum);
            return minQuantum.ToString();
        }

        public override string PartTwo()
        {
            return 0.ToString();
        }

        void GetMaxQuantum(List<int> source, List<int>[] bags, ref int count, ref int quantum)
        {
            if (bags == null)
            {
                bags = new List<int>[3];
                for (var b = 0; b < bags.Length; ++b)
                    bags[b] = new List<int>();
            }
            if (Fatal(source, bags, count)) return;
            if (source.Count == 0)
            {
                if (Disbalance(bags))
                    return;
                if (bags[0].Count < count)
                {
                    count = bags[0].Count;
                    quantum = currQuantum(bags[0]);
                } else if (bags[0].Count == count)
                {
                    if (currQuantum(bags[0]) < quantum)
                        quantum = currQuantum(bags[0]);
                }
                return;
            }
            var bs = CloneBags(bags);
            bs[0].Add(source[0]);
            GetMaxQuantum(new List<int>(source.Skip(1)), bs, ref count, ref quantum);
            bs = CloneBags(bags);
            bs[1].Add(source[0]);
            GetMaxQuantum(new List<int>(source.Skip(1)), bs, ref count, ref quantum);
            bs = CloneBags(bags);
            bs[2].Add(source[0]);
            GetMaxQuantum(new List<int>(source.Skip(1)), bs, ref count, ref quantum);
        }

        int currQuantum(List<int> bag)
        {
            var result = 1;
            foreach (var i in bag)
                result *= i;
            return result;
        }

        List<int>[] CloneBags(List<int>[] bags)
        {
            var result = new List<int>[bags.Length];
            for (var b = 0; b < result.Length; ++b)
                result[b] = new List<int>(bags[b]);
            return result;
        }

        bool Disbalance(List<int>[] bags)
        {
            if (bags[0].Count > bags[1].Count || bags[0].Count > bags[2].Count)
                return true;
            var w = bags[0].Sum();
            if (bags[1].Sum() != w || bags[2].Sum() != w)
                return true;
            return false;
        }

        bool Fatal(List<int> source, List<int>[] bags, int count)
        {
            if (bags[0].Count > count) return true;
            return false;
        /*    if (bags[0].Count - bags[1].Count - bags[2].Count > source.Count)
                return true;*/

            var r = source.Sum();

            var d1 = Math.Abs(bags[0].Sum() - bags[1].Sum());
            var d2 = Math.Abs(bags[0].Sum() - bags[2].Sum());

            return d1 + d2 > r;
        }
    }
}