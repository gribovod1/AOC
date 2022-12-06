using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2015
{
    public class Day19
    {
        public void Exec()
        {
            var source = File.ReadAllLines(@"Data\day19.txt");
            var startTime = DateTime.Now;
            var one = partOne(source);
            var two = partTwo(source);
            var endTime = DateTime.Now;
            Console.WriteLine($"partOne: {one} partTwo: {two} time: {(endTime - startTime).TotalSeconds}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        int partOne(string[] source)
        {
            var variants = new HashSet<string>();
            var rules = getRules(source);
            foreach (var rule in rules)
            {
                react(source[source.Length - 1], rule.Key, rule.Value, variants);
            }
            return variants.Count;
        }

        private void react(string molecula, string replaceable, string replacement, HashSet<string> variants)
        {
            var start = -1;
            while ((start = molecula.IndexOf(replaceable, start + 1)) >= 0)
            {
                var variant = molecula.Substring(0, start) + replacement + molecula.Substring(start + replaceable.Length, molecula.Length - start - replaceable.Length);
                variants.Add(variant);
            }
        }

        string replaceOneWord(string source, string replaceable, string replacement, ref int startIndex)
        {
            var start = source.IndexOf(replaceable, startIndex);
            if (start < 0)
            {
                return source;
            }
            startIndex = start;
            return source.Substring(0, start) + replacement + source.Substring(start + replaceable.Length, source.Length - start - replaceable.Length);
        }

        string replaceOneWord(string source, string replaceable, string replacement)
        {
            var start = source.IndexOf(replaceable);
            if (start < 0)
            {
                return source;
            }
            return source.Substring(0, start) + replacement + source.Substring(start + replaceable.Length, source.Length - start - replaceable.Length);
        }

        class Node
        {
            int rule;
            int charIndex;
            string molecula;

            public Node(string molecula)
            {
                this.molecula = molecula;
                this.rule = 0;
                this.charIndex = 0;
            }

            public bool isEnd()
            {
                return molecula == "e";
            }

            public Node getNextNode(List<KeyValuePair<string, string>> rules, HashSet<string> variants)
            {
                for (; rule < rules.Count; ++rule)
                {
                    var index = 0;
                    while ((index = molecula.IndexOf(rules[rule].Value, charIndex)) >= 0)
                    {
                        charIndex = index;
                        var variant = molecula.Substring(0, index) + rules[rule].Key + molecula.Substring(index + rules[rule].Value.Length, molecula.Length - index - rules[rule].Value.Length);
                        if (variants.Add(variant))
                        {
                            return new Node(variant);
                        }
                    }
                    charIndex = 0;
                }
                return null;
            }
        }

        int partTwo(string[] source)
        {
            var rules = getRules(source);
            var molecula = source[source.Length - 1];
            var variants = new HashSet<string>();
            var path = new Stack<Node>();
            var currentNode = new Node(molecula);
            do
            {
                if (currentNode.isEnd())
                {
                    break;
                }
                var node = currentNode.getNextNode(rules, variants);
                if (node != null)
                {
                    path.Push(currentNode);
                    currentNode = node;
                }
                else
                {
                    currentNode = path.Pop();
                }
            }
            while (path.Count > 0);
            return path.Count;
        }

        List<KeyValuePair<string, string>> getRules(string[] source)
        {
            var rules = new List<KeyValuePair<string, string>>();
            var divString = " => ";
            var molecula = source[source.Length - 1];
            for (var index = 0; index < source.Length - 2; ++index)
            {
                var d = source[index].IndexOf(divString);
                rules.Add(new KeyValuePair<string, string>(source[index].Substring(0, d), source[index].Substring(d + divString.Length, source[index].Length - d - divString.Length)));
            }
            return rules;
        }
    }
}