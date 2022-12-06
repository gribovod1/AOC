using AOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace AOC2020
{
    public class Tile
    {
        public enum BoardSide
        {
            Up, Left, Down, Right, None
        }

        public Dictionary<BoardSide, Tile> Neighbours = new Dictionary<BoardSide, Tile>();
        public ulong Id;
        public List<string> Data = new List<string>();

        public Tile(string text)
        {
            var ss = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var idText = ss[0].Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
            Id = ulong.Parse(idText[1]);

            Data = new List<string>();
            for (var s = 1; s < ss.Length; ++s)
                Data.Add(ss[s]);
        }

        void Rotate()
        {
            var result = new List<StringBuilder>();
            for (var r = 0; r < Data.Count; ++r)
            {
                var sb = new StringBuilder();

                for (var c = 0; c < Data[0].Length; ++c)
                    sb.Append(Data[c][Data[0].Length - r - 1]);
                result.Add(sb);
            }
            Data.Clear();
            for (var i = 0; i < result.Count; ++i)
                Data.Add(result[i].ToString());
        }

        void FlipVertical()
        {
            for (var r = 0; r < Data.Count / 2; ++r)
            {
                var t = Data[r];
                Data[r] = Data[Data.Count - r - 1];
                Data[Data.Count - r - 1] = t;
            }
        }

        public void FindNeighbours(List<Tile> tiles, int x = -1, int y = -1)
        {
            Show(x, y);
            foreach (var t in tiles)
            {
                if (t == this)
                    continue;
                if (Neighbours.ContainsValue(t))
                    continue;
                var side = IsNeighbour(t);
                if (side != BoardSide.None)
                {
                    if (x >= 0 && y >= 0)
                    {
                        t.FindNeighbours(tiles,
                            x + (side == BoardSide.Left ? -1 : side == BoardSide.Right ? 1 : 0),
                            y + (side == BoardSide.Up ? -1 : side == BoardSide.Down ? 1 : 0));
                        Show(x, y);
                    }
                    else
                        t.FindNeighbours(tiles);
                }
                if (Neighbours.Count == 4)
                    break;
            }
        }

        void Show(int x = -1, int y = -1)
        {
            if (x >= 0 && y >= 0)
            {
                Console.CursorLeft = x * 5;
                Console.CursorTop = y;
                var fc = Console.ForegroundColor;
                Console.ForegroundColor =
                    Neighbours.Count == 1 ? ConsoleColor.Yellow :
                    Neighbours.Count == 2 ? ConsoleColor.Green :
                    Neighbours.Count == 3 ? ConsoleColor.Magenta :
                    Neighbours.Count == 4 ? ConsoleColor.Cyan : ConsoleColor.Red;
                Console.Write(Id);
                Console.ForegroundColor = fc;
                Thread.Sleep(10);
            }
        }

        BoardSide IsNeighbour(Tile other)
        {
            for (var f = 0; f < 2; ++f)
            {
                for (var r = 0; r < 4; ++r)
                {
                    if (CheckSide(other, BoardSide.Up))
                    {
                        Neighbours.Add(BoardSide.Up, other);
                        other.Neighbours.Add(BoardSide.Down, this);
                        return BoardSide.Up;
                    }
                    if (CheckSide(other, BoardSide.Down))
                    {
                        Neighbours.Add(BoardSide.Down, other);
                        other.Neighbours.Add(BoardSide.Up, this);
                        return BoardSide.Down;
                    }
                    if (CheckSide(other, BoardSide.Left))
                    {
                        Neighbours.Add(BoardSide.Left, other);
                        other.Neighbours.Add(BoardSide.Right, this);
                        return BoardSide.Left;
                    }
                    if (CheckSide(other, BoardSide.Right))
                    {
                        Neighbours.Add(BoardSide.Right, other);
                        other.Neighbours.Add(BoardSide.Left, this);
                        return BoardSide.Right;
                    }
                    if (other.Neighbours.Count > 0)// If other has neighbours, then can't rotate and flip other
                        return BoardSide.None;
                    other.Rotate();
                }
                other.FlipVertical();
            }
            return BoardSide.None;
        }

        bool CheckSide(Tile other, BoardSide side)
        {
            if (side == BoardSide.Down)
                return other.Data[0] == Data[Data.Count - 1];
            if (side == BoardSide.Up)
                return other.Data[other.Data.Count - 1] == Data[0];
            if (side == BoardSide.Left)
                for (var r = 0; r < Data.Count; ++r)
                    if (Data[r][0] != other.Data[r][other.Data.Count - 1])
                        return false;
            if (side == BoardSide.Right)
                for (var r = 0; r < Data.Count; ++r)
                    if (Data[r][Data.Count - 1] != other.Data[r][0])
                        return false;
            return true;
        }

        public void TrimBoards()
        {
            Data.RemoveAt(0);
            Data.RemoveAt(Data.Count - 1);
            for (var i = 0; i < Data.Count; ++i)
                Data[i] = Data[i].Substring(1, Data[i].Length - 2);
        }

        public void addTile(char[,] image, int x, int y)
        {
            for (var yData = 0; yData < Data.Count; ++yData)
                for (var xData = 0; xData < Data[yData].Length; ++xData)
                    if (image[x * Data[0].Length + xData, y * Data.Count + yData] != 'X')
                        throw new Exception("Картинки наползают!");
                    else
                        image[x * Data[0].Length + xData, y * Data.Count + yData] = Data[yData][xData];
        }

        public bool isLeftUp()
        {
            return !Neighbours.ContainsKey(BoardSide.Up) && !Neighbours.ContainsKey(BoardSide.Left);
        }

        public int ways()
        {
            var result = 0;
            for (var yData = 1; yData < Data.Count-1; ++yData)
                for (var xData = 1; xData < Data[yData].Length-1; ++xData)
                    result += Data[yData][xData] == '#' ? 1 : 0;
            return result;
        }
    }


    public class Day20
    {
        public List<Tile> Tiles = new List<Tile>();

        public void Exec()
        {
            var text = File.ReadAllText(@"Data\day20.txt");
            var splited = text.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in splited)
                Tiles.Add(new Tile(s));
            var one = PartOne();
            var two = PartTwo();
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        ulong PartOne()
        {
            ulong result = 1;
            Tiles[0].FindNeighbours(Tiles,15,15);
            Console.CursorLeft = 0;
            Console.CursorTop = 30;
            for (var i = 0; i < Tiles.Count; ++i)
                if (Tiles[i].Neighbours.Count == 2)
                    result *= Tiles[i].Id;

            return result;
        }

        int PartTwo()
        {
            var w = 0;
            foreach (var t in Tiles)
                w += t.ways();
            Console.WriteLine($"ways:{w}");

            var image = GetImage();
            var height = CalcHeight() * Tiles[0].Data.Count;
            var width = CalcWidth() * Tiles[0].Data[0].Length;
            var pattern = File.ReadAllLines(@"Data\day20_monster.txt");
            var monsters = CountMonster(image, pattern, width, height);

            Console.WriteLine("IMAGE:");
            for (var y = 0; y < height; ++y)
            {
                Console.Write("|");
                for (var x = 0; x < width; ++x)
                {
                    var cb = Console.BackgroundColor;
                    var cf = Console.ForegroundColor;
                    Console.ForegroundColor = (image[x, y] == 'O') 
                        ? ConsoleColor.Red
                        : (image[x, y] == '#')
                        ? ConsoleColor.White
                        : (image[x, y] == 'X')
                        ? ConsoleColor.Yellow
                        : ConsoleColor.Blue;
                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.Write(image[x, y]);
                    Console.BackgroundColor = cb;
                    Console.ForegroundColor = cf;
                }
                Console.WriteLine("|");
            }

            return CalcWays(image, width, height);
        }

        char[,] GetImage()
        {
            for (var i = 0; i < Tiles.Count; ++i)
                Tiles[i].TrimBoards();
            var height = CalcHeight();
            var width = CalcWidth();
            var result = new char[width * Tiles[0].Data[0].Length, height * Tiles[0].Data.Count];
            for (var y = 0; y < height * Tiles[0].Data.Count; ++y)
                for (var x = 0; x < width * Tiles[0].Data[0].Length; ++x)
                    result[x, y] = 'X';

            var startRowTile = FindLeftUp();
            var row = 0;
            do
            {
                Tile currTile = startRowTile;
                var col = 0;
                do
                {
                    currTile.addTile(result, col, row);
                    currTile = currTile.Neighbours.ContainsKey(Tile.BoardSide.Right) ? currTile.Neighbours[Tile.BoardSide.Right] : null;
                    ++col;
                } while (currTile != null);
                startRowTile = startRowTile.Neighbours.ContainsKey(Tile.BoardSide.Down) ? startRowTile.Neighbours[Tile.BoardSide.Down] : null;
                ++row;
            } while (startRowTile != null);
            return result;
        }

        Tile FindLeftUp()
        {
            foreach (var t in Tiles)
                if (t.isLeftUp())
                    return t;
            throw new Exception("Error tile placement");
        }

        int CalcWidth()
        {
            var tile = FindLeftUp();
            var result = 1;
            while (tile.Neighbours.ContainsKey(Tile.BoardSide.Right))
            {
                ++result;
                tile = tile.Neighbours[Tile.BoardSide.Right];
            }
            return result;
        }

        int CalcHeight()
        {
            var tile = FindLeftUp();
            var result = 1;
            while (tile.Neighbours.ContainsKey(Tile.BoardSide.Down))
            {
                ++result;
                tile = tile.Neighbours[Tile.BoardSide.Down];
            }
            return result;
        }

        int CalcWays(char[,] image, int width, int height)
        {
            var result = 0;
            for (var y = 0; y < height; ++y)
                for (var x = 0; x < width; ++x)
                    result += image[x, y] == '#' ? 1 : 0;
            return result;
        }

        int CountMonster(char[,] image, string[] pattern, int width, int height)
        {
            for (var f = 0; f < 2; ++f)
            {
                for (var r = 0; r < 4; ++r)
                {
                    var currMonsters = 0;
                    for (var row = 0; row <= height - pattern.Length; ++row)
                        for (var col = 0; col <= width - pattern[0].Length; ++col)
                        {
                            if (ThereAreMonsterHere(image, col, row, pattern))
                                ++currMonsters;
                        }
                    if (currMonsters > 0)
                        return currMonsters;
                    RotateImage(image, width, height);
                }
                FlipImage(image, width, height);
            }
            return 0;
        }

        bool ThereAreMonsterHere(char[,] image, int x, int y, string[] pattern)
        {
            for (var row = 0; row < pattern.Length; ++row)
                for (var col = 0; col < pattern[0].Length; ++col)
                    if (pattern[row][col] == '#' && image[x + col, y + row] != '#')
                        return false;
            for (var row = 0; row < pattern.Length; ++row)
                for (var col = 0; col < pattern[0].Length; ++col)
                    if (pattern[row][col] == '#')
                        image[x + col, y + row] = 'O';
            return true;
        }

        void RotateImage(char[,] image, int width, int height)
        {
            char[,] old = new char[width, height];
            for (var y = 0; y < height; ++y)
                for (var x = 0; x < width; ++x)
                    old[x, y] = image[x, y];

            for (var y = 0; y < height; ++y)
                for (var x = 0; x < width; ++x)
                    image[x, y] = old[width - y - 1, x];
        }

        void FlipImage(char[,] image, int width, int height)
        {
            for (var y = 0; y < height; ++y)
                for (var x = 0; x < width / 2; ++x)
                {
                    var t = image[x, y];
                    image[x, y] = image[width - x - 1, y];
                    image[width - x - 1, y] = t;
                }
        }
    }
}