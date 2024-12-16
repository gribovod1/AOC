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
            bestScore = result;
            return result.ToString();
        }

        long bestScore = long.MaxValue;

        long BFS()
        {
            Dictionary<Coord, long> path = new();
            Queue<(Coord position, Coord direction, long coast, int count)> next_ = new();
            next_.Enqueue((data.reindeer.position, data.reindeer.direction, 0, 1));
            while (next_.Count > 0)
                BFS_Step(next_.Dequeue(), path, next_);
            return path[data.end];
        }

        void BFS_Step((Coord position, Coord direction, long coast, int count) current, Dictionary<Coord, long> path, Queue<(Coord position, Coord direction, long coast, int count)> next)
        {
            if (data.wall.Contains(current.position)) return;
            if (data.end == current.position)
            {
                if (path.ContainsKey(current.position))
                {
                    if (path[current.position] > current.coast)
                        path[current.position] = current.coast;
                }
                else
                    path.Add(current.position, current.coast);
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

        long DFS()
        {
            DFS_Step(data.reindeer, new HashSet<Coord>(), 0, true);
            return BestTiles.Count;
        }

        HashSet<Coord> BestTiles = new();

        void DFS_Step((Coord position, Coord direction) reindeer, HashSet<Coord> path, long coast, bool isNewCoord)
        {
            if (coast > bestScore) return;
            if (data.wall.Contains(reindeer.position)) return;
            if (path.Contains(reindeer.position)) return;
            if (data.end == reindeer.position)
            {
                if (coast == bestScore)
                    foreach (var p in path)
                        BestTiles.Add(p);
                return;
            }
            HashSet<Coord> nPath = new(path);
            nPath.Add(reindeer.position);
            long next_coast = coast + 1;
            DFS_Step(((reindeer.position.x + reindeer.direction.x, reindeer.position.y + reindeer.direction.y), reindeer.direction), nPath, next_coast, true);
            if (isNewCoord)
            {
                var next_coast2 = coast + 1000;
                DFS_Step((reindeer.position, Rotate(reindeer.direction, 1)), path, next_coast2, false);
                var next_coast3 = coast + 1000;
                DFS_Step((reindeer.position, Rotate(reindeer.direction, -1)), path, next_coast3, false);
            }
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
            Console.SetCursorPosition(0, 30);
        }

        public override string PartTwo()
        {
            long result = 0;
            result = DFS();
            return result.ToString();
        }
    }
}