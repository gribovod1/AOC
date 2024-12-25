using AnyThings;

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
            return new List<Element>() { source1,source2 };
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

        long GetDifference(long x, long y)
        {
            var trueValue = x + y;
            foreach (Element element in data.Values)
            {
                if (element.isX()) { element.Value = (int)((x >> element.GetIndex()) & 1); }
                else if (element.isY()) { element.Value = (int)((y >> element.GetIndex()) & 1); }
                else { element.Clear(); }
            }
            long falseValue = 0;
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
                        falseValue |= ((long)element.Value.Value << index);
                    }
                }
            }
            return trueValue ^ falseValue;
        }

        public override string PartTwo()
        {
            long result = 0;
            /*
             1. Запустить пару известных чисел, меняя по одному биту.
            2. Вычисляем правильный результат (z провода)
            3. Получаем, какие биты были выставлены не верно
            4. По проводам, управляющим данными битами, вычисляем ВСЕ родительские провода
            5. Вычисляем пересечение множеств полученных проводов и ранее полученного множества
            6. После проверки каждого входного бита x с каждым входным битом y, получим провода, выставленные не верно
            7. Если таких проводов 8 - то ответ найден
             */
            HashSet<Element> suspects = new();
            long wrong_bits = 0;
            foreach (Element element in data.Values)
                if (element is Wire) suspects.Add(element);
            for (int x = 0; x < 45; ++x)
                for (int y = 0; y < 45; ++y)
                {
                    var diff = GetDifference(((long)1 << x), ((long)1 << y));
                    wrong_bits |= diff;
                    //for(int i =0; i < 45;++i)
                    //HashSet<Element> GetParent(data[]);
                }

            for (int w = 0; w <= 45; ++w)
          //      if ((int)((wrong_bits >> w) & 1) == 1)
                {
                    var n = "z" + w.ToString("00");
                    var e = data[n];
                    List<Element> parents = e.GetParents();
                }


            return result.ToString();
        }
    }
}