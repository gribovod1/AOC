using AnyThings;
using System.Xml.Linq;

namespace AOC2023
{
    internal class Day08 : DayPattern<(string Instructions, Dictionary<string, (string Left, string Right)> tree)>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, (string Left, string Right)> tree = new();
            var nodes = text[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nodes.Length; ++i)
            {
                var node = nodes[i].Split(new char[] { ' ', ',', '=', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                tree.Add(node[0], (node[1], node[2]));
            }
            data = new(text[0], tree);
        }

        public override string PartOne()
        {
            /*
             * Поиск в глубину
             */
            int result = 0;
            string node = "AAA";
            while (node != "ZZZ")
            {
                char i = data.Instructions[result % data.Instructions.Length];
                node = i == 'L' ? data.tree[node].Left : data.tree[node].Right;
                ++result;
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            /*
             * Для каждого ответвления находится начало цикла и его конец, после чего, для всех размеров циклов, находится НОК 
             */
            UInt64 result = 0;
            List<UInt64> zPositions = new();
            foreach (var node in data.tree)
                if (node.Key[node.Key.Length - 1] == 'A')
                {
                    var (Start, End) = FindCycleSize(node.Key);
                    zPositions.Add((UInt64)(End - Start));
                }
            result = MathHelper.GetLCM(zPositions);
            return result.ToString();
        }

        (Int64 Start, Int64 End) FindCycleSize(string node)
        {
            Dictionary<(string node, int instruction), Int64> NodeInstructions = new();
            int Instruction = 0;
            Int64 Length = 0;
            while (true)
            {
                Int64 length;
                if (NodeInstructions.TryGetValue((node, Instruction), out length))
                    return (length, Length);
                else
                    NodeInstructions.Add((node, Instruction), Length);
                node = data.Instructions[Instruction] == 'L' ? data.tree[node].Left : data.tree[node].Right;
                ++Length;
                Instruction = (Instruction + 1) % data.Instructions.Length;
            }
        }
    }
}