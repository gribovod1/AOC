using AnyThings;

namespace AOC2022
{
    internal class Day17 : DayPattern<List<string>>
    {
        public override void Parse(string singleText)
        {
            foreach (var c in singleText)
                Streams.Add(c);

            StonePatterns.Add(new List<(int, int)>() { (0, 0), (1, 0), (2, 0), (3, 0) });
            StonePatterns.Add(new List<(int, int)>() { (1, 0), (0, 1), (1, 1), (1, 2), (2, 1) });
            StonePatterns.Add(new List<(int, int)>() { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) });
            StonePatterns.Add(new List<(int, int)>() { (0, 0), (0, 1), (0, 2), (0, 3) });
            StonePatterns.Add(new List<(int, int)>() { (0, 0), (1, 0), (0, 1), (1, 1) });
        }

        public override string PartOne()
        {
            for (var i = 0; i < 2022; ++i)
            {
                NewStone();
                do
                {
                    Move(false);
                }
                while (Move(true));
                foreach (var c in CurrentStone)
                {
                    if (c.Item2 > YCoordMax)
                        YCoordMax = c.Item2;
                    Tower.Add(c);
                }
        /*        if (BrickStreamHeight.ContainsKey((BrickNumber, StreamNumber)))
                {
                    Console.WriteLine($"Current tower size = {YCoordMax + 1} saved: {BrickStreamHeight[(BrickNumber, StreamNumber)]} Count: {BrickCounter} (BN: {BrickNumber} SN: {StreamNumber}");
                    return $"Tower size: {(YCoordMax + 1)}";
                }
                else
                {
                    BrickStreamHeight.Add((BrickNumber, StreamNumber), YCoordMax + 1);
                }*/
            }

            return $"Tower size: {(YCoordMax + 1)}";
        }

        Dictionary<(int, int),int> BrickStreamHeight = new ();

        int YCoordMax = 0;

        public override string PartTwo()
        {
            return 0.ToString();
        }

        int BrickNumber = 0;
        int StreamNumber = 0;
        UInt64 BrickCounter = 0;

        void NewStone()
        {
            ++BrickCounter;
            var minY = YCoordMax + 3;
            BrickNumber = (BrickNumber + 1) % StonePatterns.Count;
            var c = StonePatterns[BrickNumber];
            CurrentStone.Clear();
            for (var i = 0; i < c.Count; ++i)
                CurrentStone.Add((2 + c[i].Item1, minY + c[i].Item2));
        }

        bool Move(bool fall)
        {
            if (fall)
            {
                if (CheckMove(CurrentStone, 0, -1))
                {
                    for (var i = 0; i < CurrentStone.Count; ++i)
                        CurrentStone[i] = (CurrentStone[i].Item1, CurrentStone[i].Item2 - 1);
                    return true;
                }
            }
            else
            {
                StreamNumber = (StreamNumber + 1) % Streams.Count;
                var c = Streams[StreamNumber];
                var dX = c == '<' ? -1 : 1;
                if (CheckMove(CurrentStone, dX, 0))
                {
                    for (var i = 0; i < CurrentStone.Count; ++i)
                        CurrentStone[i] = (CurrentStone[i].Item1 + dX, CurrentStone[i].Item2);
                    return true;
                }
            }
            return false;
        }

        bool CheckMove(List<(int, int)> stone, int dx, int dy)
        {
            foreach (var c in stone)
            {
                if (c.Item1 + dx < 0 || c.Item1 + dx > 6 || c.Item2 + dy < 0)
                    return false;
                if (Tower.Contains((c.Item1 + dx, c.Item2 + dy)))
                    return false;
            }
            return true;
        }

        int GetTower()
        {
            if (Tower.Count == 0) return 0;
            var max = 0;
            foreach (var c in Tower)
            {
                if (c.Item2 > max)
                    max = c.Item2;
            }
            return max + 1;
        }

        List<char> Streams = new ();
        List<List<(int, int)>> StonePatterns = new ();
        HashSet<(int, int)> Tower = new HashSet<(int, int)>();
        List<(int, int)> CurrentStone = new List<(int, int)>();
    }
}