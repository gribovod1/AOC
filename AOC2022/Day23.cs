using AnyThings;
using System.Drawing;

namespace AOC2022
{
    internal class Day23 : DayPattern<string[]>
    {
        public override void Parse(string singleText)
        {
            data = singleText.Split(Environment.NewLine);
            for (var y = 0; y < data.Length; ++y)
                for (var x = 0; x < data[y].Length; ++x)
                    if (data[y][x] == '#')
                        Elves.Add((x, y));
            CheckList.Add(checkN);
            CheckList.Add(checkS);
            CheckList.Add(checkW);
            CheckList.Add(checkE);
        }

        public override string PartOne()
        {
            var result = (0, false);
            for (var s = 0; s < 10; ++s)
                result = Round();
            return $"{result.Item1}";
        }

        public override string PartTwo()
        {
            while (Round().Item2) ;
            return $"{RoundCounter}";
        }

        (int, bool) Round()
        {
            ++RoundCounter;
            var moveElve = false;
            (int, int) minCoord = (int.MaxValue, int.MaxValue);
            (int, int) maxCoord = (int.MinValue, int.MinValue);
            HashSet<(int, int)> WrongPositions = new();
            Dictionary<(int, int), (int, int)> ElvesNewPositions = new();
            foreach (var elve in Elves)
            {
                var newCoord = CalcNewPosition(elve);
                if (ElvesNewPositions.ContainsValue(newCoord))
                    WrongPositions.Add(newCoord);
                ElvesNewPositions.Add(elve, newCoord);
            }

            Elves.Clear();
            foreach (var elve in ElvesNewPositions)
            {
                var pos = WrongPositions.Contains(elve.Value) ? elve.Key : elve.Value;
                if (pos != elve.Key)
                    moveElve = true;
                Elves.Add(pos);
                if (pos.Item1 > maxCoord.Item1)
                    maxCoord.Item1 = pos.Item1;
                if (pos.Item2 > maxCoord.Item2)
                    maxCoord.Item2 = pos.Item2;
                if (pos.Item1 < minCoord.Item1)
                    minCoord.Item1 = pos.Item1;
                if (pos.Item2 < minCoord.Item2)
                    minCoord.Item2 = pos.Item2;
            }
            StartChecker = (StartChecker + 1) % CheckList.Count;
            return (((maxCoord.Item1 - minCoord.Item1 + 1) * (maxCoord.Item2 - minCoord.Item2 + 1)) - Elves.Count, moveElve);
        }

        (int, int) CalcNewPosition((int, int) elve)
        {
            if (DontWorry(elve)) return elve;
            for (var i = 0; i < CheckList.Count; i++)
            {
                var c = CheckList[(StartChecker + i) % CheckList.Count];
                var r = c(elve);
                if (r.Item1)
                    return (r.Item2, r.Item3);
            }
            return elve;
        }

        (bool, int, int) checkN((int, int) elve)
        {
            if (!(Elves.Contains((elve.Item1 - 1, elve.Item2 - 1)) ||
                  Elves.Contains((elve.Item1 + 0, elve.Item2 - 1)) ||
                  Elves.Contains((elve.Item1 + 1, elve.Item2 - 1)))) return (true, elve.Item1, elve.Item2 - 1);
            return (false, 0, 0);
        }

        (bool, int, int) checkS((int, int) elve)
        {
            if (!(Elves.Contains((elve.Item1 - 1, elve.Item2 + 1)) ||
                 Elves.Contains((elve.Item1 + 0, elve.Item2 + 1)) ||
                 Elves.Contains((elve.Item1 + 1, elve.Item2 + 1)))) return (true, elve.Item1, elve.Item2 + 1);
            return (false, 0, 0);
        }

        (bool, int, int) checkW((int, int) elve)
        {
            if (!(Elves.Contains((elve.Item1 - 1, elve.Item2 - 1)) ||
                  Elves.Contains((elve.Item1 - 1, elve.Item2 + 0)) ||
                  Elves.Contains((elve.Item1 - 1, elve.Item2 + 1)))) return (true, elve.Item1 - 1, elve.Item2);
            return (false, 0, 0);
        }

        (bool, int, int) checkE((int, int) elve)
        {
            if (!(Elves.Contains((elve.Item1 + 1, elve.Item2 - 1)) ||
                  Elves.Contains((elve.Item1 + 1, elve.Item2 + 0)) ||
                  Elves.Contains((elve.Item1 + 1, elve.Item2 + 1)))) return (true, elve.Item1 + 1, elve.Item2);
            return (false, 0, 0);
        }

        bool DontWorry((int, int) elve)
        {
            for (var x = elve.Item1 - 1; x <= elve.Item1 + 1; ++x)
                for (var y = elve.Item2 - 1; y <= elve.Item2 + 1; ++y)
                    if (!(x == elve.Item1 && y == elve.Item2) && Elves.Contains((x, y)))
                        return false;
            return true;
        }

        int StartChecker = 0;
        List<Func<(int, int), (bool, int, int)>> CheckList = new();

        HashSet<(int, int)> Elves = new();
        int RoundCounter = 0;
    }
}