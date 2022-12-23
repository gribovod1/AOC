using AnyThings;

namespace AOC2022
{
    internal class Day22 : DayPattern<Dictionary<(int, int), char>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine + Environment.NewLine).ToList();
            data = new();
            // MAP
            var tMap = text[0].Split(Environment.NewLine);
            MapSize.Item2 = tMap.Length;
            MapSize.Item1 = tMap[0].Length;

            for (var y = 0; y < tMap.Length; y++)
            {
                if (tMap[y].Length > MapSize.Item1)
                    MapSize.Item1 = tMap[y].Length;
                for (var x = 0; x < tMap[y].Length; x++)
                    if (tMap[y][x] != ' ')
                        data.Add((x, y), tMap[y][x]);
            }
            // PATH
            var StepsText = text[1].Split(new char[] { 'R', 'L' });
            var rotateIndex = 0;
            var stepNum = 0;
            do
            {
                rotateIndex = NextRotateIndex(text[1], rotateIndex + 1);
                if (rotateIndex > 0)
                    Path.Add((int.Parse(StepsText[stepNum]), text[1][rotateIndex]));
                else
                    Path.Add((int.Parse(StepsText[stepNum]), ' '));
                ++stepNum;
            } while (rotateIndex > 0);
            // Set start position
            CurrentState = (0, 0, 0);
            var ss = FindOtherSide();
            CurrentState.Item1 = ss.Item1;
        }

        int GetNewDirection(int rotate, char turn)
        {
            if (turn == ' ')
                return rotate;
            return (turn == 'R' ? rotate + 5 : rotate + 3) % 4;
        }

        int NextRotateIndex(string source, int start)
        {
            var r = source.IndexOf('R', start);
            var l = source.IndexOf('L', start);
            if (l == -1) return r;
            if (r == -1) return l;
            return Math.Min(r, l);
        }

        public override string PartOne()
        {
            foreach (var p in Path)
                Step(p);
            return ((CurrentState.Item2 + 1) * 1000 + (CurrentState.Item1 + 1) * 4 + CurrentState.Item3).ToString();
        }

        public override string PartTwo()
        {
            CurrentState = (0, 0, 0);
            var ss = FindOtherSide();
            CurrentState.Item1 = ss.Item1;
            foreach (var p in Path)
            {
                Step(p, true);
            }
            return ((CurrentState.Item2 + 1) * 1000 + (CurrentState.Item1 + 1) * 4 + CurrentState.Item3).ToString();
        }

        private void Step((int, char) p, bool isCube = false)
        {
            try
            {
                var x = CurrentState.Item1;
                var y = CurrentState.Item2;
                var r = CurrentState.Item3;
                var steps = p.Item1;
                for (var s = 0; s < steps; s++)
                {
                    switch (CurrentState.Item3)
                    {
                        case 0:
                            {
                                ++x;
                                break;
                            }
                        case 1:
                            {
                                ++y;
                                break;
                            }
                        case 2:
                            {
                                --x;
                                break;
                            }
                        case 3:
                            {
                                --y;
                                break;
                            }
                    }
                    if (!data.ContainsKey((x, y)))
                    {
                        (x, y, r) = isCube ? FindCubeSide() : FindOtherSide();
                        var pos = Console.GetCursorPosition();
                         Console.SetCursorPosition(CurrentState.Item1 + 2, CurrentState.Item2 + 7);
                       var col = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        switch (CurrentState.Item3)
                        {
                            case 0: { Console.Write(">"); break; }
                            case 1: { Console.Write("v"); break; }
                            case 2: { Console.Write("<"); break; }
                            case 3: { Console.Write("^"); break; }
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(x + 2, y + 7);
                        switch (r)
                        {
                            case 0: { Console.Write(">"); break; }
                            case 1: { Console.Write("v"); break; }
                            case 2: { Console.Write("<"); break; }
                            case 3: { Console.Write("^"); break; }
                        }
                        Console.ForegroundColor = col;
                        Console.SetCursorPosition(pos.Left, pos.Top);
                    }
                    if (data[(x, y)] == '#')
                        return;
                    CurrentState.Item1 = x;
                    CurrentState.Item2 = y;
                    CurrentState.Item3 = r;
                }
            }
            finally
            {
                CurrentState.Item3 = GetNewDirection(CurrentState.Item3, p.Item2);
            }
        }

        private (int, int, int) FindCubeSide()
        {
            if (CurrentState.Item1 >= 50 && CurrentState.Item1 < 100 && CurrentState.Item2 >= 0 && CurrentState.Item2 < 50)
            {
                // Side 1
                switch (CurrentState.Item3)
                {
                    case 2: return (0, 150 - CurrentState.Item2, 0);
                    case 3: return (0, CurrentState.Item1 + 100, 0);
                }
            }
            else
            if (CurrentState.Item1 >= 100 && CurrentState.Item1 < 150 && CurrentState.Item2 >= 0 && CurrentState.Item2 < 50)
            {
                // Side 2
                switch (CurrentState.Item3)
                {
                    case 0: return (99, 150 - CurrentState.Item2, 2);
                    case 1: return (99, CurrentState.Item1 - 50, 2);
                    case 3: return (CurrentState.Item1 - 100, 199, 3);
                }
            }
            else
            if (CurrentState.Item1 >= 50 && CurrentState.Item1 < 100 && CurrentState.Item2 >= 50 && CurrentState.Item2 < 100)
            {
                // Side 3
                switch (CurrentState.Item3)
                {
                    case 0: return (CurrentState.Item2 + 50, 49, 3);
                    case 2: return (CurrentState.Item2 - 50, 100, 1);
                }
            }
            else
            if (CurrentState.Item1 >= 0 && CurrentState.Item1 < 50 && CurrentState.Item2 >= 100 && CurrentState.Item2 < 150)
            {
                // Side 4
                switch (CurrentState.Item3)
                {
                    case 2: return (50, 150 - CurrentState.Item2, 0);
                    case 3: return (50, CurrentState.Item1 + 50, 0);
                }
            }
            else
            if (CurrentState.Item1 >= 50 && CurrentState.Item1 < 100 && CurrentState.Item2 >= 100 && CurrentState.Item2 < 150)
            {
                // Side 5
                switch (CurrentState.Item3)
                {
                    case 0: return (149, 150 - CurrentState.Item2, 2);
                    case 1: return (49, CurrentState.Item1 + 100, 2);
                }
            }
            else
            if (CurrentState.Item1 >= 0 && CurrentState.Item1 < 50 && CurrentState.Item2 >= 150 && CurrentState.Item2 < 200)
            {
                // Side 6
                switch (CurrentState.Item3)
                {
                    case 0: return (CurrentState.Item2 - 100, 149, 3);
                    case 1: return (CurrentState.Item1 + 100, 0, 1);
                    case 2: return (CurrentState.Item2 - 100, 0, 1);
                }
            }
            throw new Exception("THIS IS IMPOSSIBLE!");
        }

        (int, int, int) FindOtherSide()
        {
            var dX = 0;
            var dY = 0;
            var x = CurrentState.Item1;
            var y = CurrentState.Item2;
            var r = CurrentState.Item3;
            var steps = 0;
            switch (CurrentState.Item3)
            {
                case 0:
                    {
                        dX = 1;
                        dY = 0;
                        x = 0;
                        steps = MapSize.Item1;
                        break;
                    }
                case 1:
                    {
                        dX = 0;
                        dY = 1;
                        y = 0;
                        steps = MapSize.Item2;
                        break;
                    }
                case 2:
                    {
                        dX = -1;
                        dY = 0;
                        x = MapSize.Item1;
                        steps = MapSize.Item1;
                        break;
                    }
                case 3:
                    {
                        dX = 0;
                        dY = -1;
                        y = MapSize.Item2;
                        steps = MapSize.Item2;
                        break;
                    }
            }
            for (var s = 0; s < steps; s++)
            {
                if (data.ContainsKey((x, y)))
                    break;
                y += dY;
                x += dX;
            }
            return (x, y, r);
        }


        (int, int, int) CurrentState;
        (int, int) MapSize;

        List<( int, char)> Path = new List<(int, char)>();
    }
}