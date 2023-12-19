using System.Diagnostics.Metrics;
using System.Text;
using AnyThings;

namespace AOC2023
{
    internal class Day19 : DayPattern<(Dictionary<string, string> ins, List<Dictionary<char, int>> prop)>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = (new(), new());
            string[] ins = text[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            string[] prop = text[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < ins.Length; ++i)
            {
                string[] val = ins[i].Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                data.ins.Add(val[0], val[1]);
            }
            for (int p = 0; p < prop.Length; ++p)
            {
                string[] val = prop[p].Split(new char[] { ',', '{', '}', 'x', 'a', 'm', 's', '=' }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<char, int> dp = new();
                dp.Add('x', int.Parse(val[0]));
                dp.Add('m', int.Parse(val[1]));
                dp.Add('a', int.Parse(val[2]));
                dp.Add('s', int.Parse(val[3]));
                data.prop.Add(dp);
            }
        }

        public override string PartOne()
        {
            Int64 result = 0;
            for (int i = 0; i < data.prop.Count; ++i)
                result += Eval(data.prop[i]);
            return result.ToString();
        }

        public override string PartTwo()
        {
            /* Представляет инструкции как деление на ветки дерева - каждая инструкция делит весь набор комбинаций на две половины.
             * Когда встречает A - возвращает число комбинаций параметров. Когда встречает R - возвращает 0.
             */
            Dictionary<char, (int min, int max)> Range = new()
            {
                { 'x', (1,4000) },
                { 'm', (1, 4000) },
                { 'a', (1, 4000) },
                { 's', (1, 4000) }
            };
            return TreeRanges(Range, data.ins["in"]).ToString();
        }

        private long Eval(Dictionary<char, int> d)
        {
            int result = 0;
            string current = "in";
            do
            {
                current = Move(d, current);
            } while (current != "A" && current != "R");
            if (current == "A")
                foreach (var p in d)
                    result += p.Value;
            return result;
        }

        private string Move(Dictionary<char, int> d, string current)
        {
            string[] e = data.ins[current].Split(',');
            for (int i = 0; i < e.Length - 1; i++)
            {
                string[] dir = e[i].Split(":");
                switch (e[i][1])
                {
                    case '>':
                        {
                            if (d[e[i][0]] > int.Parse(dir[0].Substring(2)))
                                return dir[1];
                            break;
                        }
                    case '<':
                        {
                            if (d[e[i][0]] < int.Parse(dir[0].Substring(2)))
                                return dir[1];
                            break;
                        }
                }
            }
            return e[e.Length - 1];
        }

        Int64 TreeRanges(Dictionary<char, (int min, int max)> Range, string instructions)
        {
            if (instructions == "A")
            {
                Int64 result = 1;
                foreach (var r in Range)
                    if (r.Value.min < r.Value.max)
                        result *= r.Value.max - r.Value.min + 1;
                    else return 0;
                return result;
            }
            if (instructions == "R") return 0;

            int IndexEndOfInstruction = instructions.IndexOf(',');
            if (IndexEndOfInstruction > 0)
            {
                string instruction = instructions.Substring(0, IndexEndOfInstruction);
                string[] dir = instruction.Split(":");
                string target = dir[1];
                int value = int.Parse(dir[0].Substring(2));
                char property = instruction[0];
                switch (instruction[1])
                {
                    case '>':
                        {
                            if (Range[property].max >= value)
                            {
                                Dictionary<char, (int min, int max)> innerRange = new(Range);
                                innerRange[property] = (value + 1, innerRange[property].max);
                                Dictionary<char, (int min, int max)> outerRange = new(Range);
                                outerRange[property] = (outerRange[property].min, value);
                                return TreeRanges(innerRange, target) + TreeRanges(outerRange, instructions.Substring(IndexEndOfInstruction + 1));
                            }
                            return TreeRanges(Range, instructions.Substring(IndexEndOfInstruction + 1));
                        }
                    case '<':
                        {
                            if (Range[property].min <= value)
                            {
                                Dictionary<char, (int min, int max)> innerRange = new(Range);
                                innerRange[property] = (innerRange[property].min, value - 1);
                                Dictionary<char, (int min, int max)> outerRange = new(Range);
                                outerRange[property] = (value, outerRange[property].max);
                                return TreeRanges(innerRange, target) + TreeRanges(outerRange, instructions.Substring(IndexEndOfInstruction + 1));
                            }
                            return TreeRanges(Range, instructions.Substring(IndexEndOfInstruction + 1));
                        }
                }
                return 0;
            }
            else
                return TreeRanges(Range, data.ins[instructions]);
        }
    }
}