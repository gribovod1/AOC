using AnyThings;

namespace AOC2022
{
    internal class Day16 : DayPattern<Dictionary<string, Valve>>
    {
        int rateCount = 0;
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine).ToList();
            data = new();
            foreach (var s in text)
            {
                var ss = s.Split(new char[] { ' ', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var rt = ss[4].Split('=');
                var v = new Valve(ss[1], int.Parse(rt[1]));
                for (var i = 9; i < ss.Length; ++i)
                    v.AddValve(ss[i]);
                data.Add(v.Name, v);
                if (v.Rate > 0) ++rateCount;
            }
            foreach (var v in data)
                v.Value.LoadDistances(data);
        }

        public override string PartOne()
        {
            var p = new HashSet<string>();
            p.Add("AA");
            return GetMaxPath(data["AA"], 30, p).ToString();
        }

        public override string PartTwo()
        {
            var p = new HashSet<string>();
            p.Add("AA");
            var youResult = GetMaxPath2(data["AA"], 26, p);
            var elephantResult = GetMaxPath2(data["AA"], 26, youResult.Item2);
            return (youResult.Item1 + elephantResult.Item1).ToString();
        }

        int GetMaxPathOld(int restPath, Valve yourValve, Valve elephantValve, int yourCounter, int elephantCounter, HashSet<string> vs, int delta)
        {
            if (restPath <= 0) return delta;
            var result = 0;
            if (yourCounter > 0 && elephantCounter > 0)
                return delta + GetMaxPathOld(restPath - 1, yourValve, elephantValve, yourCounter - 1, elephantCounter - 1, vs, delta);
            else if (yourCounter <= 0 && elephantCounter <= 0)
            {
                foreach (var v in data)
                    if (v.Value.Rate > 0 && !vs.Contains(v.Key))
                        foreach (var e in data)
                            if (e.Key != v.Key && e.Value.Rate > 0 && !vs.Contains(e.Key))
                            {
                                var yourDistance = GetLengthPath(yourValve, v.Value);
                                var elephantDistance = GetLengthPath(elephantValve, e.Value);
                                var nvs = new HashSet<string>(vs);
                                nvs.Add(v.Key);
                                nvs.Add(e.Key);
                                var max = GetMaxPathOld(restPath - 1, v.Value, e.Value, yourDistance, elephantDistance, nvs, delta + v.Value.Rate + e.Value.Rate);
                                if (max > result)
                                    result = max;
                            }
            }
            else if (yourCounter <= 0)
            {
                foreach (var v in data)
                    if (v.Value.Rate > 0 && !vs.Contains(v.Key))
                    {
                        var yourDistance = GetLengthPath(yourValve, v.Value);
                        var nvs = new HashSet<string>(vs);
                        nvs.Add(v.Key);
                        var max = GetMaxPathOld(restPath - 1, v.Value, elephantValve, yourDistance, elephantCounter - 1, nvs, delta + v.Value.Rate);
                        if (max > result)
                            result = max;
                    }
            }
            else
            {
                foreach (var e in data)
                    if (e.Value.Rate > 0 && !vs.Contains(e.Key))
                    {
                        var elephantDistance = GetLengthPath(elephantValve, e.Value);
                        var nvs = new HashSet<string>(vs);
                        nvs.Add(e.Key);
                        var max = GetMaxPathOld(restPath - 1, yourValve, e.Value, yourCounter - 1, elephantDistance, nvs, delta + e.Value.Rate);
                        if (max > result)
                            result = max;
                    }
            }
            return delta + result;
        }

        (int, HashSet<string>) GetMaxPath2(Valve source, int restPath, HashSet<string> vs)
        {
            if (restPath <= 0) return (0, vs);
            var result = (0, vs);
            if (vs.Count < rateCount + 1)
                foreach (var v in data)
                    if (!vs.Contains(v.Key) && v.Value.Rate > 0)
                    {
                        var r = GetLengthPath(source, v.Value);
                        var nvs = new HashSet<string>(vs);
                        nvs.Add(v.Key);
                        var max = GetMaxPath2(v.Value, restPath - (source.Rate > 0 ? 1 : 0) - r, nvs);
                        if (max.Item1 > result.Item1)
                            result = max;
                    }
            return (result.Item1 + source.Rate * (restPath - 1), result.Item2);
        }

        int GetMaxPath(Valve source, int restPath, HashSet<string> vs)
        {
            if (restPath <= 0) return 0;
            var result = 0;
            if (vs.Count < rateCount + 1)
                foreach (var v in data)
                    if (!vs.Contains(v.Key) && v.Value.Rate > 0)
                    {
                        var r = GetLengthPath(source, v.Value);
                        var nvs = new HashSet<string>(vs);
                        nvs.Add(v.Key);
                        var max = GetMaxPath(v.Value, restPath - (source.Rate > 0 ? 1 : 0) - r, nvs);
                        if (max > result)
                            result = max;
                    }
            return result + source.Rate * (restPath - 1);
        }

        int GetLengthPath(Valve source, Valve target)
        {
            return source.Distances[target];
        }
    }

    class Valve
    {
        public int Rate;
        public List<string> valves = new();
        public Dictionary<Valve, int> Distances = new();
        public string Name;
        public override string ToString()
        {
            return Name;
        }
        public Valve(string name, int rate)
        {
            Name = name;
            Rate = rate;
        }

        public void AddValve(string v)
        {
            valves.Add(v);
        }

        internal void LoadDistances(Dictionary<string, Valve> data)
        {
            foreach (var v in data)
                if (v.Value != this && !Distances.ContainsKey(v.Value))
                {
                    var d = GetDistance(data, v.Value);
                    Distances.Add(v.Value, d);
                    v.Value.Distances.Add(this, d);
                }
        }

        int GetDistance(Dictionary<string, Valve> data, Valve target)
        {
            HashSet<Valve> h = new();
            var stepsCount = -1;
            Queue<Valve> queue = new Queue<Valve>();
            queue.Enqueue(this);
            do
            {
                var qCount = queue.Count;
                for (var i = 0; i < qCount; i++)
                {
                    var v = queue.Dequeue();
                    h.Add(v);
                    foreach (var s in v.valves)
                        queue.Enqueue(data[s]);
                }
                ++stepsCount;
            }
            while (!h.Contains(target));
            return stepsCount;
        }
    }
}