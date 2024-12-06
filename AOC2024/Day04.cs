using AnyThings;

namespace AOC2024
{
    internal class Day04 : DayPattern<string[]>
    {
        public override void Parse(string singleText)
        {
            data = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string PartOne()
        {
            long result = 0;
            for (var l = 0; l < data.Length; ++l)
                for (var c = 0; c < data[l].Length; ++c)
                {
                    if (CheckWordAndLenght(l, c, 0, 1, "XMAS")) ++result;
                    if (CheckWordAndLenght(l, c, 1, 1, "XMAS")) ++result;
                    if (CheckWordAndLenght(l, c, 1, 0, "XMAS")) ++result;
                    if (CheckWordAndLenght(l, c, 1, -1, "XMAS")) ++result;
                    if (CheckWordAndLenght(l, c, 0, -1, "XMAS")) ++result;
                    if (CheckWordAndLenght(l, c, -1, -1, "XMAS")) ++result;
                    if (CheckWordAndLenght(l, c, -1, 0, "XMAS")) ++result;
                    if (CheckWordAndLenght(l, c, -1, 1, "XMAS")) ++result;
                }
            return result.ToString();
        }

        bool CheckWordAndLenght(int l, int c, int dl, int dc, string word)
        {
            int ll = l + dl * (word.Length - 1);
            if (ll < 0 || ll >= data.Length) return false;
            int lc = c + dc * (word.Length - 1);
            if (lc < 0 || lc >= data[l].Length) return false;

            if (data[l][c] != word[0]) return false;
            if (word.Length == 1) return true;
            return CheckWord(l + dl, c + dc, dl, dc, word.Substring(1));
        }

        bool CheckWord(int l, int c, int dl, int dc, string word)
        {
            if (data[l][c] != word[0]) return false;
            if (word.Length == 1) return true;
            return CheckWord(l + dl, c + dc, dl, dc, word.Substring(1));
        }

        public override string PartTwo()
        {
            long result = 0;
            for (var l = 1; l < data.Length - 1; ++l)
                for (var c = 1; c < data[l].Length - 1; ++c)
                    if (data[l][c] == 'A')
                    {
                        if ((CheckWordAndLenght(l - 1, c - 1, 1, 1, "MAS") ||
                             CheckWordAndLenght(l - 1, c - 1, 1, 1, "SAM")) &&
                            (CheckWordAndLenght(l + 1, c - 1, -1, 1, "MAS") ||
                             CheckWordAndLenght(l + 1, c - 1, -1, 1, "SAM")))
                            ++result;
                    }
            return result.ToString();
        }
    }
}