using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2021
{
    class Day14 : DayPattern<string>
    {
        Dictionary<string, char> inst = new Dictionary<string, char>();
        Dictionary<string, long> pairs = new Dictionary<string, long>();

        public override void Parse(string path)
        {
            var ss = File.ReadAllLines(path);
            data = ss[0];
            for (var i = 0; i < data.Length - 1; ++i)
                upDictCounter<string>(pairs, data.Substring(i, 2), 1);

            for (var i = 2; i < ss.Length; ++i)
            {
                var si = ss[i].Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                inst.Add(si[0], si[1][0]);
            }
        }

        void upDictCounter<T>(Dictionary<T, long> dict, T key, long value)
        {
            if (dict.ContainsKey(key))
                dict[key] = dict[key] + value;
            else
                dict.Add(key, value);
        }

        public override string PartOne()
        {
            for (var i = 0; i < 10; ++i)
                polymerizationPairs();
            return diffSymb().ToString();
        }

        public override string PartTwo()
        {
            for (var i = 0; i < 30; ++i)
                polymerizationPairs();
            return diffSymb().ToString();
        }

        private void polymerizationPairs()
        {
            Dictionary<string, long> newPairs = new Dictionary<string, long>();

            foreach (var p in pairs)
            {
                if (inst.ContainsKey(p.Key))
                {
                    var np1 = string.Empty + p.Key[0] + inst[p.Key];
                    var np2 = string.Empty + inst[p.Key] + p.Key[1];
                    upDictCounter<string>(newPairs, np1, p.Value);
                    upDictCounter<string>(newPairs, np2, p.Value);
                }
                else
                    upDictCounter<string>(newPairs, p.Key, p.Value);
            }
            pairs = newPairs;
        }

        long diffSymb()
        {
            var charCounter = new Dictionary<char, long>();
            foreach (var p in pairs)
            {
                upDictCounter<char>(charCounter, p.Key[0], p.Value);
                upDictCounter<char>(charCounter, p.Key[1], p.Value);
            }
            charCounter[data[0]] = charCounter[data[0]] + 1;
            charCounter[data[data.Length - 1]] = charCounter[data[data.Length - 1]] + 1;
            return (charCounter.Max(x => x.Value) - charCounter.Min(x => x.Value)) / 2;
        }
    }
}