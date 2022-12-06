using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public class Day19
    {
        Dictionary<int, string> Rules = new Dictionary<int, string>();

        public void Exec()
        {
            var text = File.ReadAllText(@"Data\day19.txt");
            var splited = text.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var rulesText = splited[0].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Rules = new Dictionary<int, string>();
            for (var i = 0; i < rulesText.Length; ++i)
            {
                var indexD = rulesText[i].IndexOf(':');
                Rules.Add(int.Parse(rulesText[i].Substring(0, indexD)), rulesText[i].Substring(indexD + 2, rulesText[i].Length - indexD - 2));
            }
            var msgText = splited[1].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var one = PartOne(msgText);
            var two = PartTwo(msgText);
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        ulong PartOne( string[] msgText)
        {
            ulong result = 0;

            for (var m = 0; m < msgText.Length; ++m)
            {
                   var jumps = new Queue<int>();
                jumps.Enqueue(0);
                if (CheckRule(jumps, msgText[m], 0))
                    ++result;
            }
            return result;
        }

        ulong PartTwo(string[] msgText)
        {
            ulong result = 0;
            Rules[8] = "42 | 42 8";
            Rules[11] = "42 31 | 42 11 31";
            for (var m = 0; m < msgText.Length; ++m)
            {
                var jumps = new Queue<int>();
                jumps.Enqueue(0);
                if (CheckRule(jumps, msgText[m], 0))
                    ++result;
            }
            return result;
        }

        bool CheckRule(Queue<int> jumpArray, string text, int indexText)
        {
            if (indexText == text.Length)
                return jumpArray.Count == 0;
            if (jumpArray.Count == 0)
                return false;
            int indexRule = jumpArray.Dequeue();
            var ruleText = Rules[indexRule];
            var indexChar = ruleText.IndexOf("\"");
            if (indexChar >= 0)
                return text[indexText] == ruleText[indexChar + 1] 
                    ? CheckRule(jumpArray, text, indexText + 1) 
                    : false;
            var indexDiv = ruleText.IndexOf("|");
            if (indexDiv > 0)
                return ExtendJumpers(ruleText.Substring(0, indexDiv), jumpArray, text, indexText) ||
                    ExtendJumpers(ruleText.Substring(indexDiv, ruleText.Length - indexDiv), jumpArray, text, indexText);
            return ExtendJumpers(ruleText, jumpArray, text, indexText);
        }

        bool ExtendJumpers(string currentJumpers, Queue<int> jumps, string text, int indexText)
        {
            var extJumps = new Queue<int>();
            var jumpersText = currentJumpers.Split(new char[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var j in jumpersText)
                extJumps.Enqueue(int.Parse(j));
            foreach (var j in jumps)
                extJumps.Enqueue(j);
            return CheckRule(extJumps, text, indexText);
        }
    }
}