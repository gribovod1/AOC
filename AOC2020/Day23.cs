using AOC;
using System;
using System.Collections.Generic;

namespace AOC2020
{
    public class Day23
    {
        public void Exec()
        {
            PartOne();
            PartTwo();
        }

        ulong PartOne()
        {
            var text = "952438716";
            var stepCount = 100;
            for (var i = 0; i < stepCount; ++i)
            {
                var excText = text.Substring(1, 3);
                var targetValue = text[0] - 1 < '1' ? '9' : (char)(text[0] - 1);
                while (excText.IndexOf(targetValue) >= 0)
                    targetValue = targetValue - 1 < '1' ? '9' : (char)(targetValue - 1);
                var index = text.IndexOf(targetValue);
                text = text.Remove(1, 3).Insert(index - 2, excText);
                text = next(text);
            }
            while (text[0] != '1')
                text = next(text);
            text = text.Substring(1, text.Length - 1);
            Console.WriteLine($"partOne: {text}");
            return 0;
        }

        string next(string text)
        {
            return text.Substring(1, text.Length - 1) + text[0];
        }

        ulong PartTwo()
        {
            var text = "952438716";
            var minValue = 1;
            var maxValue = 1000000;
            var work = new LinkedList<int>();
            var nodes = new Dictionary<int, LinkedListNode<int>>();
            for (var i = 0; i < 9; i++)
                nodes.Add(text[i] - 0x30, work.AddLast(text[i] - 0x30));
            for (var i = 10; i <= 1000000; i++)
                nodes.Add(i, work.AddLast(i));

            var stepCount = 10000000;
            var currNode = work.First;
            for (var i = 0; i < stepCount; ++i)
            {
                var targetValue = currNode.Value - 1 >= minValue ? currNode.Value - 1 : maxValue;
                var targetNode = nodes[targetValue];
                while (isNear(3, currNode, targetNode))
                {
                    targetValue = targetValue - 1 >= minValue ? targetValue - 1 : maxValue;
                    targetNode = nodes[targetValue];
                }
                for (var c = 0; c < 3; ++c)
                {
                    var node = currNode.Next;
                    if (node == null)
                        node = work.First;
                    work.Remove(node);
                    work.AddAfter(targetNode, node);
                    targetNode = node;
                }
                currNode = currNode.Next;
                if (currNode == null)
                    currNode = work.First;
            }
            var nodeOne = nodes[1];
            ulong result = (ulong)nodeOne.Next.Value * (ulong)nodeOne.Next.Next.Value;
            Console.WriteLine($"partTwo: {result}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(result.ToString());
            return 0;
        }

        bool isNear(int count, LinkedListNode<int> node, LinkedListNode<int> target)
        {
            while (count > 0 && node != target)
            {
                node = node.Next;
                if (node == null)
                    node = target.List.First;
                --count;
            }
            return node == target;
        }
    }
}