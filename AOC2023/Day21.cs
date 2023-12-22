using AnyThings;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AOC2023
{
    internal class Day21 : DayPattern<(HashSet<(int x, int y)>  Scala, (int width, int height) Size, (int width, int height) Start)>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = (new(),(0,0),(0,0));
            for (int row = 0; row < text.Length; row++)
                for (int col = 0; col < text[row].Length; col++)
                    if( text[row][col]=='#')
                        data.Scala.Add((col,row));
            else if (text[row][col] == 'S')
                        data.Start=(col, row);
            data.Size = (text[0].Length, text.Length);
        }

        public override string PartOne()
        {
            Int64 result = 0;
            HashSet<(int x, int y)> Start = new();
            Start.Add(data.Start);
            HashSet<(int x, int y)> Available = new();
            StringBuilder sb = new();
            for (int i =0;i<500;++i)
            {
                foreach (var s in Start)
                {
                    Add(Available, (s.x + 1, s.y));
                    Add(Available, (s.x - 1, s.y));
                    Add(Available, (s.x, s.y + 1));
                    Add(Available, (s.x, s.y - 1));
                }
                (Available, Start) = (Start, Available);
                Available.Clear();
                //  if ((i+1) %100 == 0)
                sb.AppendLine($"{i + 1}\t{Start.Count}");
                /*                {
                                    Console.WriteLine($"{i+1} {Start.Count}");
                                    for (int dy = -2; dy <= 2; ++dy)
                                    {
                                        for (int dx = -2; dx <= 2; ++dx)
                                            Console.Write($"{CalcCount(Start, dx, dy)}\t");
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine("");
                                 }*/
            }
            File.WriteAllText($"Data/{GetType().Name}_path.txt", sb.ToString());
            result = Start.Count;
            return result.ToString();
        }

        int CalcCount(HashSet<(int x, int y)> Points, int dx, int dy)
        {
            int count = 0;
            foreach (var s in Points)
            {
                if (s.x >= dx * data.Size.width && s.y >= dy * data.Size.height && s.x < dx * data.Size.width + data.Size.width && s.y < dy * data.Size.height + data.Size.height)
                    ++count;
            }
            return count;
        }

        void Add(HashSet<(int x, int y)> Available, (int x, int y) coord)
        {
           // if (coord.x < 0 || coord.y < 0 || coord.x >= data.Size.width || coord.y >= data.Size.height) return;
            if (data.Scala.Contains((MathMod(coord.x , data.Size.width), MathMod(coord.y , data.Size.height)))) return;
            Available.Add(coord);
        }

        static int MathMod(int a, int b)
        {
            return (Math.Abs(a * b) + a) % b;
        }

        public override string PartTwo()
        {
            /* 
             */
            // Собрать сведения о 
            Int64 result = 0;
                return result.ToString();
        }
    }
}