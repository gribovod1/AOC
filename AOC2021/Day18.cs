using AnyThings;
using DataStruct;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2021
{
    internal class SnailPair : BinaryTree<long>, ICloneable
    {
        public static SnailPair Parse(string s)
        {
            int i = 0;
            return Parse(s, ref i);
        }

        public static SnailPair Parse(string s, ref int position)
        {
            SnailPair result = new SnailPair();
            if (s[position] == '[')
            {
                ++position;
                result.Left = Parse(s, ref position);
                ++position;
                result.Right = Parse(s, ref position);
                ++position;
            }
            else
            {
                var dLength = 0;
                while (char.IsDigit(s[position + dLength]))
                    ++dLength;
                result.Value = long.Parse(s.Substring(position, dLength));
                position += dLength;
            }
            return result;
        }

        public long SnailValue
        {
            get
            {
                if (IsLeaf)
                    return Value;
                else
                    return (Left as SnailPair).SnailValue * 3 + (Right as SnailPair).SnailValue * 2;
            }
        }

        public static SnailPair operator +(SnailPair a, SnailPair b)
        {
            var result = new SnailPair(a.Clone() as SnailPair, b.Clone() as SnailPair);
            while (result.Explode() || result.Split()) ;
            return result;
        }

        public bool Explode()
        {
            if (IsLeaf)
                return false;
            if ((Left as SnailPair).Explode())
                return true;
            if ((Right as SnailPair).Explode())
                return true;
            if (Level > 4)
            {
                var lSide = GetNextLeaf(x => x.IsLeaf, BinaryTree<long>.Direction.ToLeft);
                if (lSide != null)
                    lSide.Value += Left.Value;
                var rSide = GetNextLeaf(x => x.IsLeaf, BinaryTree<long>.Direction.ToRight);
                if (rSide != null)
                    rSide.Value += Right.Value;
                Left = null;
                Right = null;
                Value = 0;
                return true;
            }
            return false;
        }

        public bool Split()
        {
            if (!IsLeaf)
            {
                if ((Left as SnailPair).Split())
                    return true;
                if ((Right as SnailPair).Split())
                    return true;
                return false;
            }
            if (Value >= 10)
            {
                Left = new SnailPair(Value / 2);
                Right = new SnailPair(Value / 2 + (Value % 2));
                Value = 0;
                return true;
            }
            return false;
        }

        public SnailPair()
        {
        }

        public SnailPair(long value) : base(value)
        {
        }

        public SnailPair(SnailPair left, SnailPair right) : base(left, right)
        {
        }

        public override string ToString()
        {
            return IsLeaf ? Value.ToString() : $"[{Left},{Right}]";
        }

        public object Clone()
        {
            var result = new SnailPair(Value);
            result.Parent = Parent;
            result.Left = Left != null ? (Left as SnailPair).Clone() as SnailPair : null;
            result.Right = Right != null ? (Right as SnailPair).Clone() as SnailPair : null;
            return result;
        }
    }

    class Day18 : DayPattern<List<SnailPair>>
    {
        public override void ParseFile(string path)
        {
            var lines = File.ReadAllLines(path);
            data = new List<SnailPair>();
            for (var s = 0; s < lines.Length; ++s)
                data.Add(SnailPair.Parse(lines[s]));
        }

        public override string PartOne()
        {
            var result = data[0];
            for (var s = 1; s < data.Count; ++s)
                result += data[s];
            return result.SnailValue.ToString();
        }

        public override string PartTwo()
        {
            long max = 0;
            for (var i = 0; i < data.Count; ++i)
                for (var j = 0; j < data.Count; ++j)
                    if (i != j)
                    {
                        var value = (data[i] + data[j]).SnailValue;
                        if (value > max)
                            max = value;
                    }
            return max.ToString();
        }
    }
}