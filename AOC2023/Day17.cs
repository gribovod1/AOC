using System.Text;
using AnyThings;

namespace AOC2023
{
    internal class Day17 : DayPattern<(int Level, Int64[] summary)[,]>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new (int, Int64[] summary)[text[0].Length, text.Length];
            for (int row = 0; row < text.Length; row++)
                for (int col = 0; col < text[row].Length; col++)
                {
                    Int64[] summary = new long[4 * 4];
                    for (int i = 0; i < summary.Length; ++i)
                        summary[i] = Int64.MaxValue;
                    data[col, row] = (text[row][col] - '0', summary);
                }
        }

        public override string PartOne()
        {
            /* 
             */
            Int64 result = 0;

            Queue<(int x, int y, int d, int l)> Cood = new();
            for (int i = 0; i < data[0, 0].summary.Length; ++i)
                data[0, 0].summary[i] = 0;
            Cood.Enqueue((0, 0, 0, 0));
            while (Cood.Count > 0)
            {
                var (x, y, d, l) = Cood.Dequeue();
                if (l < 3)
                {
                    if (d != 2) CheckAndAdd(Cood, (x + 1, y, 0, d == 0 ? l + 1 : 1), (x, y, d, l));
                    if (d != 3) CheckAndAdd(Cood, (x, y + 1, 1, d == 1 ? l + 1 : 1), (x, y, d, l));
                    if (d != 0) CheckAndAdd(Cood, (x - 1, y, 2, d == 2 ? l + 1 : 1), (x, y, d, l));
                    if (d != 1) CheckAndAdd(Cood, (x, y - 1, 3, d == 3 ? l + 1 : 1), (x, y, d, l));
                }
                else
                {
                    if (d != 2 && d != 0) CheckAndAdd(Cood, (x + 1, y, 0, 1), (x, y, d, l));
                    if (d != 3 && d != 1) CheckAndAdd(Cood, (x, y + 1, 1, 1), (x, y, d, l));
                    if (d != 0 && d != 2) CheckAndAdd(Cood, (x - 1, y, 2, 1), (x, y, d, l));
                    if (d != 1 && d != 3) CheckAndAdd(Cood, (x, y - 1, 3, 1), (x, y, d, l));
                }
            }

            result = GetMinimum(data.GetLength(0) - 1, data.GetLength(1) - 1);
            return result.ToString();
        }

        public override string PartTwo()
        {
            /* */
            Int64 result = 0;
            for (int row = 0; row < data.GetLength(1); row++)
                for (int col = 0; col < data.GetLength(0); col++)
                {
                    Int64[] summary = new long[11 * 4];
                    for (int i = 0; i < summary.Length; ++i)
                        summary[i] = Int64.MaxValue;
                    data[col, row].summary = summary;
                }

            Queue<(int x, int y, int d, int l)> Cood = new();
            for (int i = 0; i < data[0, 0].summary.Length; ++i)
                data[0, 0].summary[i] = 0;
            Cood.Enqueue((0, 0, 0, 0));
            while (Cood.Count > 0)
            {
                var (x, y, d, l) = Cood.Dequeue();
                if (l < 10)
                {
                    if (d != 2) CheckAndAdd(Cood, d == 0 ? (x + 1, y, 0, l + 1) : (x + 4, y, 0, 4), (x, y, d, l));
                    if (d != 3) CheckAndAdd(Cood, d == 1 ? (x, y + 1, 1, l + 1) : (x, y + 4, 1, 4), (x, y, d, l));
                    if (d != 0) CheckAndAdd(Cood, d == 2 ? (x - 1, y, 2, l + 1) : (x - 4, y, 2, 4), (x, y, d, l));
                    if (d != 1) CheckAndAdd(Cood, d == 3 ? (x, y - 1, 3, l + 1) : (x, y - 4, 3, 4), (x, y, d, l));
                }
                else
                {
                    if (d != 2 && d != 0) CheckAndAdd(Cood, (x + 4, y, 0, 4), (x, y, d, l));
                    if (d != 3 && d != 1) CheckAndAdd(Cood, (x, y + 4, 1, 4), (x, y, d, l));
                    if (d != 0 && d != 2) CheckAndAdd(Cood, (x - 4, y, 2, 4), (x, y, d, l));
                    if (d != 1 && d != 3) CheckAndAdd(Cood, (x, y - 4, 3, 4), (x, y, d, l));
                }
            }

            result = GetMinimum(data.GetLength(0) - 1, data.GetLength(1) - 1);
            return result.ToString();
            /* 
           827 low
           */
        }

        Int64 GetMinimum(int x, int y)
        {
            Int64 result = Int64.MaxValue;
            for (int i = 0; i < data[x, y].summary.Length; ++i)
                if (result > data[x, y].summary[i])
                    result = data[x, y].summary[i];
            return result;
        }

        private void CheckAndAdd(Queue<(int x, int y, int d, int l)> cood, (int x, int y, int d, int l) value, (int x, int y, int d, int l) prev)
        {
            if (value.x < 0 || value.y < 0 || value.x >= data.GetLength(0) || value.y >= data.GetLength(1)) return;
            Int64 summary = data[prev.x, prev.y].summary[prev.l * 4 + prev.d];
            while (prev.x != value.x || prev.y != value.y)
            {
                switch (value.d)
                {
                    case 0:
                        {
                            ++prev.x;
                            break;
                        }
                    case 1:
                        {
                            ++prev.y;
                            break;
                        }
                    case 2:
                        {
                            --prev.x;
                            break;
                        }
                    case 3:
                        {
                            --prev.y;
                            break;
                        }
                }
                summary += data[prev.x, prev.y].Level;
            }

            if (data[value.x, value.y].summary[value.l * 4 + value.d] > summary)
            {
                data[value.x, value.y].summary[value.l * 4 + value.d] = summary;
                cood.Enqueue(value);
            }
        }
    }
}