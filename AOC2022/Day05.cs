using AnyThings;

namespace AOC2022
{
    class move
    {
        public int Count;
        public int From;
        public int To;

        public move(int count, int from, int to)
        {
            Count = count;
            From = from;
            To = to;
        }

        public move(string source)
        {
            var ss = source.Split(' ');
            Count = int.Parse(ss[1]);
            From = int.Parse(ss[3]) - 1;
            To = int.Parse(ss[5]) - 1;
        }
    }

    internal class Day05 : DayPattern<List<move>>
    {
        List<Stack<char>> cargo = new List<Stack<char>>();
        public override void Parse(string path)
        {
            var ss = File.ReadAllText(path).Split(Environment.NewLine + Environment.NewLine);
            var cs = ss[0].Split(Environment.NewLine);
            var StackNumbers = cs[cs.Length - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var StackCount = int.Parse(StackNumbers[StackNumbers.Length - 1]);
            for (var s = 0; s < StackCount; s++)
            {
                var curr = new Stack<char>();
                cargo.Add(curr);
                for (var l = cs.Length - 2; l >= 0; --l)
                {
                    var box = cs[l][s * 4 + 1];
                    if (box > 'A')
                        curr.Push(box);
                    else break;
                }
            }

            data = new List<move>();
            var ms = ss[1].Split(Environment.NewLine);
            foreach (var s in ms)
                data.Add(new move(s));
        }

        public override string PartOne()
        {
            string result = string.Empty;
            /*     foreach (var m in data)
                 {
                     for(var i = 0; i < m.Count; ++i)
                     {
                         cargo[m.To].Push(cargo[m.From].Pop());
                     }
                 }
                 foreach(var s in cargo)
                 {
                     result += s.Peek();
                 }*/

            return result;
        }

        public override string PartTwo()
        {
            string result = string.Empty;
            foreach (var m in data)
            {
                var t = new Stack<char>();
                for (var i = 0; i < m.Count; ++i)
                {
                    t.Push(cargo[m.From].Pop());
                }
                for (var i = 0; i < m.Count; ++i)
                    cargo[m.To].Push(t.Pop());
            }
            foreach (var s in cargo)
            {
                result += s.Peek();
            }

            return result;
        }
    }
}