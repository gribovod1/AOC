using AnyThings;
using Coord = (int r, int c);

namespace AOC2024
{
    internal class Day11 : DayPattern<List<long>>
    {
        public override void Parse(string singleText)
        {
            string[] nText = singleText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            data = new();
            foreach (var t in nText)
                data.Add(long.Parse(t));
        }

        public override string PartOne()
        {
            long result = CalcStoneCount(50);
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;// CalcStoneCount(75);
            return result.ToString();
        }

        private long CalcStoneCount(int v)
        {
            Console.Write($"{data.Count}: ");
            for (var s = 0; s < data.Count && s < 10; ++s)
                Console.Write($"{data[s]} ");
            Console.WriteLine();
            List<long> current = data;
            int PREV = current.Count;
            for (var i = 0; i < v; ++i)
            {
                current = ProcessStones(current);
                current.Sort();
                int Count0 = 0;
                int CountOdd = 0;
                int CountOther = 0;
                for (var s = 0; s < current.Count; ++s)
                {
                    if (current[s] == 0)
                        ++Count0;
                    else if ((current[s].ToString().Length % 2) == 0)
                    {
                        ++CountOdd;
                    } else
                    {
                        ++CountOther;
                    }
                }

                Console.Write($"{current.Count}: {Count0} {CountOdd} {CountOther}");
                PREV = current.Count;
                Console.WriteLine();
            }
            Console.WriteLine();


            List<char> text = new();
            foreach(var c in current)
            {
                var n = c.ToString();
                text.AddRange(n);
            }
            Console.WriteLine($"Text length: {text.Count}");
            int lenght = 1;

            while (lenght < text.Count / 2)
            {
                int one = 0;
                int two = lenght;
                while (one < lenght && text[one] == text[two])
                {
                    ++one;
                    ++two;
                }
                if (one == lenght)
                    Console.WriteLine($"Sub length: {one}");
                ++lenght;
            }
            Console.WriteLine();

           // var sub = longestSubstring(current);
           // Console.WriteLine($"{sub.Count}");
            return current.Count;
        }

        private List<long> ProcessStones(List<long> current)
        {
            List<long> result = new();
            foreach(var s in current)
            {
                if (s == 0)
                    result.Add(1);
                else
                {
                    var t = s.ToString();
                    if ((t.Length % 2) == 0)
                    {
                        result.Add(long.Parse(t.Substring(0, t.Length / 2)));
                        result.Add(long.Parse(t.Substring(t.Length / 2)));
                    }
                    else
                    {
                        result.Add(s * 2024);
                    }
                }
            }
            return result;
        }

        static List<long> longestSubstring(List<long> s)
        {
            int n = s.Count;
            int[] dp = new int[n + 1];

            List<long> ans = new();
            int ansLen = 0;

            // find length of non-overlapping 
            // substrings for all pairs (i, j)
            for (int i = n - 1; i >= 0; i--)
            {
                for (int j = i; j < n; j++)
                {

                    // if characters match, set value 
                    // and compare with ansLen.
                    if (s[i] == s[j])
                    {
                        dp[j] = 1 + Math.Min(dp[j + 1], j - i - 1);

                        if (dp[j] >= ansLen)
                        {
                            ansLen = dp[j];
                            ans = s.GetRange(i, ansLen);
                        }
                    }
                    else
                    {
                        dp[j] = 0;
                    }
                }
            }

            return ans;
        }
    }
}