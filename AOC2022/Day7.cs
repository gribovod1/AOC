using AnyThings;

namespace AOC2022
{
        class Dir
        {
            public int size;
            public Dir root;
            public string name;
            public Dictionary<string, Dir> inner = new Dictionary<string, Dir>();
            public Dictionary<string, int> files = new Dictionary<string, int>();

            public int getSize()
            {
                var result = size;
                foreach (var d in inner)
                    result += d.Value.getSize();
                return result;
            }

            public void GetAllDirectories(List<Dir> data)
            {
                    data.Add(this);
                foreach (var d in inner)
                    d.Value.GetAllDirectories(data);
            }

            public void Load(List<string> data, ref int line)
            {
                while (line < data.Count)
                {
                   var  ss = data[line].Split(' ');
                    if (ss[0] == "$")
                    {
                        if (ss[1] == "cd")
                        {
                            if (ss[2] == "..")
                                return;
                            ++line;
                            inner[ss[2]].Load(data, ref line);
                        }
                    }
                    else
                    {
                        if (ss[0] == "dir")
                        {
                            var d = new Dir();
                            d.root = this;
                            d.name = ss[1];
                            inner.Add(ss[1], d);
                        }
                        else
                        {
                            var fs = int.Parse(ss[0]);
                            size += fs;
                            files.Add(ss[1], fs);
                        }
                    }
                    ++line;
                }
            }
        }

    internal class Day7 : DayPattern<List<Dir>>
    {
        public override void Parse(string path)
        {
            var text = File.ReadAllText(path).Split(Environment.NewLine).ToList();
            var root = new Dir();
            root.name = "/";
            var l = 1;
            root.Load(text, ref l);
            data = new List<Dir>();
            root.GetAllDirectories(data);
        }

        public override string PartOne()
        {
            int counter = 0;
            foreach (var d in data)
            {
                int s = d.getSize();
                if (s <= 100000)
                    counter += s;
            }
            return counter.ToString();
        }

        public override string PartTwo()
        {
            var needSizeMin = data[0].getSize() - 40000000;
            var minSize = 70000000;
            foreach (var d in data)
            {
                int s = d.getSize();
                if (s >= needSizeMin && s < minSize)
                    minSize = s;
            }
            return minSize.ToString();
        }
    }
}