using System.Text;
using AnyThings;

namespace AOC2023
{
    internal class Day10 : DayPattern<(char tube, int label)[,]>
    {
        (int x, int y) Start = (0, 0);
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine);
            data = new (char tube, int label)[text[0].Length, text.Length];
            for (int y = 0; y < data.GetLength(1); ++y)
            {
                for (int x = 0; x < data.GetLength(0); ++x)
                {
                    char tube = text[y][x];
                    data[x, y] = (tube, 0);
                    if (tube == 'S')
                    {
                        Start = (x, y);
                    }
                }
            }
        }

        public override string PartOne()
        {
            /*
             */
            Int64 result = 0;
            (int x, int y)[] coord = FindNextCoords(Start);
            (int x, int y)[] PrevCoord = new (int x, int y)[2];
            PrevCoord[0] = Start;
            PrevCoord[1] = Start;
            data[Start.x, Start.y].label = 1;
            data[coord[0].x, coord[0].y].label = 1;
            data[coord[1].x, coord[1].y].label = 1;
            int currentStep = 1;
            while (coord[0].x != coord[1].x || coord[0].y != coord[1].y)
            {
                var t = NextCoord(coord[0], PrevCoord[0]);
                PrevCoord[0] = coord[0];
                coord[0] = t;
                data[coord[0].x, coord[0].y].label = 1;
                if (coord[0].x == coord[1].x && coord[0].y == coord[1].y) break;
                t = NextCoord(coord[1], PrevCoord[1]);
                PrevCoord[1] = coord[1];
                coord[1] = t;
                data[coord[1].x, coord[1].y].label = 1;
                ++currentStep;
            }
            result = currentStep;
            return result.ToString();
        }

        private (int x, int y) NextCoord((int x, int y) coord, (int x, int y) prev)
        {
            (int x, int y) result = coord;
            char tube = data[coord.x, coord.y].tube;
            switch (tube)
            {
                case '|':
                    {
                        if (prev.y < coord.y)
                            result = (coord.x, coord.y + 1);
                        else
                            result = (coord.x, coord.y - 1);
                        break;
                    }
                case '-':
                    {
                        if (prev.x < coord.x)
                            result = (coord.x + 1, coord.y);
                        else
                            result = (coord.x - 1, coord.y);
                        break;
                    }
                case '7':
                    {
                        if (prev.x < coord.x)
                            result = (coord.x, coord.y + 1);
                        else
                            result = (coord.x - 1, coord.y);
                        break;
                    }
                case 'F':
                    {
                        if (prev.y > coord.y)
                            result = (coord.x + 1, coord.y);
                        else
                            result = (coord.x, coord.y + 1);
                        break;
                    }
                case 'J':
                    {
                        if (prev.x < coord.x)
                            result = (coord.x, coord.y - 1);
                        else
                            result = (coord.x - 1, coord.y);
                        break;
                    }
                case 'L':
                    {
                        if (prev.x > coord.x)
                            result = (coord.x, coord.y - 1);
                        else
                            result = (coord.x + 1, coord.y);
                        break;
                    }
            }
            return result;
        }

        private (int x, int y)[] FindNextCoords((int x, int y) start)
        {
            (int x, int y)[] result = new (int x, int y)[2];
            int current = 0;
            char tube = GetSymbol(start.x, start.y - 1);
            if (tube == 'F' || tube == '7' || tube == '|')
            {
                result[current] = (start.x, start.y - 1);
                ++current;
            }
            tube = GetSymbol(start.x, start.y + 1);
            if (tube == 'L' || tube == 'J' || tube == '|')
            {
                result[current] = (start.x, start.y + 1);
                ++current;
            }
            tube = GetSymbol(start.x - 1, start.y);
            if (tube == 'L' || tube == 'F' || tube == '-')
            {
                result[current] = (start.x - 1, start.y);
                ++current;
            }
            tube = GetSymbol(start.x + 1, start.y);
            if (tube == 'J' || tube == '7' || tube == '-')
            {
                result[current] = (start.x + 1, start.y);
                ++current;
            }
            return result;
        }

        char GetSymbol(int x, int y)
        {
            if (x < 0 || x >= data.GetLength(0) || y < 0 || y >= data.GetLength(1)) return '.';
            return data[x, y].tube;
        }

        public override string PartTwo()
        {
            /*
            */
            Int64 result = 0;

            // Проверяем, в каком направлении правая сторона окажется внутренней
            (int x, int y)[] coord = FindNextCoords(Start);
            (int x, int y)[] PrevCoord = new (int x, int y)[2];
            PrevCoord[0] = Start;
            PrevCoord[1] = Start;
            int innerDirection = 0;
            // Отмечаем внутренние области кольца
            coord = FindNextCoords(Start);
            PrevCoord = new (int x, int y)[2];
            PrevCoord[0] = Start;
            PrevCoord[1] = Start;
            while (coord[innerDirection].x != Start.x || coord[innerDirection].y != Start.y)
            {
                var t = NextCoord(coord[innerDirection], PrevCoord[innerDirection]);
                PrevCoord[innerDirection] = coord[innerDirection];
                coord[innerDirection] = t;
                CheckInnerLoop(t, PrevCoord[innerDirection]);
            }
            // Расширяем волновым алгоритмом все найденные внутренние области

            for (int y = 0; y < data.GetLength(1); ++y)
            {
                for (int x = 0; x < data.GetLength(0); ++x)
                {
                    if (data[x, y].label == 2)
                    {
                        FillArea(x, y);
                    }
                }
            }

            // сохраним карту для отладки
            StringBuilder text = new();
            for (int y = 0; y < data.GetLength(1); ++y)
            {
                for (int x = 0; x < data.GetLength(0); ++x)
                {
                    var p = data[x, y];
                    var t = p.tube;
                    if (t == 'S')
                    {

                    }
                    else
                    if (p.label == 1)
                    {
                        t = '#';
                    }
                    else if (p.label >= 2)
                    {
                        t = 'O';
                    }
                    else
                    {
                        t = ' ';
                    }
                    text.Append(t);
                }
                text.AppendLine();
            }
            File.WriteAllText($"Data/{this.GetType().Name}_result.txt", text.ToString());

            // Считаем области, помеченные как внутренние
            for (int x = 0; x < data.GetLength(0); ++x)
                for (int y = 0; y < data.GetLength(1); ++y)
                    if (data[x, y].label >= 2)
                        ++result;
            return result.ToString();
        }

        private void FillArea(int x, int y)
        {
            if (!CheckInMap((x, y)) || data[x, y].label == 3 || data[x, y].label == 1)
                return;
            data[x, y].label = 3;
            FillArea(x - 1, y);
            FillArea(x + 1, y);
            FillArea(x, y - 1);
            FillArea(x, y + 1);
        }

        private bool CheckCoordinates((int x, int y) coord, (int x, int y) prev)
        {
            switch (data[coord.x, coord.y].tube)
            {
                case '|':
                    {
                        if (prev.y < coord.y)
                            if (!CheckInMap((coord.x - 1, coord.y))) return false;
                        if (!CheckInMap((coord.x + 1, coord.y))) return false;
                        return true;
                    }
                case '-':
                    {
                        if (prev.x < coord.x)
                            if (!CheckInMap((coord.x, coord.y + 1))) return false;
                        if (!CheckInMap((coord.x, coord.y - 1))) return false;
                        return true;
                    }
                case '7':
                    {
                        if (prev.x < coord.x)
                            return true;
                        if (!CheckInMap((coord.x + 1, coord.y))) return false;
                        if (!CheckInMap((coord.x + 1, coord.y - 1))) return false;
                        if (!CheckInMap((coord.x, coord.y - 1))) return false;
                        return true;
                    }
                case 'F':
                    {
                        if (prev.y > coord.y)
                            return true;
                        if (!CheckInMap((coord.x, coord.y - 1))) return false;
                        if (!CheckInMap((coord.x - 1, coord.y - 1))) return false;
                        if (!CheckInMap((coord.x - 1, coord.y))) return false;
                        return true;
                    }
                case 'J':
                    {
                        if (prev.y < coord.y)
                            return true;
                        if (!CheckInMap((coord.x, coord.y + 1))) return false;
                        if (!CheckInMap((coord.x + 1, coord.y + 1))) return false;
                        if (!CheckInMap((coord.x + 1, coord.y))) return false;
                        return true;
                    }
                case 'L':
                    {
                        if (prev.x > coord.x)
                            return true;
                        if (!CheckInMap((coord.x - 1, coord.y))) return false;
                        if (!CheckInMap((coord.x - 1, coord.y + 1))) return false;
                        if (!CheckInMap((coord.x, coord.y + 1))) return false;
                        return true;
                    }
            }
            return false;
        }

        private void CheckInnerLoop((int x, int y) coord, (int x, int y) prev)
        {
            switch (data[coord.x, coord.y].tube)
            {
                case '|':
                    {
                        if (prev.y < coord.y)
                            CheckInnerLoop((coord.x - 1, coord.y));
                        else
                            CheckInnerLoop((coord.x + 1, coord.y));
                        break;
                    }
                case '-':
                    {
                        if (prev.x < coord.x)
                            CheckInnerLoop((coord.x, coord.y + 1));
                        else
                            CheckInnerLoop((coord.x, coord.y - 1));
                        break;
                    }
                case '7':
                    {
                        if (prev.x < coord.x)
                            return;
                        CheckInnerLoop((coord.x + 1, coord.y));
                        CheckInnerLoop((coord.x + 1, coord.y - 1));
                        CheckInnerLoop((coord.x, coord.y - 1));
                        break;
                    }
                case 'F':
                    {
                        if (prev.y > coord.y)
                            return;
                        CheckInnerLoop((coord.x, coord.y - 1));
                        CheckInnerLoop((coord.x - 1, coord.y - 1));
                        CheckInnerLoop((coord.x - 1, coord.y));
                        break;
                    }
                case 'J':
                    {
                        if (prev.y < coord.y)
                            return;
                        CheckInnerLoop((coord.x, coord.y + 1));
                        CheckInnerLoop((coord.x + 1, coord.y + 1));
                        CheckInnerLoop((coord.x + 1, coord.y));
                        break;
                    }
                case 'L':
                    {
                        if (prev.x > coord.x)
                            return;
                        CheckInnerLoop((coord.x - 1, coord.y));
                        CheckInnerLoop((coord.x - 1, coord.y + 1));
                        CheckInnerLoop((coord.x, coord.y + 1));
                        break;
                    }
            }
        }

        bool CheckInMap((int x, int y) coord)
        {
            return coord.x >= 0 && coord.x < data.GetLength(0) && coord.y >= 0 && coord.y < data.GetLength(1);
        }

        private (int x, int y) GetRightSide((int x, int y) coord, (int x, int y) prev)
        {
            (int x, int y) result = coord;
            switch (data[coord.x, coord.y].tube)
            {
                case '|':
                    {
                        if (prev.y < coord.y)
                            result = (coord.x - 1, coord.y);
                        else
                            result = (coord.x + 1, coord.y);
                        break;
                    }
                case '-':
                    {
                        if (prev.x < coord.x)
                            result = (coord.x, coord.y + 1);
                        else
                            result = (coord.x, coord.y - 1);
                        break;
                    }
                case '7':
                    {
                        if (prev.x < coord.x)
                            result = (coord.x - 1, coord.y + 1);
                        else
                            result = (coord.x, coord.y - 1);
                        break;
                    }
                case 'F':
                    {
                        if (prev.y > coord.y)
                            result = (coord.x + 1, coord.y + 1);
                        else
                            result = (coord.x - 1, coord.y);
                        break;
                    }
                case 'J':
                    {
                        if (prev.x < coord.x)
                            result = (coord.x + 1, coord.y);
                        else
                            result = (coord.x - 1, coord.y - 1);
                        break;
                    }
                case 'L':
                    {
                        if (prev.x > coord.x)
                            result = (coord.x + 1, coord.y - 1);
                        else
                            result = (coord.x, coord.y + 1);
                        break;
                    }
            }
            return result;
        }

        void CheckInnerLoop((int x, int y) Coord)
        {
            if (Coord.x >= 0 && Coord.x < data.GetLength(0) && Coord.y >= 0 && Coord.y < data.GetLength(1) &&
                data[Coord.x, Coord.y].label != 1)
            {
                data[Coord.x, Coord.y].label = 2;
            }
        }
    }
}

