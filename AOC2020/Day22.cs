using AOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AOC2020
{
    public class Day22
    {
        public void Exec()
        {
            var text = File.ReadAllText(@"Data\day22.txt");
            var splited = text.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Queue<int> player1 = new Queue<int>();
            var sCards = splited[0].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (var s = 1; s < sCards.Length; ++s)
                player1.Enqueue(int.Parse(sCards[s]));

            Queue<int> player2 = new Queue<int>();
            sCards = splited[1].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (var s = 1; s < sCards.Length; ++s)
                player2.Enqueue(int.Parse(sCards[s]));

            var p_win = game(player1, player2) == 1 ? player1 : player2;
            ulong one = 0;

            int cardCount = p_win.Count;
            var i = p_win.GetEnumerator();
            while (i.MoveNext())
            {
                one += (ulong)(i.Current * cardCount);
                Console.Write($"{i.Current}, ");
                --cardCount;
            }

            Console.WriteLine();
            // one = PartOne();
            var two = PartTwo();
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(one.ToString());
        }

        static string getHash(Queue<int> player1, Queue<int> player2)
        {
            var cardCount = player1.Count + player2.Count;
            var result = new StringBuilder( "P1: ");
            var i = player1.GetEnumerator();
            while (i.MoveNext())
            {
                result.Append(i.Current);
                result.Append(',');
            }
            result.Append("P2");
            i = player2.GetEnumerator();
            while (i.MoveNext())
            {
                result.Append(i.Current);
                result.Append(',');
            }
            return result.ToString();
        }

        static int game(Queue<int> player1, Queue<int> player2)
        {
            var prev = new HashSet<string>();
            do
            {
                var state = getHash(player1, player2);
                if (prev.Contains(state))
                    return 1;
                prev.Add(state);
                
                var p1 = player1.Dequeue();
                var p2 = player2.Dequeue();


                if (p1 <= player1.Count && p2 <= player2.Count)
                {
                    var rPlayer1 = new Queue<int>();
                    var e = player1.GetEnumerator();
                    for (var p1i = 0; p1i < p1; ++p1i)
                    {
                        e.MoveNext();
                        rPlayer1.Enqueue(e.Current);
                    }
                    var rPlayer2 = new Queue<int>();
                    e = player2.GetEnumerator();
                    for (var p2i = 0; p2i < p2; ++p2i)
                    {
                        e.MoveNext();
                        rPlayer2.Enqueue(e.Current);
                    }
                    var p = game(rPlayer1, rPlayer2);
                    if (p == 1)
                    {
                        player1.Enqueue(p1);
                        player1.Enqueue(p2);
                    }
                    else if (p == 2)
                    {
                        player2.Enqueue(p2);
                        player2.Enqueue(p1);
                    }
                    else
                        throw new Exception();
                }
                else if (p1 > p2)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }
                else if (p1 < p2)
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
                else
                    throw new Exception();
            } while (player1.Count > 0 && player2.Count > 0);
            return player1.Count > 0 ? 1 : 2;
        }

        ulong PartOne()
        {
            ulong result = 0;

            return result;
        }

        ulong PartTwo()
        {
            ulong result = 0;
            return result;
        }
    }
}