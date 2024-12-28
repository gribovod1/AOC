using AnyThings;
using System.Security.Cryptography;

namespace AOC2024
{
    internal class Day21 : DayPattern<(Dictionary<(char, char), List<char>> doorToUserTable, string[] codes)>
    {
        public override void Parse(string singleText)
        {
            data = new();
            data.doorToUserTable = new();

            var doorToR1Table = GetDoorKeys();
            var doorToR2Table = GetNextRobot(doorToR1Table);
            var doorToUserTable = GetNextRobot(doorToR2Table);
            data.doorToUserTable = doorToUserTable;

            data.codes = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        Dictionary<(char, char), List<char>> GetDoorKeys()
        {
            Dictionary<(char, char), List<char>> result = new()            {
                { ('A', '0'), new(){ '<','A'} },
                { ('A', '1'), new(){ '^', '<', '<','A' } },
                { ('A', '2'), new(){ '^', '<','A' } },
                { ('A', '3'), new(){ '^', 'A' } },
                { ('A', '4'), new(){ '^','^','<', '<','A' } },
                { ('A', '5'), new(){ '^','^', '<','A' } },
                { ('A', '6'), new(){ '^','^','A' } },
                { ('A', '7'), new(){ '^', '^', '^','<', '<','A' } },
                { ('A', '8'), new(){ '^', '^', '^' , '<','A' } },
                { ('A', '9'), new(){ '^', '^', '^','A' } },
                { ('0', '1'), new(){ '^', '<','A' } },
                { ('0', '2'), new(){ '^','A' } },
                { ('0', '3'), new(){ '^', '>','A' } },
                { ('0', '4'), new(){ '^','^', '<','A' } },
                { ('0', '5'), new(){ '^', '^','A' } },
                { ('0', '6'), new(){ '^', '^', '>', 'A' } },
                { ('0', '7'), new(){ '^', '^', '^', '<', 'A' } },
                { ('0', '8'), new(){ '^', '^', '^', 'A' } },
                { ('0', '9'), new(){ '^', '^', '^', '>', 'A' } },
                { ('1', '2'), new(){ '>', 'A' } },
                { ('1', '3'), new(){ '>', '>', 'A' } },
                { ('1', '4'), new(){ '^', 'A' } },
                { ('1', '5'), new(){ '^',  '>', 'A' } },
                { ('1', '6'), new(){ '^', '>', '>','A' } },
                { ('1', '7'), new(){ '^', '^', 'A' } },
                { ('1', '8'), new(){ '^', '^', '^', '>', 'A' } },
                { ('1', '9'), new(){ '^', '^', '^', '>', '>','A' } },
                { ('2', '3'), new(){ '>', 'A' } },
                { ('2', '4'), new(){ '^',  '<', 'A' } },
                { ('2', '5'), new(){ '^', 'A' } },
                { ('2', '6'), new(){ '^', '>', 'A' } },
                { ('2', '7'), new(){ '^', '^',  '<', 'A' } },
                { ('2', '8'), new(){ '^', '^',  'A' } },
                { ('2', '9'), new(){ '^', '^',  '>', 'A' } },
                { ('3', '4'), new(){ '^', '<', '<', 'A' } },
                { ('3', '5'), new(){ '^', '<', 'A' } },
                { ('3', '6'), new(){ '^',  'A' } },
                { ('3', '7'), new(){ '^', '^', '<', '<', 'A' } },
                { ('3', '8'), new(){ '^', '^','<', 'A' } },
                { ('3', '9'), new(){ '^', '^', 'A' } },
                { ('4', '5'), new(){ '>', 'A' } },
                { ('4', '6'), new(){ '>', '>', 'A' } },
                { ('4', '7'), new(){ '^', 'A' } },
                { ('4', '8'), new(){ '^', '>', 'A' } },
                { ('4', '9'), new(){ '^', '>', '>', 'A' } },
                { ('5', '6'), new(){ '>', 'A' } },
                { ('5', '7'), new(){ '^',  '<', 'A' } },
                { ('5', '8'), new(){ '^',  'A' } },
                { ('5', '9'), new(){ '^', '>', 'A' } },
                { ('6', '7'), new(){ '^',  '<', '<', 'A' } },
                { ('6', '8'), new(){ '^',  '<', 'A' } },
                { ('6', '9'), new(){ '^', 'A' } },
                { ('7', '8'), new(){ '>', 'A' } },
                { ('7', '9'), new(){ '>', '>', 'A' } },
                { ('8', '9'), new(){ '>',  'A' } },
            };
            var l = result.ToList();
            for (int i = 0; i < l.Count; ++i)
                result.Add((l[i].Key.Item2, l[i].Key.Item1), ReturnPath(l[i].Value));
            var e = new List<char>() { 'A', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            foreach (char c in e)
                result.Add((c, c), new List<char> { 'A' });
            return result;
        }

        List<char> ReturnPath(List<char> path)
        {
            List<char> result = new();
            foreach (char c in path)
                switch (c)
                {
                    case '<': { result.Add('>'); break; }
                    case '>': { result.Add('<'); break; }
                    case 'v': { result.Add('^'); break; }
                    case '^': { result.Add('v'); break; }
                    default: { result.Add(c); break; }
                }
            return result;
        }

        Dictionary<(char, char), List<char>> GetRobotTranslation()
        {
            Dictionary<(char, char), List<char>> result = new()            {
                { ('A', '>'), new(){ 'v', 'A' } },
                { ('A', 'v'), new(){ 'v', '<', 'A' } },
                { ('A', '<'), new(){ 'v', '<', '<', 'A' } },
                { ('A', '^'), new(){ '<', 'A' } },
                { ('>', 'v'), new(){ '<', 'A' } },
                { ('>', '<'), new(){ '<', '<', 'A' } },
                { ('>', '^'), new(){ '<', '^', 'A' } },
                { ('v', '<'), new(){ '<', 'A' } },
                { ('v', '^'), new(){ '^', 'A' } },
                { ('<', '^'), new(){ '>', '^', 'A' } },
            };
            var l = result.ToList();
            for (int i = 0; i < l.Count; ++i)
                result.Add((l[i].Key.Item2, l[i].Key.Item1), ReturnPath(l[i].Value));
            var e = new List<char>() { 'A', '>', 'v', '<', '^' };
            foreach (char c in e)
                result.Add((c, c), new List<char> { 'A' });
            return result;
        }

        Dictionary<(char, char), List<char>> GetNextRobot(Dictionary<(char, char), List<char>> prev)
        {
            Dictionary<(char, char), List<char>> result = new();
            var translation = GetRobotTranslation();
            foreach (var p in prev)
            {
                List<char> newPath = new();

                char prevKey = 'A';
                for (int index = 0; index < p.Value.Count; ++index)
                {
                    newPath.AddRange(translation[(prevKey, p.Value[index])]);
                    prevKey = p.Value[index];
                }
                result.Add(p.Key, newPath);
            }
            return result;
        }


        public override string PartOne()
        {
            long result = 0;
            foreach (var c in data.codes)
            {
                var code = c;
                long length = 0;
                char prev = 'A';
                for (int i = 0; i < code.Length; ++i)
                {
                    length += data.doorToUserTable[(prev, code[i])].Count;
                    prev = code[i];
                }
                result += length * long.Parse(code.Substring(0, code.Length - 1));
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            return result.ToString();
        }
    }
}