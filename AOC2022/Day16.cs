using AnyThings;

namespace AOC2022
{
    internal class Day16 : DayPattern<Dictionary<string, Valve>>
    {
        int summaryRate = 0;
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine).ToList();
            data = new();
            for (var sIndex = 0; sIndex < text.Count; ++sIndex)
            {
                var ss = text[sIndex].Split(new char[] { ' ', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var rt = ss[4].Split('=');
                var v = new Valve(ss[1], int.Parse(rt[1]));
                v.Index = sIndex;
                for (var i = 9; i < ss.Length; ++i)
                    v.AddValve(ss[i]);
                data.Add(v.Name, v);
                summaryRate += v.Rate;
            }
            foreach (var v in data)
                v.Value.LoadDistances(data);
        }

        public override string PartOne()
        {
            maxResult = 0;
            return GetMaxPathE(data["AA"], 0, data["AA"], 100, 30, 0, 0, summaryRate).ToString();
        }

        public override string PartTwo()
        {
            maxResult = 0;
            return GetMaxPathE(data["AA"], 0, data["AA"], 0, 26, 0, 0, summaryRate).ToString();
        }

        bool IsNotUsed(UInt64 valves, Valve valve)
        {
            return (valves & ((UInt64)1 << valve.Index)) == 0;
        }

        UInt64 SetUsed(UInt64 valves, Valve valve)
        {
            return valves | ((UInt64)1 << valve.Index);
        }

        int maxResult = 0;

        int GetMaxPathE(Valve source, int busy, Valve sourceElephant, int busyElephant, int restPath, UInt64 usedValves, int currentResult, int restRate)
        {
            int result = currentResult;
            if (restPath > 0)
            {
                if (restRate * (restPath - 1) + currentResult > maxResult)
                {
                    if (busy == 0)
                    {
                        if (busyElephant == 0)
                        {
                            foreach (var d in source.Distances)
                                if (d.Value + 1 <= restPath && IsNotUsed(usedValves, d.Key))
                                {
                                    var used = SetUsed(usedValves, d.Key);
                                    foreach (var dE in sourceElephant.Distances)
                                        if (dE.Value + 1 <= restPath && IsNotUsed(used, dE.Key))
                                        {
                                            var steps = Math.Min(d.Value + 1, dE.Value + 1);
                                            var currentRate = d.Key.Rate * (restPath - d.Value - 1) + dE.Key.Rate * (restPath - dE.Value - 1);
                                            var max = GetMaxPathE(d.Key, d.Value + 1 - steps, dE.Key, dE.Value + 1 - steps, restPath - steps, SetUsed(used, dE.Key), currentResult + currentRate, restRate - d.Key.Rate - dE.Key.Rate);
                                            if (result < max)
                                                result = max;
                                        }
                                }
                        }
                        else
                        {
                            foreach (var d in source.Distances)
                                if (d.Value + 1 <= restPath && IsNotUsed(usedValves, d.Key))
                                {
                                    var steps = Math.Min(d.Value + 1, busyElephant);
                                    var currentRate = d.Key.Rate * (restPath - d.Value - 1);
                                    var max = GetMaxPathE(d.Key, d.Value + 1 - steps, sourceElephant, busyElephant - steps, restPath - steps, SetUsed(usedValves, d.Key), currentResult + currentRate, restRate - d.Key.Rate);
                                    if (result < max)
                                        result = max;
                                }
                        }
                    }
                    else
                    {
                        if (busyElephant == 0)
                        {
                            foreach (var d in sourceElephant.Distances)
                                if (d.Value + 1 <= restPath && IsNotUsed(usedValves, d.Key))
                                {
                                    var steps = Math.Min(busy, d.Value + 1);
                                    var currentRate = d.Key.Rate * (restPath - d.Value - 1);
                                    var max = GetMaxPathE(source, busy - steps, d.Key, d.Value + 1 - steps, restPath - steps, SetUsed(usedValves, d.Key), currentResult + currentRate, restRate - d.Key.Rate);
                                    if (result < max)
                                        result = max;
                                }
                        }
                        else
                        {
                            var steps = Math.Min(busy, busyElephant);
                            result = GetMaxPathE(source, busy - steps, sourceElephant, busyElephant - steps, restPath - steps, usedValves, currentResult, restRate);
                        }
                    }
                }
            }
            if (result > maxResult)
                maxResult = result;
            return result;
        }
    }

    class Valve
    {
        public int Index;
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
                if (v.Value != this && !Distances.ContainsKey(v.Value) && v.Value.Rate > 0)
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