using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2021
{
    class Cave
    {
        string Name;

        private List<Cave> gates = new List<Cave>();

        public Cave(string v)
        {
            this.Name = v;
        }

        internal void addGate(Cave c1)
        {
            gates.Add(c1);
        }

        internal int PathCountToEnd(bool doubledSmallCave, List<Cave> path = null)
        {
            if (path == null)
                path = new List<Cave>();

            if (char.IsLower(Name[0]) && path.Any(x => x.Name == Name))
            {
                if (Name == "start" || Name == "end" || doubledSmallCave)
                    return 0;
                doubledSmallCave = true;
            }

            path.Add(this);
            try
            {
                if (Name == "end")
                    return 1;
                int result = 0;
                foreach (var c in gates)
                    result += c.PathCountToEnd(doubledSmallCave, path);
                return result;
            }
            finally
            {
                path.RemoveAt(path.Count - 1);
            }
        }
    }

    class Day12 : DayPattern<Cave>
    {
        public override void Parse(string path)
        {
            var dict = new Dictionary<string, Cave>();
            var ss = File.ReadAllLines(path);
            foreach (var s in ss)
            {
                var cs = s.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                var c1 = dict.ContainsKey(cs[0]) ? dict[cs[0]] : new Cave(cs[0]);
                var c2 = dict.ContainsKey(cs[1]) ? dict[cs[1]] : new Cave(cs[1]);
                c1.addGate(c2);
                c2.addGate(c1);
                if (!dict.ContainsKey(cs[0]))
                    dict.Add(cs[0], c1);
                if (!dict.ContainsKey(cs[1]))
                    dict.Add(cs[1], c2);
            }
            data = dict["start"];
        }

        public override string PartOne()
        {
            return data.PathCountToEnd(true).ToString();
        }

        public override string PartTwo()
        {
            return data.PathCountToEnd(false).ToString();
        }
    }
}
