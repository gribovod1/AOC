using AnyThings;
using System.Text;

namespace AOC2024
{
    abstract class Element
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public int? Value { get; set; }

        public abstract bool Process();

        public abstract bool ProcessAll();

        public bool isZ()
        {
            return (this is Wire) && Name[0] == 'z';
        }

        public bool isX()
        {
            return (this is Wire) && Name[0] == 'x';
        }

        public bool isY()
        {
            return (this is Wire) && Name[0] == 'y';
        }

        public int GetIndex()
        {
            int index = 0;
            while (char.IsLetter(Name[index])) ++index;
            return int.Parse(Name.Substring(index));
        }

        public void Clear()
        {
            Value = null;
        }

        public abstract List<Element> GetInputs();

        public List<Element> GetParents(List<Element>? prev = null)
        {
            var result = prev ?? new List<Element>();
            var inputs = GetInputs();
            foreach (var input in inputs)
                if (input != null)
                {
                    result.Add(input);
                    input.GetParents(result);
                }
            return result;
        }

        public HashSet<Element> GetWires(bool ExcludeINputOutput, HashSet<Element>? prev = null)
        {
            var result = prev ?? new HashSet<Element>();
            var inputs = GetInputs();
            foreach (var input in inputs)
                if (input != null)
                {
                    if (input is Wire && !(ExcludeINputOutput && (input.isX() || input.isY() || input.isZ())))
                        result.Add(input);
                    input.GetWires(ExcludeINputOutput, result);
                }
            return result;
        }

        public bool CycleTest(Element node = null)
        {
            var e = node ?? this;
            var inputs = GetInputs();
            foreach (var input in inputs)
                if (input == null)
                    continue;
                else if (input == e || !input.CycleTest(e))
                    return false;
            return true;
        }
    }

    class Wire : Element
    {
        public Element? InputElement;

        public override bool Process()
        {
            if (Value != null) return true;
            if (InputElement == null || InputElement.Value == null) return false;
            Value = InputElement.Value;
            return true;
        }

        public override bool ProcessAll()
        {
            if (Process()) return true;
            if (InputElement != null) InputElement.ProcessAll();
            return Process();
        }

        public override List<Element> GetInputs()
        {
            return new List<Element>() { InputElement };
        }

        public Wire(string name, int? value = null)
        {
            Name = name;
            Value = value;
        }
    }

    class GateAND : Element
    {
        Element source1;
        Element source2;

        public override bool Process()
        {
            if (Value != null) return true;
            if (source1.Value == null || source2.Value == null) return false;
            Value = source1.Value.Value & source2.Value.Value;
            return true;
        }

        public override bool ProcessAll()
        {
            if (Process()) return true;
            source1.ProcessAll();
            source2.ProcessAll();
            return Process();
        }

        public override List<Element> GetInputs()
        {
            return new List<Element>() { source1, source2 };
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
        Element source1;
        Element source2;

        public override bool Process()
        {
            if (Value != null) return true;
            if (source1.Value == null || source2.Value == null) return false;
            Value = source1.Value.Value | source2.Value.Value;
            return true;
        }

        public override bool ProcessAll()
        {
            if (Process()) return true;
            source1.ProcessAll();
            source2.ProcessAll();
            return Process();
        }

        public override List<Element> GetInputs()
        {
            return new List<Element>() { source1, source2 };
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
        Element source1;
        Element source2;

        public override bool Process()
        {
            if (Value != null) return true;
            if (source1.Value == null || source2.Value == null) return false;
            Value = source1.Value.Value ^ source2.Value.Value;
            return true;
        }

        public override bool ProcessAll()
        {
            if (Process()) return true;
            source1.ProcessAll();
            source2.ProcessAll();
            return Process();
        }

        public override List<Element> GetInputs()
        {
            return new List<Element>() { source1, source2 };
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
                    "AND" => new GateAND("Gate " + g, element1, element2),
                    "OR" => new GateOR("Gate " + g, element1, element2),
                    "XOR" => new GateXOR("Gate " + g, element1, element2),
                    _ => new Wire("Gate " + g),
                };
                data.Add(g, gate);
                ((Wire)element3).InputElement = gate;
            }

            long xValue = 0;
            long yValue = 0;
            var list = data.Values.ToList();
            foreach (var g in list)
            {
                if (g.Name[0] == 'x' || g.Name[0] == 'y')
                {
                    var index = g.GetIndex();
                    if (g.Name[0] == 'x')
                    {
                        xValue |= ((long)g.Value.Value << index);
                    }
                    else
                    {
                        yValue |= ((long)g.Value.Value << index);
                    }
                }
            }
            Console.WriteLine($"x: {xValue} y: {yValue}");
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
            }
            return resultInt.ToString("B");

        }

        void Clean(long x, long y)
        {
            foreach (Element element in data.Values)
            {
                if (element.isX()) { element.Value = (int)((x >> element.GetIndex()) & 1); }
                else if (element.isY()) { element.Value = (int)((y >> element.GetIndex()) & 1); }
                else { element.Clear(); }
            }
        }

        long GetZ(long x, long y)
        {
            long result = 0;
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
                        result |= ((long)element.Value.Value << index);
                    }
                }
            }
            return result;
        }

        long GetDifference(long x, long y)
        {
            Clean(x, y);
            return (x + y) ^ GetZ(x, y);
        }

        Element GetWire(char bus, int index)
        {
            return data[bus + index.ToString("00")];
        }

        long Test()
        {
            long wrong_bits = 0;
            for (int x = 0; x < 45; ++x)
                for (int y = 0; y < 45; ++y)
                {
                    var diff = GetDifference(((long)1 << x), ((long)1 << y));
                    wrong_bits |= diff;
                }
            return wrong_bits;
        }

        HashSet<Element> GetAllSuspects(long wrong_bits)
        {
            HashSet<Element> wrongElements = new();
            for (int w = 0; w <= 45; ++w)
            {
                var n = "z" + w.ToString("00");
                var e = data[n];
                if ((int)((wrong_bits >> w) & 1) != 0)
                {
                    //                   wrongElements.Add(e);
                    var parents = e.GetWires(true);
                    foreach (var p in parents)
                        wrongElements.Add(p);
                }
            }
            return wrongElements;
        }

        HashSet<Element> GetSuspects(int bit_index)
        {
            HashSet<Element> wrongElements = new();
            var n = "z" + bit_index.ToString("00");
            var e = data[n];
            var parents = e.GetWires(true);
            foreach (var p in parents)
                wrongElements.Add(p);
            return wrongElements;
        }

        bool SwapWires(Element wire1, Element wire2)
        {
            var i1 = (wire1 as Wire).InputElement;
            var i2 = (wire2 as Wire).InputElement;
            (wire1 as Wire).InputElement = i2;
            (wire2 as Wire).InputElement = i1;
            if (wire1.CycleTest() && wire2.CycleTest()) 
                return true;
            (wire1 as Wire).InputElement = i1;
            (wire2 as Wire).InputElement = i2;
            return false;
        }

        int OneCount(long number)
        {
            int result = 0;
            while (number > 0)
            {
                if (number % 1 != 0) ++result;
                number >>= 1;
            }
            return result;
        }

        (bool modify, int wire1, int wire2) FindSwap(List<Element> wrongElements, long wrong_bits)
        {
            for (int w1 = 0; w1 < wrongElements.Count; ++w1)
                for (int w2 = w1 + 1; w2 < wrongElements.Count; ++w2)
                    if (SwapWires(wrongElements[w1], wrongElements[w2]))
                    {
                        long wrong_bits2 = Test();
                        if (OneCount(wrong_bits2) < OneCount(wrong_bits))
                            return (true, w1, w2);
                        else
                        {
                            SwapWires(wrongElements[w1], wrongElements[w2]);
                        }
                    }
            return (false, -1, -1);
        }

        public override string PartTwo()
        {
            long wrong_bits = Test();
            int bit = 0;
            HashSet<Element> swap = new();
            while (bit < 45)
            {
                if (((wrong_bits >> bit) & 1) != 0)
                {
                    List<Element> wrongElements = GetSuspects(bit).ToList();
                    while (wrongElements.Count > 0)
                    {
                        (bool modify, int w1, int w2) = FindSwap(wrongElements, wrong_bits);
                        if (modify)
                        {
                            swap.Add(wrongElements[w1]);
                            swap.Add(wrongElements[w2]);
                            wrongElements = GetSuspects(bit).ToList();
                        }
                    }
                }
                ++bit;
            }
            wrong_bits = Test();
            if (wrong_bits != 0) Console.WriteLine($"Это ещё не всё! {wrong_bits.ToString("00")}");
            var swaped = swap.ToList();
            swaped.Sort();
            StringBuilder result = new();
            foreach (Element element in swaped)
                result.Append($"{element.Name},");
            return result.ToString(1, result.Length - 1);



            /*
Заполняем подряд биты для обоих чисел начиная с младших,
как только результат будет отличаться от ожидаемого - то значит в новых задействованных проводниках ошибка.
Необходимо фиксировать, какие провода были задействованы.             
*/

            for (int bit_index = 0; bit_index < 45; ++bit_index)
            {
                long x = (long)1 << bit_index;
                long y = (long)1 << bit_index;
                Clean(x, y);
                long z = GetZ(x, y);
                if (z != x + y)
                {
                    var zWrong = GetWire('z', bit_index + 1);
                    var p = zWrong.GetParents();
                    var ws = zWrong.GetWires(false).ToList();
                    Console.WriteLine($"Wrong index: {bit_index}");
                    for (int w = ws.Count - 1; w >= 0; --w)
                        if (!ws[w].isX() && !ws[w].isY())
                        {
                            Clean(x, y);
                            ws[w].Value = 1;
                            long zT1 = GetZ(x, y);
                            if (zT1 == x + y)
                            {
                                Console.WriteLine($"Wrong wire: {ws[w]}");
                            }/*
                            Clean(x, y);
                            ws[w].Value = 1;
                            zT1 = GetZ(x, y);
                            if (zT1 == x + y)
                            {
                                Console.WriteLine($"Wrong wire: {ws[w]}");
                            }*/
                        }
                }
            }




            return result.ToString();
        }
    }
}