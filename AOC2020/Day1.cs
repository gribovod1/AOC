using System;
using System.IO;

namespace AOC2020
{
    class Day1
    {
        static void exec()
        {
            var ss = File.ReadAllText(@"Data\day1.txt").Split(new string[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries);
            var nums = new int[ss.Length];
            for (var i = 0; i < ss.Length; i++)
            {
                nums[i] = int.Parse(ss[i]);
                for (var j = 0; j < i; j++)
                {
                    for (var k = 0; k < j; k++)
                    {
                        if (nums[j] + nums[i] + nums[k] == 2020)
                        {
                            Console.WriteLine(nums[j] * nums[i] * nums[k]);
                            Console.ReadKey();
                            return;
                        }
                    }
                }
            }
        }
    }
}
