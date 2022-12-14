using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AnyThings;

using DayType = System.Collections.Generic.List<string>;

namespace AOC2021
{
    class Day10 : DayPattern<DayType>
    {
        public override void ParseFile(string path)
        {
            data = new List<string>(File.ReadAllLines(path));
            foreach (var s in data)
                calc(s);
        }

        int one = 0;
        public override string PartOne()
        {
            return one.ToString();
        }

        List<UInt64> two = new List<UInt64>();
        public override string PartTwo()
        {
            two.Sort();
            return two[two.Count / 2].ToString();
        }

        void calc(string data)
        {
            var st = new Stack<char>();
            var cIndex = 0;
            for (cIndex = 0; cIndex < data.Length; ++cIndex)
            {
                if (isOpen(data[cIndex]))
                    st.Push(data[cIndex]);
                else
                {
                    var openC = st.Pop();
                    if (!isPair(openC, data[cIndex]))
                    {
                        one += onePartCoast(data[cIndex]);
                        return;
                    }
                }
            }
            UInt64 result = 0;
            while (st.Count > 0)
                result = result * 5 + twoPartCoast(st.Pop());
            two.Add(result);
        }

        private int onePartCoast(char v)
        {
            switch (v)
            {
                case ')':
                    return 3;
                case ']':
                    return 57;
                case '}':
                    return 1197;
                case '>':
                    return 25137;
            }
            return 0;
        }

        private UInt64 twoPartCoast(char v)
        {
            switch (v)
            {
                case '(':
                    return 1;
                case '[':
                    return 2;
                case '{':
                    return 3;
                case '<':
                    return 4;
            }
            return 0;
        }

        bool isOpen(char c)
        {
            return c == '(' || c == '{' || c == '<' || c == '[';
        }

        bool isPair(char c1, char c2)
        {
            switch (c1)
            {
                case '(':
                    return c2 == ')';
                case '{':
                    return c2 == '}';
                case '<':
                    return c2 == '>';
                case '[':
                    return c2 == ']';
            }
            return false;
        }
    }
}