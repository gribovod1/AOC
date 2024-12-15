using AnyThings;
using Coord = (long x, long y);

namespace AOC2024
{
    internal class Day15 : DayPattern<(HashSet<Coord> wall, HashSet<Coord> boxes, Coord robot, string program)>
    {
        public override void Parse(string singleText)
        {
            string[] mapAndProgram = singleText.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            data.wall = new();
            data.boxes = new();

            var map = mapAndProgram[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (int r = 0; r < map.Length; r++)
                for (int c = 0; c < map[r].Length; ++c)
                {
                    switch (map[r][c])
                    {
                        case '@':
                            {
                                data.robot = (c, r);
                                break;
                            }
                        case '#':
                            {
                                data.wall.Add((c, r));
                                break;
                            }
                        case 'O':
                            {
                                data.boxes.Add((c, r));
                                break;
                            }
                    }
                }

            data.program = mapAndProgram[1].Replace(Environment.NewLine, "");
        }

        public override string PartOne()
        {
            long result = 0;
            for (int c = 0; c < data.program.Length; ++c)
            {
                switch (data.program[c])
                {
                    case '>':
                        {
                            Move(1, 0);
                            break;
                        }
                    case 'v':
                        {
                            Move(0, 1);
                            break;
                        }
                    case '<':
                        {
                            Move(-1, 0);
                            break;
                        }
                    case '^':
                        {
                            Move(0, -1);
                            break;
                        }
                }
                //     Show();
                //     Console.ReadKey();
            }
            var boxes = data.boxes.ToList();
            for (int b = 0; b < boxes.Count; ++b)
                result += boxes[b].x + boxes[b].y * 100;

            return result.ToString();
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
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var b in data.boxes)
            {
                Console.SetCursorPosition((int)b.x + 5, (int)b.y + 10);
                Console.Write("[]");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition((int)data.robot.x + 5, (int)data.robot.y + 10);
            Console.Write('@');

            Console.SetCursorPosition(0, 30);
        }

        private void Move(int dx, int dy)
        {
            if (data.wall.Contains((data.robot.x + dx, data.robot.y + dy))) return;
            if (data.boxes.Contains((data.robot.x + dx, data.robot.y + dy)) &&
                !MoveBox((data.robot.x + dx, data.robot.y + dy), dx, dy)) return;
            data.robot = (data.robot.x + dx, data.robot.y + dy);
        }

        bool MoveBox(Coord box, int dx, int dy)
        {
            if (data.wall.Contains((box.x + dx, box.y + dy))) return false;
            if (data.boxes.Contains((box.x + dx, box.y + dy)) && !MoveBox((box.x + dx, box.y + dy), dx, dy)) return false;
            data.boxes.Remove(box);
            data.boxes.Add((box.x + dx, box.y + dy));
            return true;
        }

        public override string PartTwo()
        {
            TwoParse(Puzzle);
            long result = 0;
         //   Show();
         //   Console.ReadKey();
            for (int c = 0; c < data.program.Length; ++c)
            {
                switch (data.program[c])
                {
                    case '>':
                        {
                            Move2(1, 0);
                            break;
                        }
                    case 'v':
                        {
                            Move2(0, 1);
                            break;
                        }
                    case '<':
                        {
                            Move2(-1, 0);
                            break;
                        }
                    case '^':
                        {
                            Move2(0, -1);
                            break;
                        }
                }
                //     Show();
                //     Console.ReadKey();
            }
           //   Show();
           //   Console.ReadKey();
            var boxes = data.boxes.ToList();
            for (int b = 0; b < boxes.Count; ++b)
                result += boxes[b].x + boxes[b].y * 100;
            return result.ToString();
        }

        private void Move2(int dx, int dy)
        {
            if (data.wall.Contains((data.robot.x + dx, data.robot.y + dy))) return;
            if (dx != 0)
            {
                if (dx == 1 && data.boxes.Contains((data.robot.x + dx, data.robot.y + dy)))
                {
                    if (!CheckMoveBox2((data.robot.x + dx, data.robot.y + dy), dx, dy))
                        return;
                    MoveBox2((data.robot.x + dx, data.robot.y + dy), dx, dy);
                }
                if (dx == -1 && data.boxes.Contains((data.robot.x + 2 * dx, data.robot.y + dy)))
                {
                    if (!CheckMoveBox2((data.robot.x + 2 * dx, data.robot.y + dy), dx, dy))
                        return;
                    MoveBox2((data.robot.x + 2 * dx, data.robot.y + dy), dx, dy);
                }
            }
            else
            {
                if (data.boxes.Contains((data.robot.x + dx, data.robot.y + dy)))
                {
                    if (!CheckMoveBox2((data.robot.x + dx, data.robot.y + dy), dx, dy)) return;
                    MoveBox2((data.robot.x + dx, data.robot.y + dy), dx, dy);
                }
                else if (data.boxes.Contains((data.robot.x + dx - 1, data.robot.y + dy)))
                {
                    if (!CheckMoveBox2((data.robot.x + dx - 1, data.robot.y + dy), dx, dy)) return;
                    MoveBox2((data.robot.x + dx - 1, data.robot.y + dy), dx, dy);
                }
            }
            data.robot = (data.robot.x + dx, data.robot.y + dy);
        }

        bool CheckMoveBox2(Coord box, int dx, int dy)
        {
            if (dx != 0)
            {
                if (dx == 1 && data.wall.Contains((box.x + 2 * dx, box.y))) return false;
                if (dx == -1 && data.wall.Contains((box.x + dx, box.y))) return false;
                if (data.boxes.Contains((box.x + 2 * dx, box.y)) &&
                    !CheckMoveBox2((box.x + 2 * dx, box.y), dx, dy)) return false;
            }
            else
            {
                if (data.wall.Contains((box.x, box.y + dy))) return false;
                if (data.wall.Contains((box.x + 1, box.y + dy))) return false;
                bool result = true;
                if (data.boxes.Contains((box.x - 1, box.y + dy)))
                    result &= CheckMoveBox2((box.x - 1, box.y + dy), dx, dy);
                if (data.boxes.Contains((box.x, box.y + dy)))
                    result &= CheckMoveBox2((box.x, box.y + dy), dx, dy);
                if (data.boxes.Contains((box.x + 1, box.y + dy)))
                    result &= CheckMoveBox2((box.x + 1, box.y + dy), dx, dy);
                return result;
            }
            return true;
        }

        void MoveBox2(Coord box, int dx, int dy)
        {
            if (dx != 0)
            {
                if (data.boxes.Contains((box.x + 2 * dx, box.y)))
                    MoveBox2((box.x + 2 * dx, box.y), dx, dy);
            }
            else
            {
                if (data.boxes.Contains((box.x - 1, box.y + dy)))
                    MoveBox2((box.x - 1, box.y + dy), dx, dy);
                if (data.boxes.Contains((box.x, box.y + dy)))
                    MoveBox2((box.x, box.y + dy), dx, dy);
                if (data.boxes.Contains((box.x + 1, box.y + dy)))
                    MoveBox2((box.x + 1, box.y + dy), dx, dy);
            }
            data.boxes.Remove(box);
            data.boxes.Add((box.x + dx, box.y + dy));
        }

        private void TwoParse(string puzzle)
        {
            string[] mapAndProgram = puzzle.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new();
            data.wall = new();
            data.boxes = new();

            var map = mapAndProgram[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (int r = 0; r < map.Length; r++)
                for (int c = 0; c < map[r].Length; ++c)
                {
                    switch (map[r][c])
                    {
                        case '@':
                            {
                                data.robot = (c * 2, r);
                                break;
                            }
                        case '#':
                            {
                                data.wall.Add((c * 2, r));
                                data.wall.Add((c * 2 + 1, r));
                                break;
                            }
                        case 'O':
                            {
                                data.boxes.Add((c * 2, r));
                                break;
                            }
                    }
                }

            data.program = mapAndProgram[1].Replace(Environment.NewLine, "");
        }
    }
}