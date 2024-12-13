using AnyThings;
using Coord = (long x, long y);

namespace AOC2024
{
    internal class Day13 : DayPattern<List<(Coord A, Coord B, Coord P)>>
    {
        public override void Parse(string singleText)
        {
            string[] machines = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (int l = 0; l < machines.Length; l++)
            {
                var pText = machines[l].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                var Atext = pText[0].Split(new char[] { ' ', ',', '+' }, StringSplitOptions.RemoveEmptyEntries);
                var Btext = pText[1].Split(new char[] { ' ', ',', '+' }, StringSplitOptions.RemoveEmptyEntries);
                var Ptext = pText[2].Split(new char[] { ' ', ',', '=' }, StringSplitOptions.RemoveEmptyEntries);
                data.Add(((long.Parse(Atext[3]), long.Parse(Atext[5])),
                    (long.Parse(Btext[3]), long.Parse(Btext[5])),
                    (long.Parse(Ptext[2]), long.Parse(Ptext[4]))));
            }
        }

        public override string PartOne()
        {
            long result = 0;
            for (int l = 0; l < data.Count; ++l)
                result += Calc(data[l]);
            return result.ToString();
        }

        public override string PartTwo()
        {

            long result = 0;
            for (int l = 0; l < data.Count; ++l)
            {
                var m = data[l];
                m.P.x += 10000000000000;
                m.P.y += 10000000000000;
                result += Calc(m);
            }
            return result.ToString();
        }

        long Calc((Coord A, Coord B, Coord P) m)
        {
            if ((m.P.y * m.A.x - m.A.y * m.P.x) % (m.A.x * m.B.y - m.A.y * m.B.x) != 0) return 0;
            var b = (m.P.y * m.A.x - m.A.y * m.P.x) / (m.A.x * m.B.y - m.A.y * m.B.x);
            if ((m.P.x - m.B.x * b) % m.A.x != 0) return 0;
            var a = (m.P.x - m.B.x * b) / m.A.x;
            return b + a * 3;
        }
    }
}