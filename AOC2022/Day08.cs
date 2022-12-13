using AnyThings;

namespace AOC2022
{
    internal class Day08 : DayPattern<int[,]>
    {
        public override void Parse(string path)
        {
            var text = File.ReadAllText(path).Split(Environment.NewLine);
            data = new int[text.Length, text.Length];
            for (var i = 0; i < text.Length; ++i)
            {
                for (var c = 0; c < text[i].Length; ++c)
                {
                    data[c, i] = int.Parse(text[i][c].ToString());
                }
            }
        }

        public override string PartOne()
        {
            int counter = 4 * data.GetLength(0) - 4;

            for (var x = 1; x <= data.GetLength(0) - 2; ++x)
                for (var y = 1; y <= data.GetLength(0) - 2; ++y)
                    if (treeVisible(x, y))
                        ++counter;
            return counter.ToString();
        }

        public override string PartTwo()
        {
            int counter = 0;

            for (var x = 1; x <= data.GetLength(0) - 2; ++x)
                for (var y = 1; y <= data.GetLength(0) - 2; ++y)
                {
                    var c = treeVolume(x, y);
                    if (c > counter)
                        counter = c;
                }
            return counter.ToString();
        }

        private bool treeVisible(int x, int y)
        {
            var max = data[x, y];
            var visible = true;
            for (var i = x + 1; i < data.GetLength(0); i++)
                if (data[i, y] >= max)
                {
                    visible = false;
                    break;
                }
            if (visible) return true;
            visible = true;
            for (var i = y + 1; i < data.GetLength(0); i++)
                if (data[x, i] >= max)
                {
                    visible = false;
                    break;
                }
            if (visible) return true; visible = true;
            for (var i = x - 1; i >= 0; --i)
                if (data[i, y] >= max)
                {
                    visible = false;
                    break;
                }
            if (visible) return true; visible = true;
            for (var i = y - 1; i >= 0; --i)
                if (data[x, i] >= max)
                {
                    visible = false;
                    break;
                }
            if (visible) return true;
            return false;
        }

        private int treeVolume(int x, int y)
        {
            var max = data[x, y];
            var result = 1;
            var count = 0;
            for (var i = x + 1; i < data.GetLength(0); i++)
            {
                ++count;
                if (data[i, y] >= max)
                    break;
            }
            result *= count;
            count = 0;
            for (var i = y + 1; i < data.GetLength(0); i++)
            {
                ++count;
                if (data[x, i] >= max)
                    break;
            }
            result *= count;
            count = 0;
            for (var i = x - 1; i >= 0; --i)
            {
                ++count;
                if (data[i, y] >= max)
                    break;
            }
            result *= count;
            count = 0;
            for (var i = y - 1; i >= 0; --i)
            {
                ++count;
                if (data[x, i] >= max)
                    break;
            }
            result *= count;
            return result;
        }
    }
}