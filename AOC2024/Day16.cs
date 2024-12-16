using AnyThings;
using System.Collections;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using Coord = (int x, int y);

namespace AOC2024
{
    internal class Day16 : DayPattern<(HashSet<Coord> wall, (Coord position, Coord direction) reindeer, Coord end)>
    {
        public override void Parse(string singleText)
        {
            data = new();
            data.wall = new();

            var map = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (int r = 0; r < map.Length; r++)
                for (int c = 0; c < map[r].Length; ++c)
                {
                    switch (map[r][c])
                    {
                        case 'S':
                            {
                                data.reindeer = ((c, r), (1,0));
                                break;
                            }
                        case '#':
                            {
                                data.wall.Add((c, r));
                                break;
                            }
                        case 'E':
                            {
                                data.end = (c, r);
                                break;
                            }
                    }
                }
        }

        public override string PartOne()
        {
            long result = 0;
            result = BFS();
            Show();
            return result.ToString();
        }

        long DFS()
        {
            long result = 0;
            if( DFS_Step(data.reindeer, new HashSet<Coord>(), ref result, true))
                return result;
            return -1;
        }

        long CurrentCoast = long.MaxValue;
        HashSet<Coord> CurrentPath = null;

        bool DFS_Step((Coord position, Coord direction) reindeer, HashSet<Coord> path, ref long coast, bool isNewCoord)
        {
            if (CurrentCoast != long.MaxValue && coast > CurrentCoast) return false;
            if (data.wall.Contains(reindeer.position)) return false;
            if (path.Contains(reindeer.position)) return false;
            if (data.end == reindeer.position)
            {
                if (coast < CurrentCoast)
                {
                    CurrentCoast = coast;
                    CurrentPath = path;
                }
                return true;
            }
            bool result = false;
            HashSet<Coord> nPath = new(path);
            nPath.Add(reindeer.position);
            long next_coast = coast + 1;
            if (DFS_Step(((reindeer.position.x + reindeer.direction.x, reindeer.position.y + reindeer.direction.y), reindeer.direction), nPath, ref next_coast, true))
            {
                result = true;
            }
            if (isNewCoord)
            {
                var next_coast2 = coast + 1000;
                if (DFS_Step((reindeer.position, Rotate(reindeer.direction, 1)), path, ref next_coast2, false))
                {
                    next_coast = result ? Math.Min(next_coast, next_coast2) : next_coast2;
                    result = true;
                }
                var next_coast3 = coast + 1000;
                if (DFS_Step((reindeer.position, Rotate(reindeer.direction, -1)), path, ref next_coast3, false))
                {
                    next_coast = result ? Math.Min(next_coast, next_coast3) : next_coast3;
                    result = true;
                }
            }
            if (result)
                coast = next_coast;
            return result;
        }

        long BFS()
        {
            Dictionary<Coord, long> path = new();
            Queue<(Coord position, Coord direction, long coast, int count)> next_ = new();
            next_.Enqueue((data.reindeer.position, data.reindeer.direction, 0, 1));
            while (next_.Count > 0)
                BFS_Step(next_.Dequeue(), path, next_);
            return path[data.end];
        }

        int SummaryBestCount = 0;

        void BFS_Step((Coord position, Coord direction, long coast, int count) current, Dictionary<Coord, long> path, Queue<(Coord position, Coord direction, long coast, int count)> next)
        {
            if (data.wall.Contains(current.position)) return;
            if (data.end == current.position)
            {
                if (path.ContainsKey(current.position))
                {
                    if (path[current.position] > current.coast)
                    {
                        path[current.position] = current.coast;
                        SummaryBestCount = current.count;
                    }
                    else if(path[current.position] == current.coast)
                        SummaryBestCount += current.count;
                }
                else
                {
                    path.Add(current.position, current.coast);
                    SummaryBestCount = current.count;
                }
                return;
            }

            if (path.ContainsKey(current.position))
            {
                if (path[current.position] > current.coast)
                    path[current.position] = current.coast;
                else return;
            }
            else
                path.Add(current.position, current.coast);
            next.Enqueue(((current.position.x + current.direction.x, current.position.y + current.direction.y), current.direction, current.coast + 1, current.count + 1));
            var new_direction = Rotate(current.direction, 1);
            next.Enqueue(((current.position.x + new_direction.x, current.position.y + new_direction.y), new_direction, current.coast + 1001, current.count + 1));
            new_direction = Rotate(current.direction, -1);
            next.Enqueue(((current.position.x + new_direction.x, current.position.y + new_direction.y), new_direction, current.coast + 1001, current.count + 1));
        }


        private (int x, int y) Rotate((int x, int y) direction, int v)
        {
            if (v > 0)
            {
                switch (direction)
                {
                    case (1, 0): return (0, 1);
                    case (0, 1): return (-1, 0);
                    case (-1, 0): return (0, -1);
                    case (0, -1): return (1, 0);
                }
            }
            switch (direction)
            {
                case (1, 0): return (0, -1);
                case (0, -1): return (-1, 0);
                case (-1, 0): return (0, 1);
                case (0, 1): return (1, 0);
            }
            return direction;
        }

        void Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            foreach (var w in data.wall)
            {
                Console.SetCursorPosition((int)w.x + 5, (int)w.y + 10);
                Console.Write('#');
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition((int)data.reindeer.position.x + 5, (int)data.reindeer.position.y + 10);
            Console.Write('@');
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition((int)data.end.x + 5, (int)data.end.y + 10);
            Console.Write('E');
            if (CurrentPath != null)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var p in CurrentPath)
                {
                    Console.SetCursorPosition((int)p.x + 5, (int)p.y + 10);
                    Console.Write('*');
                }
            }
            Console.SetCursorPosition(0, 30);
        }

        public override string PartTwo()
        {
            long result = 0;
            result = SummaryBestCount;
            return result.ToString();
        }
    }
}