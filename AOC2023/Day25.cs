using AnyThings;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AOC2023
{
    class FinalNode
    {
        public List<FinalNode> Nodes = new();

        public string Name;

        public double Value = 0;
        public FinalNode() { }

        public FinalNode(string name)
        {
            Name = name;
        }
        public void Stream(Queue<FinalNode> queue, HashSet<FinalNode> processed, Dictionary<string, double> UsedPipe)
        {
            List<FinalNode> nodes = new();
            foreach (var n in Nodes)
                if (n.Value < Value && (!UsedPipe.ContainsKey(GetPipeName(this, n)) || UsedPipe[GetPipeName(this, n)] < 1))
                    nodes.Add(n);
            if (nodes.Count > 0)
            {
                nodes.Sort((FinalNode x, FinalNode y) => { return y.Value.CompareTo(x.Value); });
                var d = Math.Min(1, (Value - nodes[0].Value) / (double)(nodes.Count + 1));
                for (int i = 0; i < nodes.Count; ++i)
                {
                    if (!UsedPipe.ContainsKey(GetPipeName(this, nodes[i])))
                        UsedPipe[GetPipeName(this, nodes[i])] = 0;
                    var value = Math.Min(d, 1 - UsedPipe[GetPipeName(this, nodes[i])]);
                    nodes[i].Value += value;
                    UsedPipe[GetPipeName(this, nodes[i])] += value;
                    Value -= value;
                }
                for (int i = 0; i < nodes.Count; ++i)
                    if (!processed.Contains(nodes[i]))
                    {
                        queue.Enqueue(nodes[i]);
                        processed.Add(nodes[i]);
                    }
            }
        }

        string GetPipeName(FinalNode x, FinalNode y)
        {
            if (x.Name.CompareTo(y.Name) > 0)
                return $"{x.Name}_{y.Name}";
            return $"{y.Name}_{x.Name}";
        }
    }

    internal class Day25 : DayPattern<Dictionary<string, FinalNode>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (int row = 0; row < text.Length; row++)
            {
                var ns = text[row].Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                FinalNode node;
                if (!data.TryGetValue(ns[0], out node))
                {
                    node = new FinalNode(ns[0]);
                    data.Add(node.Name, node);
                }
                for (int i = 1; i < ns.Length; ++i)
                {
                    FinalNode next;
                    if (!data.TryGetValue(ns[i], out next))
                    {
                        next = new FinalNode(ns[i]);
                        data.Add(next.Name, next);
                    }
                    next.Nodes.Add(node);
                    node.Nodes.Add(next);
                }
            }
        }

        public override string PartOne()
        {
            Int64 result = 0;
            int IterationCount = data.Count * 40;
            FullWater(IterationCount);
            Int64 countMin = 0;
            Int64 countMax = 0;
            double limit = IterationCount * 3 / (double)data.Count;
            foreach (var n in data)
            {
                if (n.Value.Value > limit)
                    ++countMax;
                else
                    ++countMin;
            }
            result = countMin * countMax;
            /*            var ie = data.GetEnumerator();
                        ie.MoveNext();
                        var start = ie.Current;
                        result = 1;
                        while (ie.MoveNext())
                            if (GetStreamValue(start.Value, ie.Current.Value) == 3)
                                result++;*/
            return result.ToString();
        }

        void FullWater(int IterationCount)
        {
            var ie = data.GetEnumerator();
            ie.MoveNext();
            var start = ie.Current.Value;
            start.Value = int.MaxValue;
                double Value = 0;
            for (int i = 0; i < IterationCount; ++i)
            {
                Queue<FinalNode> q = new();
                HashSet<FinalNode> p = new();
                Dictionary<string, double> UsedPipe = new();
                q.Enqueue(start);
                FinalNode end = null;
                while (q.Count > 0)
                {
                    end = q.Dequeue();
                    end.Stream(q,p, UsedPipe);
                }
         //       end.Value = 0;
                Value = 0;
                foreach (var n in data)
                {
                    if (n.Value != start)
                    {
                        Value += n.Value.Value;
                    }
                }
                Value /= i + 1;
            }
        }

        int GetStreamValue(FinalNode source, FinalNode destination)
        {

            return 0;
        }

        int GetValue(FinalNode node)
        {
            HashSet<FinalNode> nodes = new HashSet<FinalNode>();
            Queue<FinalNode> queue = new Queue<FinalNode>();
            nodes.Add(node);
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach(var n in current.Nodes)
                    if (nodes.Add(n))
                        queue.Enqueue(n);
            }
            return nodes.Count;
        }

        int GetExternalLinks(HashSet<FinalNode> nodes)
        {
            int result = 0;
            foreach (var node in nodes)
                foreach(var next in node.Nodes)
                    if (!nodes.Contains(next))
                        ++result;
            return result;
        }

        public override string PartTwo()
        {
            /* 
             */
            Int64 result = 0;
            return result.ToString();
        }
    }
}