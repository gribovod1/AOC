using AnyThings;
using System.Collections.Generic;
using System.Text;
using Coord = (int x, int y);

namespace AOC2024
{
    class Comp : IComparable
    {
        public string Name;

        public Dictionary<string, Comp> Comps;
        public Comp(string name)
        {
            Name = name;
            Comps = new();
        }

        public Comp(string name, Comp link)
        {
            Name = name;
            Comps = new();
            AddLink(link);
        }

        public void AddLink(Comp link)
        {
            Comps.Add(link.Name, link);
        }

        public int CompareTo(object? obj)
        {
            var comp = obj as Comp;
            if (comp != null)
                return Name.CompareTo(comp.Name);
            return 0;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    internal class Day23 : DayPattern<Dictionary<string, Comp>>
    {
        public override void Parse(string singleText)
        {
            data = new();
            var comps = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var link in comps)
            {
                var pair = link.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (!data.ContainsKey(pair[0]))
                    data.Add(pair[0], new Comp(pair[0]));
                if (!data.ContainsKey(pair[1]))
                    data.Add(pair[1], new Comp(pair[1]));
                data[pair[0]].AddLink(data[pair[1]]);
                data[pair[1]].AddLink(data[pair[0]]);
            }
        }

        public override string PartOne()
        {
            long result = 0;
            HashSet<string> triad = new();
            foreach (var c in data)
            {
                if (c.Value.Comps.Count > 1)
                {
                    var linked = c.Value.Comps.Values.ToList();
                    for (int i = 0; i < linked.Count; i++)
                        for (int i1 = i + 1; i1 < linked.Count; i1++)
                            if (linked[i].Comps.ContainsKey(c.Key) && linked[i].Comps.ContainsKey(linked[i1].Name) &&
                            linked[i1].Comps.ContainsKey(c.Key) && linked[i1].Comps.ContainsKey(linked[i].Name))
                            {
                                List<Comp> trio = new();
                                trio.Add(c.Value);
                                trio.Add(linked[i]);
                                trio.Add(linked[i1]);
                                bool needAdded = false;
                                needAdded |= c.Value.Name[0] == 't';
                                needAdded |= linked[i].Name[0] == 't';
                                needAdded |= linked[i1].Name[0] == 't';
                                trio.Sort();
                                StringBuilder sb = new();
                                for (int t = 0; t < 3; ++t)
                                    sb.Append(trio[t].Name + " ");
                                var desc = sb.ToString();
                                if (!triad.Contains(desc))
                                {
                                    triad.Add(desc);
                                    if (needAdded) ++result;
                                }
                            }
                }
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            int maxCount = 0;
            List<Comp> maxLAN = null;
            var comps = data.Values.ToHashSet();
            while (comps.Count > 0)
            {
                var c = comps.First();
                var lan = GetMaxLAN(new(), c.Comps.Values.ToList(), c, maxCount);
                if (maxCount < lan.Count)
                {
                    maxCount = lan.Count;
                    maxLAN = lan;
                }
                comps.Remove(c);
            }

            maxLAN.Sort();
            StringBuilder result = new();
            foreach (var l in maxLAN)
                result.Append($"{l.Name},");
            return result.ToString(1, result.Length - 1);
        }

        List<Comp> GetMaxLAN(List<Comp> currentLAN, List<Comp> futureLAN, Comp current, int maxSize)
        {
            if (current.Comps.Count < maxSize) return currentLAN;

            foreach(var c in futureLAN)
            {
                var result = new List<Comp>(currentLAN);
                result.Add(current);
                var sub = GetMaxLAN(result, futureLAN.Intersect(c.Comps.Values.ToList()), c, maxSize);
            }



            return result;
        }
    }
}