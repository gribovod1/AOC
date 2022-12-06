using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public class Day13
    {
        public void Exec()
        {
            var a = Inversion(11, 26);
             a = Inversion(21, 5);
             a = Inversion(15, 7);
            if (a != 1)
                throw new Exception();

                var numbersText = File.ReadAllLines(@"Data\day13.txt");
            var time = ulong.Parse(numbersText[0]);
            var bus = new List<ulong>();
            foreach (var s in numbersText[1].Split(','))
                if (s == "x")
                    bus.Add(0);
                else
                    bus.Add(ulong.Parse(s));
            var one = partOne(time, bus.ToArray());

            bus = new List<ulong>();
            foreach (var s in numbersText[1].Split(','))
                if (ulong.TryParse(s, out ulong o))
                    bus.Add(o);
                else bus.Add(0);
            var two = PartTwo(bus.ToArray());

            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        public ulong partOne(ulong time, ulong[] buss)
        {
            ulong result = 0;
            var currTime = time;
            while (true)
            {
                foreach (var b in buss)
                    if (b != 0 && currTime % b == 0)
                    {
                        result = b * (currTime - time);
                        return result;
                    }
                currTime++;
            }
        }

        public ulong PartTwo(ulong[] buss)
        {
            ulong M = 1;
            foreach (var b in buss)
                M *= b == 0 ? 1 : b;
            ulong result = 0;
            for (ulong index = 1; index < (ulong)buss.Length; ++index)
                if (buss[index] != 0)
                    result += (buss[index] - index) * (M / buss[index]) * (ulong)Inversion((long)(M / buss[index]), (long)buss[index]);
            return result % M;
        }

        long Inversion(long b, long m)
        {
            long r1 = m;
            long r2 = b;
            long t1 = 0;
            long t2 = 1;
            while (r2 > 0)
            {
                var q = r1 / r2;
                var r = r1 - q * r2;
                r1 = r2;
                r2 = r;
                var t = t1 - q * t2;
                t1 = t2;
                t2 = t;
            }
            if (r1 == 1)
                return mod(t1, m);
                return t1 % m;
            throw new Exception();
        }

        long mod(long n, long d)
        {
            long result = n % d;
            if (Math.Sign(result) * Math.Sign(d) < 0)
                result += d;
            return result;
        }

    }
}