using AnyThings;
using System.Linq;
using System.Text;

namespace AOC2023
{
    internal class Day22 : DayPattern<List<List<(int x, int y, int z)>>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (int row = 0; row < text.Length; row++)
            {
                List<(int x, int y, int z)> brick = new();
                var coords = text[row].Split(new char[] { '~', ',' }, StringSplitOptions.RemoveEmptyEntries);
                (int x, int y, int z) start = (int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
                (int x, int y, int z) end = (int.Parse(coords[3]), int.Parse(coords[4]), int.Parse(coords[5]));
                var dx = end.x != start.x ? 1 : 0;
                var dy = end.y != start.y ? 1 : 0;
                var dz = end.z != start.z ? 1 : 0;
                while (start != end)
                {
                    brick.Add(start);
                    start = (start.x + dx, start.y + dy, start.z + dz);
                }
                brick.Add(start);
                brick.Sort(SortCoords);
                data.Add(brick);
            }
            data.Sort(SortBricks);
        }

        int SortBricks(List<(int x, int y, int z)> x, List<(int x, int y, int z)> y)
        {
            return x[0].z.CompareTo(y[0].z);
        }

        int SortCoords((int x, int y, int z) x, (int x, int y, int z) y)
        {
            return x.z.CompareTo(y.z);
        }

        public override string PartOne()
        {
            Int64 result = 0;
            Move();
            data.Sort(SortBricks);
            Dictionary<(int x, int y, int z),int> coords = GetCoords();

            for (var b =0;b<data.Count;++b)
            {
                int c = 0;
                for (; c < data[b].Count; ++c)
                {
                    if (coords.TryGetValue((data[b][c].x, data[b][c].y, data[b][c].z + 1), out int brickUp) &&
                        brickUp != b && GetCountDownBricks(coords, brickUp) <= 1)
                        break;
                }
                if (c == data[b].Count)
                    ++result;
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            /* 
             */
            Int64 result = 0;
            Dictionary<(int x, int y, int z), int> coords = GetCoords();
            Dictionary<int, Int64> BreakCounter = new();

            for (var b = data.Count - 1; b >= 0; --b)
            {
                result += GetCountBreakBricks(coords, BreakCounter, b);
            }
            return result.ToString();
        }

        Int64 GetCountBreakBricks(Dictionary<(int x, int y, int z), int> coords, Dictionary<int, Int64> BreakCounter, int DownBrick)
        {
            Int64 result = 0;
            HashSet<int> accepted = new();
            for(int c = 0; c < data[DownBrick].Count; ++c) {
                if (coords.TryGetValue((data[DownBrick][c].x, data[DownBrick][c].y, data[DownBrick][c].z + 1), out int up) &&
                    up != DownBrick && accepted.Add(up))
                {
                    if (BreakCounter.ContainsKey(up))
                        result += BreakCounter[up];
                    else
                        ++result;
                }
            }
            BreakCounter.Add(DownBrick, result);
            return result;
        }

  /*      int GetCountDownBricks(Dictionary<(int x, int y, int z), int> coords, int DownBrick)
        {
            Dictionary<(int x, int y, int z), int> BreakCoords = new();
            HashSet<int> BreakedBricks = new();
            BreakedBricks.Add(DownBrick);
            for (int c = 0; c < data[DownBrick].Count; ++c)
                BreakCoords.Add(data[DownBrick][c], DownBrick);
            bool added = true;
            do
            {
                foreach(var c in BreakCoords)
                {
                    int brickUp;
                    if (coords.TryGetValue((c.Key.x, c.Key.y, c.Key.z + 1), out brickUp) && brickDown != UpBrick)
                        result.Add(brickDown);

                }
            } while (added);
            HashSet<int> result = new();
            for (int cUp = 0; cUp < data[UpBrick].Count; ++cUp)
            {
                int brickDown;
                if (coords.TryGetValue((data[UpBrick][cUp].x, data[UpBrick][cUp].y, data[UpBrick][cUp].z - 1), out brickDown) && brickDown != UpBrick)
                    result.Add(brickDown);
            }
            return result.Count;
        }*/

        int GetCountDownBricks(Dictionary<(int x, int y, int z), int> coords, int UpBrick)
        {
            HashSet<int> result = new();
            for (int cUp = 0; cUp < data[UpBrick].Count; ++cUp)
            {
                int brickDown;
                if (coords.TryGetValue((data[UpBrick][cUp].x, data[UpBrick][cUp].y, data[UpBrick][cUp].z - 1), out brickDown) && brickDown != UpBrick)
                    result.Add(brickDown);
            }
            return result.Count;
        }

        bool Move()
        {
            bool result = false;
            do
            {
                result = false;
                var coords = GetCoords();
                for (int b = 0; b < data.Count; ++b)
                    if (data[b][0].z > 1)
                    {
                        int c = 0;
                        for (; c < data[b].Count; ++c)
                        {
                            int brickDown;
                            if (coords.TryGetValue((data[b][c].x, data[b][c].y, data[b][c].z - 1), out brickDown) && (brickDown != b))
                                break;
                        }
                        if (c == data[b].Count)
                            for (int nc = 0; nc < data[b].Count; ++nc)
                            {
                                coords.Remove(data[b][nc]);
                                data[b][nc] = (data[b][nc].x, data[b][nc].y, data[b][nc].z - 1);
                                coords.Add(data[b][nc], b);
                                result = true;
                            }
                    }
            }
            while (result);
            return result;
        }

        Dictionary<(int x, int y, int z), int> GetCoords()
        {
            Dictionary<(int x, int y, int z), int> coords = new();
            for (var b = 0; b < data.Count; ++b)
                foreach (var c in data[b])
                    coords.Add(c, b);
            return coords;
        }
    }
}