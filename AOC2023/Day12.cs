using AnyThings;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day12 : DayPattern<List<(string Springs, int[] Groups)>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            foreach (var s in text)
            {
                var row = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var ns = row[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                int[] n = new int[ns.Length];
                for (int i = 0; i < ns.Length; ++i)
                    n[i] = int.Parse(ns[i]);
                data.Add((row[0].Replace('.', 'T').Replace('#', 'S').Replace('?', 'Q'), n));
            }
        }

        public override string PartOne()
        {
            Int64 result = 0;
            List<Int64> counts = new();
            for (int s = 0; s < data.Count; ++s)
            {
                counts.Add(GetCounts(s, 0, 0, new(), new()));
                result += counts[counts.Count - 1];
            }
            StringBuilder sb = new();
            foreach (var c in counts)
            {
                sb.AppendLine(c.ToString());
            }
            var debug = sb.ToString();
            return result.ToString();
        }

        Int64 GetCounts(int PuzzleIndex, int GroupIndex, int PatternStartIndex, List<(int Index, int Length)> Groups, Dictionary<(int, int), Int64> Memo)
        {
            //           if (Memo.ContainsKey((GroupIndex, PatternStartIndex)))
            //               return Memo[(GroupIndex, PatternStartIndex)];
            Int64 result = 0;
            for (int i = PatternStartIndex; i < data[PuzzleIndex].Springs.Length - (data[PuzzleIndex].Groups[GroupIndex] - 1); ++i)
            {
                if (Memo.ContainsKey((GroupIndex, i)))
                    result += Memo[(GroupIndex, i)];
                else
                {
                    if (Match(data[PuzzleIndex].Springs, i, data[PuzzleIndex].Groups[GroupIndex]))
                    {
                        if (GroupIndex < data[PuzzleIndex].Groups.Length - 1)
                        {
                            if (MatchPart(PuzzleIndex, Groups, GroupIndex, i, i + data[PuzzleIndex].Groups[GroupIndex]))
                            {
                                Groups.Add((i, data[PuzzleIndex].Groups[GroupIndex]));
                                var c = GetCounts(PuzzleIndex, GroupIndex + 1, i + data[PuzzleIndex].Groups[GroupIndex] + 1, Groups, Memo);
                                Groups.RemoveAt(Groups.Count - 1);
                                result += c;
                                Memo.Add((GroupIndex, i), c);
                            }
                        }
                        else
                        {
                            if (MatchPart(PuzzleIndex, Groups, GroupIndex, i, data[PuzzleIndex].Springs.Length))
                                ++result;
                        }
                    }
                }
            }
            //            Memo.Add((GroupIndex, PatternStartIndex), result);
            return result;
        }

        bool MatchPart(int PuzzleIndex, List<(int Index, int Length)> Groups, int GroupIndex, int GroupStart, int Length)
        {
            StringBuilder match = new(new string('T', data[PuzzleIndex].Springs.Length), data[PuzzleIndex].Springs.Length);
            for (int g = 0; g < Groups.Count; ++g)
            {
                int c = 0;
                if (Groups[g].Index > 0)
                    match[Groups[g].Index - 1] = 'T';
                for (; c < Groups[g].Length; ++c)
                    match[Groups[g].Index + c] = 'S';
                if (Groups[g].Index + c < data[PuzzleIndex].Springs.Length)
                    match[Groups[g].Index + c] = 'T';
            }
            for (int c = 0; c < data[PuzzleIndex].Groups[GroupIndex]; ++c)
                match[GroupStart + c] = 'S';

            var regex = new Regex("^(" + data[PuzzleIndex].Springs.Substring(0, Length).Replace("Q", "[TS]") + ")$");
            return regex.IsMatch(match.ToString(0, Length));
        }

        bool Match(string Pattern, int Start, int Count)
        {
            if (Pattern.Length - Start < Count) return false;
            if (Start > 0 && Pattern[Start - 1] == 'S')
                return false;
            if (Pattern.Length > Start + Count && Pattern[Start + Count] == 'S')
                return false;
            for (int i = 0; i < Count; i++)
                if (Pattern[i + Start] == 'T') return false;
            return true;
        }

        public override string PartTwo()
        {
            Int64 result = 0;
            for (int s = 0; s < data.Count; ++s)
            {
                StringBuilder tSprings = new();
                int[] newGroups = new int[data[s].Groups.Length * 5];
                for (int i = 0; i < 5; ++i)
                {
                    tSprings.Append($"{data[s].Springs}Q");
                    data[s].Groups.CopyTo(newGroups, data[s].Groups.Length * i);
                }
                data[s] = (tSprings.ToString(0, tSprings.Length - 1), newGroups);
                result += GetCounts(s, 0, 0, new(), new());
            }
            return result.ToString();
        }
    }
}