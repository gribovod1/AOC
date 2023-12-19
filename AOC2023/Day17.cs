using System.Text;
using AnyThings;

namespace AOC2023
{
    internal class Day17 : DayPattern<(int Level, Int64[] summary)[,]>
    {
        struct Point {
            public int x;
            public int y; 
            public int d; 
            public int l;
            public List<(int x, int y)> path;

            public Point(int x, int y, int d, int l) {
                this.x=x;
                this.y=y;
                this.d=d;
                this.l=l;
                this.path=new();
            }

            public Point(int x, int y, int d, int l, List<(int x, int y)> path) {
                this.x=x;
                this.y=y;
                this.d=d;
                this.l=l;
                this.path=new(path);
                this.path.Add((x, y));
            }
        }
        List<(int x, int y)>[] Paths = new List<(int x, int y)>[11 * 4];
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new (int, Int64[] summary)[text[0].Length, text.Length];
            for (int row = 0; row < text.Length; row++)
                for (int col = 0; col < text[row].Length; col++)
                {
                    Int64[] summary = new long[11 * 4];
                    for (int i = 0; i < summary.Length; ++i)
                        summary[i] = Int64.MaxValue;
                    data[col, row] = (text[row][col] - '0', summary);
                }
        }

        public override string PartOne()
        {
            /* Используется обычный обход по карте, с выбором минимального пути, но хранится не один минимум, а каждый для
             * всех комбинаций направления и длины пути.
             * В начале добавляются два направления, чтобы с начала использовалось правило для минимального шага.
             */
            Int64 result = 0;

            Queue<Point> Cood = new();
            for (int i = 0; i < data[0, 0].summary.Length; ++i)
                data[0, 0].summary[i] = 0;
            Cood.Enqueue(new Point(0, 0, 3, 0));
            Cood.Enqueue(new Point(0, 0, 2, 0));
            while (Cood.Count > 0)
                Step(Cood, Cood.Dequeue(), 1, 3);

            (result, int index) = GetMinimum(data.GetLength(0) - 1, data.GetLength(1) - 1);
            return result.ToString();
        }

        public override string PartTwo()
        {
            /* */
            Int64 result = 0;
            for (int row = 0; row < data.GetLength(1); row++)
                for (int col = 0; col < data.GetLength(0); col++)
                    for (int i = 0; i < data[col, row].summary .Length; ++i)
                        data[col, row].summary [i] = Int64.MaxValue;

            Queue<Point> Cood = new();
            for (int i = 0; i < data[0, 0].summary.Length; ++i)
                data[0, 0].summary[i] = 0;
            Cood.Enqueue(new Point(0, 0, 3, 0));
            Cood.Enqueue(new Point(0, 0, 2, 0));
            while (Cood.Count > 0)
                Step(Cood, Cood.Dequeue(), 4, 10);

            (result, int index) = GetMinimum(data.GetLength(0) - 1, data.GetLength(1) - 1);



            if (index >= 0 && Paths[index] != null)
            for(int i =0;i < Paths[index].Count;++i) {
                data[Paths[index][i].x, Paths[index][i].y].Level = 0;
            }

            StringBuilder sb = new();
            for (int row = 0; row < data.GetLength(1); row++) {                
                for (int col = 0; col < data.GetLength(0); col++) {
                    sb.Append(data[col,row].Level == 0 ? '#' : data[col,row].Level.ToString());
                }
                sb.AppendLine();
            }
            sb.AppendLine();


            if (index >= 0 && Paths[index] != null)
            for(int i =0;i < Paths[index].Count;++i) {
                sb.AppendLine($"({Paths[index][i].x} {Paths[index][i].y})");
            }

            File.WriteAllText($"Data/{GetType().Name}_path.txt", sb.ToString());

            return result.ToString();
        }

        void Step(Queue<Point> Coordinates, Point p, int MinStep, int MaxLength) {
            if (p.l < MaxLength)
            {
                if (p.d != 2) CheckAndAdd(Coordinates, p.d == 0 ? new Point(p.x + 1, p.y, 0, p.l + 1, p.path) : new Point(p.x + MinStep, p.y, 0, MinStep, p.path), p);
                if (p.d != 3) CheckAndAdd(Coordinates, p.d == 1 ? new Point(p.x, p.y + 1, 1, p.l + 1, p.path) : new Point(p.x, p.y + MinStep, 1, MinStep, p.path), p);
                if (p.d != 0) CheckAndAdd(Coordinates, p.d == 2 ? new Point(p.x - 1, p.y, 2, p.l + 1, p.path) : new Point(p.x - MinStep, p.y, 2, MinStep, p.path), p);
                if (p.d != 1) CheckAndAdd(Coordinates, p.d == 3 ? new Point(p.x, p.y - 1, 3, p.l + 1, p.path) : new Point(p.x, p.y - MinStep, 3, MinStep, p.path), p);
            }
            else
            {
                if (p.d != 2 && p.d != 0) CheckAndAdd(Coordinates, new Point(p.x + MinStep, p.y, 0, MinStep, p.path), p);
                if (p.d != 3 && p.d != 1) CheckAndAdd(Coordinates, new Point(p.x, p.y + MinStep, 1, MinStep, p.path), p);
                if (p.d != 0 && p.d != 2) CheckAndAdd(Coordinates, new Point(p.x - MinStep, p.y, 2, MinStep, p.path), p);
                if (p.d != 1 && p.d != 3) CheckAndAdd(Coordinates, new Point(p.x, p.y - MinStep, 3, MinStep, p.path), p);
            }
        }

        (Int64,int) GetMinimum(int x, int y)
        {
            Int64 result = Int64.MaxValue;
            int maxIndex = -1;
            var s = data[x, y].summary;
            for (int i = 0; i < s.Length; ++i)
                if (result > s[i])
                    {
                        result = s[i];
                        maxIndex = i;
                    }
            return (result,maxIndex);
        }

        private void CheckAndAdd(Queue<Point> cood, Point value, Point prev)
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
                if (value.x == data.GetLength(0)-1 && value.y == data.GetLength(1)-1) {
                    Paths[value.l * 4 + value.d] = value.path;
                    data[value.x, value.y].summary[value.l * 4 + value.d] = summary;
               } else {
                    data[value.x, value.y].summary[value.l * 4 + value.d] = summary;
                    cood.Enqueue(value);
                }
            }
        }
    }
}