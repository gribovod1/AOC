using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public class Day24
    {
        public void Exec()
        {
            var text = File.ReadAllLines(@"Data\day24.txt");
            var hs = PartOne(text);
            var one = hs.Count;
            var two = PartTwo(hs).Count;
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        HashSet<KeyValuePair<int, int>> PartOne(string[] text)
        {
            var rotated = new HashSet<KeyValuePair<int, int>>();
            for (var index = 0; index < text.Length; ++index)
            {
                var s = text[index];
                var x = 0;
                var y = 0;
                while (s.Length > 0)
                {
                    string dir = s[0] == 's' || s[0] == 'n' ? s.Substring(0, 2) : s.Substring(0, 1);
                    s = s.Substring(dir.Length, s.Length - dir.Length);
                    if (dir[0] == 's')
                        ++y;
                    if (dir[0] == 'n')
                        --y;
                    if (dir == "sw" || dir == "w")
                        ++x;
                    if (dir == "ne" || dir == "e")
                        --x;
                }
                var coord = new KeyValuePair<int, int>(x, y);
                if (rotated.Contains(coord))
                    rotated.Remove(coord);
                else
                    rotated.Add(coord);
            }

            return rotated;
        }

        HashSet<KeyValuePair<int, int>> PartTwo(HashSet<KeyValuePair<int, int>> blacks, int count = 0)
        {
            if (count == 100)
                return blacks;
            var result = new HashSet<KeyValuePair<int, int>>();
            var e = blacks.GetEnumerator();
            while (e.MoveNext())
            {
                var nearBlackCount = NearBlackCount(blacks, e.Current);
                if (nearBlackCount == 1 || nearBlackCount == 2)
                    result.Add(e.Current);
                foreach (var n in Neighbors(blacks, e.Current))
                    if (NeedAdded(blacks, n))
                        result.Add(n);
            }
            return PartTwo(result, count + 1);
        }

        List<KeyValuePair<int, int>> Neighbors(HashSet<KeyValuePair<int, int>> blacks, KeyValuePair<int, int> coord)
        {
            return new List<KeyValuePair<int, int>>()
            {
                new KeyValuePair<int, int>(coord.Key + 1, coord.Value + 1),
                new KeyValuePair<int, int>(coord.Key + 1, coord.Value),
                new KeyValuePair<int, int>(coord.Key, coord.Value - 1),
                new KeyValuePair<int, int>(coord.Key - 1, coord.Value - 1),
                new KeyValuePair<int, int>(coord.Key - 1, coord.Value),
                new KeyValuePair<int, int>(coord.Key, coord.Value + 1)
            };
        }

        bool NeedAdded(HashSet<KeyValuePair<int, int>> blacks, KeyValuePair<int, int> coord)
        {
            if (blacks.Contains(coord))
                return false;
            return NearBlackCount(blacks, coord) == 2;
        }

        int NearBlackCount(HashSet<KeyValuePair<int, int>> blacks, KeyValuePair<int, int> coord)
        {
            var result = 0;
            foreach (var n in Neighbors(blacks, coord))
                if (blacks.Contains(n))
                    ++result;
            return result;
        }
    }
}