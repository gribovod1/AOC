﻿using AnyThings;
using System.Text;

namespace AOC2024
{
    interface Element
    {
        string Name { get; }

        int? Value { get; }
        abstract bool Process();

        abstract bool ProcessAll();

        bool isZ()
        {
            return (this is Wire) && Name[0] == 'z';
        }

        int GetIndex()
        {
            int index = 0;
            while (char.IsLetter(Name[index])) ++index;
            return int.Parse(Name.Substring(index));
        }
    }

    class Wire : Element
    {
        public string Name { get; set; }

        public int? Value { get; set; }

        public Element? InputElement;

        public bool Process()
        {
            if (Value != null) return true;
            if (InputElement == null || InputElement.Value == null) return false;
            Value = InputElement.Value;
            return true;
        }

        public bool ProcessAll()
        {
            if (Process()) return true;
            if (InputElement != null) InputElement.ProcessAll();
            return Process();
        }

        public Wire(string name, int? value = null)
        {
            Name = name;
            Value = value;
        }
    }

    class GateAND : Element
    {
        public string Name { get; set; }

        public int? Value { get; set; }

        Element source1;
        Element source2;

        public bool Process()
        {
            if (Value != null) return true;
            if (source1.Value == null || source2.Value == null) return false;
            Value = source1.Value.Value & source2.Value.Value;
            return true;
        }

        public bool ProcessAll()
        {
            if (Process()) return true;
            source1.ProcessAll();
            source2.ProcessAll();
            return Process();
        }

        public GateAND(string name, Element source1, Element source2, int? value = null)
        {
            this.Name = name;
            this.source1 = source1;
            this.source2 = source2;
            this.Value = value;
        }
    }

    class GateOR : Element
    {
        public string Name { get; set; }

        public int? Value { get; set; }

        Element source1;
        Element source2;

        public bool Process()
        {
            if (Value != null) return true;
            if (source1.Value == null || source2.Value == null) return false;
            Value = source1.Value.Value | source2.Value.Value;
            return true;
        }

        public bool ProcessAll()
        {
            if (Process()) return true;
            source1.ProcessAll();
            source2.ProcessAll();
            return Process();
        }

        public GateOR(string name, Element source1, Element source2, int? value = null)
        {
            this.Name = name;
            this.source1 = source1;
            this.source2 = source2;
            this.Value = value;
        }
    }

    class GateXOR : Element
    {
        public string Name { get; set; }

        public int? Value { get; set; }

        Element source1;
        Element source2;

        public bool Process()
        {
            if (Value != null) return true;
            if (source1.Value == null || source2.Value == null) return false;
            Value = source1.Value.Value ^ source2.Value.Value;
            return true;
        }

        public bool ProcessAll()
        {
            if (Process()) return true;
            source1.ProcessAll();
            source2.ProcessAll();
            return Process();
        }

        public GateXOR(string name, Element source1, Element source2, int? value = null)
        {
            this.Name = name;
            this.source1 = source1;
            this.source2 = source2;
            this.Value = value;
        }
    }

    internal class Day24 : DayPattern<Dictionary<string, Element>>
    {
        public override void Parse(string singleText)
        {
            data = new();
            var WiresAndGates = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var InputWires = WiresAndGates[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var Gates = WiresAndGates[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var w in InputWires)
            {
                var nameAndValue = w.Split(": ");
                data.Add(nameAndValue[0], new Wire(nameAndValue[0], int.Parse(nameAndValue[1])));
            }

            foreach (var g in Gates)
            {
                var description = g.Split(new string[] { " ", "->" }, StringSplitOptions.RemoveEmptyEntries);
                if (!data.ContainsKey(description[0])) data.Add(description[0], new Wire(description[0]));
                if (!data.ContainsKey(description[2])) data.Add(description[2], new Wire(description[2]));
                if (!data.ContainsKey(description[3])) data.Add(description[3], new Wire(description[3]));
                Element element1 = data[description[0]];
                Element element2 = data[description[2]];
                Element element3 = data[description[3]];
                Element gate = description[1] switch
                {
                    "AND" => new GateAND(g, element1, element2),
                    "OR" => new GateOR(g, element1, element2),
                    "XOR" => new GateXOR(g, element1, element2),
                    _ => new Wire(g),
                };
                data.Add(g, gate);
                ((Wire)element3).InputElement = gate;
            }
        }

        public override string PartOne()
        {
            long resultInt = 0;
            var list = data.Values.ToList();
            Queue<Element> q = new Queue<Element>(list);
            while (q.Count > 0)
            {
                Element element = q.Dequeue();
                if (element.isZ())
                {
                    if (element.ProcessAll())
                    {
                        int index = element.GetIndex();
                        resultInt |= ((long)element.Value.Value << index);
                    }
                }
                /*  if (!element.Process())
                  { q.Enqueue(element); }
                  else
                  {
                      if (element.isZ())
                      {
                          int index = element.GetIndex();
                          result |= (element.Value.Value << index);
                      }
                  }*/
            }
            return resultInt.ToString();

        }

        public override string PartTwo()
        {
            long result = 0;

            return result.ToString();
        }
    }
}