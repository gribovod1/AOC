using AnyThings;

namespace AOC2023
{
    internal class Day14 : DayPattern<Dictionary<(int x, int y), char>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (int row = 0; row < text.Length; row++)
                for (int col = 0; col < text[row].Length; col++)
                    if (text[row][col] != '.')
                        data.Add((col, row), text[row][col]);
            Size = (text[0].Length, text.Length);
        }

        (int x, int y) Size;

        public override string PartOne()
        {
            /* Последовательно, по координатам, считываются все точки, если в какой-то обнаруживается круглый камень, 
             * то он катится "на север", пока не найдёт другой любой камень (даже и круглый - в том направлении, 
             * всё что могло встретиться, уже докатилось).
             */
            Int64 result = Move(0, -1);
            return result.ToString();
        }

        private Int64 Move(int dx, int dy)
        {
            Int64 result = 0;
            for (int cx = 0; cx < Size.x; ++cx)
            {
                int x = cx;
                if (dx > 0)
                    x = Size.x - 1 - cx;
                for (int cy = 0; cy < Size.y; ++cy)
                {
                    int y = cy;
                    if (dy > 0)
                        y = Size.y - 1 - cy;
                    char type = '.';
                    if (data.TryGetValue((x, y), out type) && type == 'O')
                    {
                        int newY = y + dy;
                        if (dy != 0)
                        {
                            while (newY >= 0 && newY < Size.y && !data.ContainsKey((x, newY)))
                                newY += dy;
                            newY -= dy;
                        }
                        int newX = x + dx;
                        if (dx != 0)
                        {
                            while (newX >= 0 && newX < Size.x && !data.ContainsKey((newX, newY)))
                                newX += dx;
                            newX -= dx;
                        }
                        if (newY != y || newX != x)
                        {
                            data.Remove((x, y));
                            data.Add((newX, newY), 'O');
                        }
                        result += Size.y - newY;
                    }
                }
            }
            return result;
        }

        Int64 MoveCycle()
        {
            Move(0, -1);
            Move(-1, 0);
            Move(0, 1);
            return Move(1, 0);
        }

        public override string PartTwo()
        {
            /* Для начала заканчивается цикл, начатый ещё на предыдущем задании. После чего цикл повторяется раз 100, чтобы "затереть"
             * стартовую позицию. Дальше, после каждого цикла проверяется: а не начала ли последовательность результатов повторяться?
             * Когда повтор обнаружен, можно расчитать индекс результата в массиве для любого числа циклов.*/
            Int64 result = 0;
            Move(-1, 0);
            Move(0, 1);
            var p = Move(1, 0);
            List<Int64> Press = new();
            Press.Add(p);
            int COUNT = 1000000000;
            (int StartIndex, int Length) Sequence;
            for (int i = 0; i < 100; ++i)
                Press.Add(MoveCycle());
            while (!FindPattern(Press, out Sequence))
                Press.Add(MoveCycle());
            result = Press[(COUNT - Sequence.StartIndex - 1) % Sequence.Length + Sequence.StartIndex];
            return result.ToString();
        }

        bool FindPattern(List<Int64> Values, out (int StartIndex, int Length) Sequence)
        {
            Sequence = (0, 0);
            for (int length = 4; length < Values.Count / 2; length++)
            {
                var startOne = Values.Count - 2 * length;
                var startTwo = Values.Count - 1 * length;
                if (CompareSubArray(Values, startOne, startTwo, length))
                {
                    Sequence = (startTwo, length);
                    return true;
                }
            }
            return false;
        }

        bool CompareSubArray(List<Int64> Values, int xIndex, int yIndex, int length)
        {
            for (int i = 0; i < length; ++i)
                if (Values[xIndex + i] != Values[yIndex + i])
                    return false;
            return true;
        }
    }
}