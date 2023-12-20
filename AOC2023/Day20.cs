using System;
using System.Diagnostics.Metrics;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using AnyThings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AOC2023
{
    internal class Day20 : DayPattern<Dictionary<string, Element>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (var i = 0; i < text.Length; ++i)
            {
                string[] val = text[i].Split(new string[] { " -> ", ", " }, StringSplitOptions.RemoveEmptyEntries);
                char type = val[0][0];
                string name = type != 'b' ? val[0].Substring(1) : val[0];
                List<string> nodes = new List<string>();
                for (int n = 1; n < val.Length; ++n) { nodes.Add(val[n]); }
                switch (type)
                {
                    case '%':
                        {
                            data.Add(name, new Trigger(name, nodes));
                            break;
                        }
                    case '&':
                        {
                            data.Add(name, new Invertor(name, nodes));
                            break;
                        }
                    case 'b':
                        {
                            data.Add(name, new Broadcast(name, nodes));
                            break;
                        }
                }
            }

            Queue<(string name, int index)> sort = new();


            foreach (var s in data)
                foreach (var d in s.Value.nodes)
                    if (data.ContainsKey(d))
                        data[d].InputNode(s.Key);
                    else
                        sort.Enqueue((s.Key, 0));

            while (sort.Count > 0)
            {
                var s = sort.Dequeue();
                if (!pattern.ContainsKey(s.name))
                {
                    pattern.Add(s.name, (s.index, 0));
                    foreach (var n in data[s.name].Input)
                        sort.Enqueue((n.Key, s.index + 1));
                }
            }
        }

        Dictionary<string, (int index, uint value)> pattern = new();

        public override string PartOne()
        {
            Int64 result = 0;
            Int64 HighCounter = 0;
            Int64 LowCounter = 0;
            int i = 0;
            for ( i = 0; i < 1000; ++i)
                listOfResults.Add(Circuit(ref HighCounter, ref LowCounter));
            result = HighCounter * LowCounter;
            return result.ToString();
        }

            List<Int64> listOfResults = new List<Int64>();

        public override string PartTwo()
        {
            /* 
             */
            // Собрать сведения о 
            Int64 result = 0;
            Int64 HighCounter = 0;
            Int64 LowCounter = 0;
            for (var i = 0; i < 10000; ++i)
                listOfResults.Add(Circuit(ref HighCounter, ref LowCounter));

      /*      for (var i = 0; i < listOfResults.Count - 1; ++i)
                listOfResults[i] = listOfResults[i] ^ listOfResults[i + 1];*/
            StringBuilder sb = new();
            for (var i = 0; i < listOfResults.Count - 1; ++i)
            {
                var value = Convert.ToString(listOfResults[i], 2);
                if (value.Length<32)
                    sb.Append('0',32-value.Length);
                sb.AppendLine(value);
            }
                File.WriteAllText($"Data/{GetType().Name}_path.txt", sb.ToString());
                return result.ToString();
        }

        Int64 Circuit(ref Int64 HighCounter, ref Int64 LowCounter)
        {
            Int64 result = 0;
            Queue<(string source, string destination, uint value)> pulses = new();
            pulses.Enqueue(("button", "broadcaster", 0u));
            while (pulses.Count > 0)
            {
                var p = pulses.Dequeue();
                if (p.value == 0)
                    ++LowCounter;
                else
                    ++HighCounter;
                if (data.ContainsKey(p.destination))
                    data[p.destination].ProcessPulse(pulses, p);
                if (pattern.ContainsKey(p.destination) && data.ContainsKey(p.destination))
                    pattern[p.destination] = (pattern[p.destination].index, data[p.destination].Output);
            }
            foreach(var p in pattern)
                result |= (p.Value.value << p.Value.index);
            return result;
        }
    }

    abstract class Element
    {
        public Dictionary<string, uint> Input = new();
        public uint Output = 0;
        public string name;
        public List<string> nodes;
        public void InputNode(string node)
        {
            Input.Add(node, 0);
        }

        internal abstract void ProcessPulse(Queue<(string source, string destination, uint value)> pulses, (string source, string destination, uint value) p);

        public Element(string name, List<string> nodes)
        {
            this.name = name;
            this.nodes = nodes;
        }
    }

    class Trigger : Element
    {
        bool SendPulse = false;
        bool Enabled = false;
        public Trigger(string name, List<string> nodes) : base(name, nodes)
        {
        }

        internal override void ProcessPulse(Queue<(string source, string destination, uint value)> pulses, (string source, string destination, uint value) p)
        {
            if (p.value == 0)
            {
                Enabled = !Enabled;
                Output = Enabled ? 1u : 0u;
                foreach (var n in nodes)
                    pulses.Enqueue((name, n, Output));
            }
        }
    }

    class Broadcast : Element
    {
        public Broadcast(string name, List<string> nodes) : base(name, nodes)
        {
        }

        internal override void ProcessPulse(Queue<(string source, string destination, uint value)> pulses, (string source, string destination, uint value) p)
        {
            foreach (var n in nodes)
                pulses.Enqueue((name, n, 0));
        }
    }

    class Invertor : Element
    {
        public Invertor(string name, List<string> nodes) : base(name, nodes)
        {
        }

        internal override void ProcessPulse(Queue<(string source, string destination, uint value)> pulses, (string source, string destination, uint value) p)
        {
            Input[p.source] = p.value;
            Output = 1;
            foreach (var i in Input)
                Output &= i.Value;
            foreach (var n in nodes)
                pulses.Enqueue((name, n, Output == 1 ? 0u : 1u));
        }
    }
}