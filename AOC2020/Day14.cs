using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public class Day14
    {
        public struct Progr
        {
            public string mask;
            public List<KeyValuePair<ulong, ulong>> mems;
        }

        public void Exec()
        {
            var numbersText = File.ReadAllLines(@"Data\day14.txt");
            var programs = new List<Progr>();
            foreach (var line in numbersText)
            {
                if (line.IndexOf("mask") == 0)
                {
                    var p = new Progr();
                    p.mems = new List<KeyValuePair<ulong, ulong>>();
                    var s = line.Split(' ');
                    p.mask = s[2];
                    programs.Add(p);
                }
                else
                {
                    var s = line.Split(new char[] { ' ', '=', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    programs[programs.Count - 1].mems.Add(new KeyValuePair<ulong, ulong>(ulong.Parse(s[1]), ulong.Parse(s[2])));
                }
            }
            var one = partOne(programs);
            var two = partTwo(programs);
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public ulong partOne(List<Progr> programs)
        {
            ulong summ = 0;
            var values = new Dictionary<ulong, ulong>();
            foreach (var p in programs)
                for (int w = 0; w < p.mems.Count; ++w)
                {
                    ulong curr = p.mems[w].Value;
                    for (var m = 0; m < p.mask.Length; ++m)
                        if (p.mask[m] == '1')
                            curr |= (ulong)1 << (p.mask.Length - m - 1);
                        else if (p.mask[m] == '0')
                            curr &= ~((ulong)1 << (p.mask.Length - m - 1));
                    values[p.mems[w].Key] = curr;
                }

            foreach (var v in values.Values)
                summ += v;
            return summ;
        }

        public ulong partTwo(List<Progr> programs)
        {
            ulong summ = 0;
            var values = new Dictionary<ulong, ulong>();
            foreach (var p in programs)
                for (int w = 0; w < p.mems.Count; ++w)
                {
                    var addrList = new List<ulong>();
                    addrs(p, 0, addrList, p.mems[w].Key);
                    for (var i = 0; i < addrList.Count; ++i)
                        values[addrList[i]] = p.mems[w].Value;
                }
            foreach (var v in values.Values)
                summ += v;
            return summ;
        }

        void addrs(Progr program, int maskIndex, List<ulong> store, ulong addr)
        {
            if (maskIndex >= program.mask.Length)
                store.Add(addr);
            else
            {
                if (program.mask[maskIndex] == '0')
                    addrs(program, maskIndex + 1, store, addr);
                else if (program.mask[maskIndex] == '1')
                    addrs(program, maskIndex + 1, store, addr | ((ulong)1 << (program.mask.Length - maskIndex - 1)));
                else
                {
                    addrs(program, maskIndex + 1, store, addr & ~((ulong)1 << (program.mask.Length - maskIndex - 1)));
                    addrs(program, maskIndex + 1, store, addr | ((ulong)1 << (program.mask.Length - maskIndex - 1)));
                }
            }
        }
    }
}