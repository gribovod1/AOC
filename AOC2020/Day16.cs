using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public class Day16
    {
        class TicketField
        {
            public string Name;
            public List<KeyValuePair<int, int>> Ranges;

            public TicketField(string text)
            {
                Ranges = new List<KeyValuePair<int, int>>();
                var reangeStrings = text.Split(new string[] { ": ", " or " }, StringSplitOptions.RemoveEmptyEntries);
                Name = reangeStrings[0];
                var r1s = reangeStrings[1].Split('-');
                Ranges.Add(new KeyValuePair<int, int>(int.Parse(r1s[0]), int.Parse(r1s[1])));
                r1s = reangeStrings[2].Split('-');
                Ranges.Add(new KeyValuePair<int, int>(int.Parse(r1s[0]), int.Parse(r1s[1])));
            }

            public bool checkNumber(int number)
            {
                return (number >= Ranges[0].Key && number <= Ranges[0].Value) ||
                    (number >= Ranges[1].Key && number <= Ranges[1].Value);
            }
        }
        public void Exec()
        {
            var tickets = File.ReadAllText(@"Data\day16.txt");
            var sTickets = tickets.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var ranges = new List<TicketField>();
            var reangesStrings = sTickets[0].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in reangesStrings)
                ranges.Add(new TicketField(s));

            var nearTickets = new List<List<int>>();
            var nearTicketsText = sTickets[2].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 1; i < nearTicketsText.Length; ++i)
            {
                nearTickets.Add(new List<int>());
                var tNumbers = nearTicketsText[i].Split(',');
                foreach (var nText in tNumbers)
                    nearTickets[nearTickets.Count - 1].Add(int.Parse(nText));
            }

            var yourTicket = new List<int>();
            var yNumbers = sTickets[1].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[1].Split(',');
            foreach (var nText in yNumbers)
                yourTicket.Add(int.Parse(nText));

            var one = partOne(ranges, nearTickets);
            var two = partTwo(ranges, yourTicket, nearTickets);
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        int partOne(List<TicketField> Fields, List<List<int>> nearTickets)
        {
            var summ = 0;
            var i = 0;
            while (i < nearTickets.Count)
            {
                var allTrue = true;
                foreach (var number in nearTickets[i])
                {
                    var c = 0;
                    foreach (var f in Fields)
                        {
                            if (f.checkNumber(number))
                                break;
                            ++c;
                        }
                    if (c == Fields.Count)
                    {
                        Console.WriteLine($"number: {number}");
                        summ += number;
                        allTrue = false;
                    }
                }
                if (!allTrue)
                    nearTickets.RemoveAt(i);
                else
                    ++i;
            }
            return summ;
        }

        ulong partTwo(List<TicketField> Fields, List<int> yourTicket, List<List<int>> nearTickets)
        {
            nearTickets.Add(yourTicket);
            var places = new List<List<int>>();
            for (var i = 0; i < yourTicket.Count; ++i)
                places.Add(new List<int>());                                 
            for (var p = 0; p < places.Count; ++p)
            {
                for (var f = 0; f < Fields.Count; ++f)
                {
                    var c = 0;
                    for (var t = 0; t < nearTickets.Count; ++t)
                    {
                        if (!Fields[f].checkNumber(nearTickets[t][p]))
                            break;
                        ++c;
                    }
                    if (c == nearTickets.Count)
                        places[p].Add(f);
                }
            }
            var place = new List<TicketField>();
            for (var i = 0; i < Fields.Count; ++i)
                place.Add(null);

            for (var i = 0; i < places.Count; ++i)
            {
                var index = findOneCount(places);
                if (index >= 0)
                {
                    place[index] =Fields[places[index][0]];
                    exclude(places, places[index][0]);
                }
            }

            ulong mult = 1;
            for (var p = 0; p < place.Count; ++p)
            {
                if (place[p].Name.IndexOf("departure") >= 0)
                    mult *= (ulong)yourTicket[p];
            }
            return mult;
        }

        int findOneCount(List<List<int>> places)
        {
            for (var i = 0; i < places.Count; ++i)
                if (places[i].Count ==1)
                    return  i;
            return -1;
        }

        void exclude(List<List<int>> places, int fieldIndex)
        {
            for (var i = 0; i < places.Count; ++i)
                places[i].Remove(fieldIndex);
        }
    }
}