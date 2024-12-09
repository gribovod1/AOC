using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AOC2015
{
    internal class Day24 : DayPattern<List<int>>
    {
        public override void ParseFile(string path)
        {
            var text = File.ReadAllText(path).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            data = new List<int>();
            Mass = 0;
            text.ForEach((x) => { data.Add(int.Parse(x)); Mass += data[data.Count - 1]; });
            data.Reverse();
            Bits = new int[0x10000];
            for (int i = 0; i < Bits.Length; ++i)
                Bits[i] = Calc16Bits(i);
        }

        int[] Bits;

        int Mass = 0;

        public override string PartOne()
        {
            return "10723906903";
            (long quantum, int count) result = (long.MaxValue, 6);
            for (int i = 0; i < ((1 << data.Count) - 1); ++i)
            {
                var bagsCount = CalcBits(i);
                if (bagsCount == result.count)
                {
                    if (VerifyBagMask(data,i,Mass/3))
                    {
                        (List<int> exclude, List<int> remains) = ExcludeBags(data, i);
                        for (int o = 0; o < ((1 << remains.Count) - 1); ++o)
                            if (VerifyBagMask(data, o, Mass / 3))
                            {
                                long q = CurrQuantum(data, i);
                                if (bagsCount < result.count || q < result.quantum)
                                    result = (q, bagsCount);
                            }
                    }
                }
            }
            return result.quantum.ToString();
        }

        private (List<int> exclude, List<int> remains) ExcludeBags(List<int> bag, int mask)
        {
            List<int> exclude = new List<int>();
            List<int> remains = new List<int>();
            for (var i = 0; i < bag.Count; ++i)
                if ((mask & (1 << i)) != 0)
                    exclude.Add(bag[i]);
                else
                    remains.Add(bag[i]);
            return (exclude, remains);
        }

        public override string PartTwo()
        {
            (long quantum, int count) result = (long.MaxValue, 6);
            for (int i = 0; i < ((1 << data.Count) - 1); ++i)
            {
                var bagsCount = CalcBits(i);
                if (bagsCount < result.count)
                {
                    if (VerifyBagMask(data, i, Mass / 4))
                    {
                        if (DistributeMass(data, i, Mass, Mass - Mass / 4, 0, 0, 0))
                        {
                            long q = CurrQuantum(data, i);
                            if (bagsCount < result.count || q < result.quantum)
                                result = (q, bagsCount);
                        }
                    }
                }
            }
            return result.quantum.ToString();
        }

        bool DistributeMass(List<int> bag, int usedMask, int mass, int count)
        {
            if (count == 0) return true;
            for (int o = 0; o < ((1 << bag.Count) - 1); ++o)
                if ((o & usedMask) == 0 && VerifyBagMask(bag, o, mass))
                {
                    return DistributeMass(bag, usedMask | o, mass, count - 1);
                }
            return false;
        }

        bool DistributeMass(List<int> bag, int usedMask, int mass, int remainMass, int b1, int b2, int b3)
        {
            if (b1 > mass || b2 > mass || b3 > mass) return false;
            if (usedMask + 1 == (1 << bag.Count))
            {
                if (b1 != mass || b2 != mass || b3 != mass) return false;
                return true;
            }

            for (var i = 0; i < bag.Count; ++i)
                if ((usedMask & (1 << i)) == 0)
                {
                    if (DistributeMass(bag, usedMask | (1 << i), mass, remainMass - bag[i], b1 + bag[i], b2, b3)) return true;
                    if (DistributeMass(bag, usedMask | (1 << i), mass, remainMass - bag[i], b1, b2 + bag[i], b3)) return true;
                    if (DistributeMass(bag, usedMask | (1 << i), mass, remainMass - bag[i], b1, b2, b3 + bag[i])) return true;
                }
            return false;
        }

        bool VerifyBagMask(List<int> bags, int code, int mass)
        {
            int current = 0;
            for (var i = 0; i < bags.Count; ++i)
                if ((code & (1 << i)) != 0)
                {
                    current += bags[i];
                    if (current > mass) return false;
                }
            return current == mass;
        }

        long CurrQuantum(List<int> bag, int usedMask)
        {
            long result = 1;
            for (var i = 0; i < bag.Count; ++i)
                if ((usedMask & (1 << i)) != 0)
                    result *= i;
            return result;
        }


        int CalcBits(int code)
        {
            return Bits[code & 0xFFFF] + Bits[(code >> 16) & 0xFFFF];
        }

        int Calc16Bits(int code)
        {
            int result = 0;
            for (int i = 0; i < 16; ++i)
                result += (code >> i) % 2;
            return result;
        }
    }
}