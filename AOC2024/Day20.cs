using AnyThings;
using System.Diagnostics;
using Coord = (int x, int y);

namespace AOC2024
{
    internal class Day20 : DayPattern<(HashSet<Coord> trace, Coord start, Coord end)>
    {
        public override void Parse(string singleText)
        {
            data = new();
            data.trace = new();

            var map = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (int r = 0; r < map.Length; r++)
                for (int c = 0; c < map[r].Length; ++c)
                {
                    switch (map[r][c])
                    {
                        case 'S':
                            {
                                data.start = (c, r);
                                data.trace.Add((c, r));
                                break;
                            }
                        case '.':
                            {
                                data.trace.Add((c, r));
                                break;
                            }
                        case 'E':
                            {
                                data.end = (c, r);
                                data.trace.Add((c, r));
                                break;
                            }
                    }
                }
        }

        public override string PartOne()
        {
            /*
             Берём точку трассы и и генерируем 8 точек, которые должны изменить маршрут.
            Добавляем эти точки в общий список и делаем волну, считаем сколько заняло времени, если меньше на 100 ходов, то ++             
             */
            long TimeBeforeCheat = BFS();
            long result = 0;
            var ts = data.trace.ToList();
            Dictionary<long, List<HashSet<Coord>>> traces = new();
            foreach(var trace in ts)
            {
                if (TimeWithCheat((trace.x + 1, trace.y ), (trace.x + 2, trace.y), traces) == TimeBeforeCheat - 8) 
                    ++result;
               // if (TimeWithCheat((trace.x - 1, trace.y ), (trace.x - 2, trace.y )) <= TimeBeforeCheat - 20) ++result;
                if (TimeWithCheat((trace.x , trace.y + 1), (trace.x, trace.y + 2), traces) == TimeBeforeCheat -8) 
                    ++result;
               // if (TimeWithCheat((trace.x , trace.y - 1), (trace.x , trace.y - 2)) <= TimeBeforeCheat - 20) ++result;
            }
            return result.ToString();
        }

        long TimeWithCheat(Coord p1, Coord p2, Dictionary<long, List<HashSet<Coord>>> traces)
        {
            bool needDeleteP1 = !data.trace.Contains(p1);
            bool needDeleteP2 = !data.trace.Contains(p2);
            data.trace.Add(p1);
            data.trace.Add(p2);
            var hash = TraceHash();
            long result = long.MaxValue;
            if (traces.ContainsKey(hash))
            {
                if (!ExistTrace(traces[hash], data.trace))
                {
                    result = BFS();
                    traces[hash].Add(new(data.trace));
                }
            } else
            {
                result = BFS();
                traces.Add(hash, new());
                traces[hash].Add(new(data.trace));
            }
            if (needDeleteP1) data.trace.Remove(p1);
            if (needDeleteP2) data.trace.Remove(p2);
            return result;
        }

        bool ExistTrace(List<HashSet<Coord>> traces, HashSet<Coord> trace)
        {
            foreach (var t in traces)
                if (TraceCompare(trace, t)) return true;
            return false;
        }

        bool TraceCompare(HashSet<Coord> trace1, HashSet<Coord> trace2)
        {
            if (trace1.Count != trace2.Count) 
                return false;
            var l1 = trace1.ToList();
            l1.Sort();
            var l2 = trace2.ToList();
            l2.Sort();
            for(int i =0;i < l1.Count; i++) 
                if (l1[i] != l2[i])
                    return false;
            return true;
        }

        long TraceHash()
        {
            long result = 0;
            var ts = data.trace.ToList();
            ts.Sort();
            foreach (var trace in ts)
            {
                result ^= trace.GetHashCode();// (trace.x.GetHashCode() + trace.y.GetHashCode());
            }
 //           result ^= (trace.x + trace.y * 15);
            return result;
        }

        long BFS()
        {
            Queue<(Coord position, long time)> next_ = new();
            next_.Enqueue((data.start, 0));
            Dictionary<Coord, long> path = new();
            while (next_.Count > 0)
                BFS_Step(next_.Dequeue(), path, next_);
            return path[data.end];
        }

        void BFS_Step((Coord position, long time) current, Dictionary<Coord, long> path, Queue<(Coord position, long time)> next)
        {
            if (!data.trace.Contains(current.position)) return;
            if (data.end == current.position)
            {
                if (path.ContainsKey(current.position))
                {
                    if (path[current.position] > current.time)
                        path[current.position] = current.time;
                }
                else
                    path.Add(current.position, current.time);
                return;
            }

            if (path.ContainsKey(current.position))
            {
                if (path[current.position] > current.time)
                    path[current.position] = current.time;
                else return;
            }
            else
                path.Add(current.position, current.time);
            next.Enqueue(((current.position.x + 1, current.position.y), current.time + 1));
            next.Enqueue(((current.position.x - 1, current.position.y), current.time + 1));
            next.Enqueue(((current.position.x, current.position.y + 1), current.time + 1));
            next.Enqueue(((current.position.x, current.position.y - 1), current.time + 1));
        }



        public override string PartTwo()
        {
            long result = 0;
            return result.ToString();
        }
    }
}