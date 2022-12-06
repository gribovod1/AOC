using AnyThings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace AOC2021
{
    struct AmphipodState
    {
        public int Energy;
        public Point[] Coord;

        public AmphipodState(int energy, Point[] coord) : this()
        {
            Energy = energy;
            Coord = new Point[coord.Length];
            for (var i = 0; i < coord.Length; ++i)
                Coord[i] = coord[i];
        }

    }
    internal class AmphipodCave
    {
        public List<Point> FreeCoords = new List<Point>();
        public int CaveSize
        {
            get {return FreeCoords.Count; }
        }

        public AmphipodState StartCoords;
        private Dictionary<KeyValuePair<Point,Point>,List<Point>> AllWays; // Словарь с координатами для кратчайшего пути

        public List<AmphipodState> getVariants(AmphipodState state)
        {
            var result = new List<AmphipodState>();
            for (var i = 0; i < state.Coord.Length; ++i)
            {
                var steps = GetAmphipodSteps(state, i);
                foreach(var se in steps)
                {
                    var newState = new AmphipodState(state.Energy + se.Key, state.Coord);
                    newState.Coord[i] = se.Value;
                    result.Add(newState);
                }
            }
            return result;
        }

        private List<KeyValuePair<int, Point>> GetAmphipodSteps(AmphipodState state, int index)
        {
            List<KeyValuePair<int, Point>> result = new List<KeyValuePair<int, Point>>();
            for (var i = 0; i < CaveSize; ++i)                
            {
                var coord = FreeCoords[i];
                int stepCount = stepsToCoord(state, index, coord);
                if (stepCount > 0)
                {
                    var energyByStep = (int)Math.Pow(10, (index / 2 + 1));
                    result.Add(new KeyValuePair<int, Point>(stepCount * energyByStep, coord));
                }
            }
            return result;
        }

        List<Point> ForbiddenPoints = new List<Point>() { new Point(3, 1), new Point(5, 1), new Point(7, 1), new Point(9, 1) };
        private int stepsToCoord(AmphipodState state, int index, Point coord)
        {
            if (ForbiddenPoints.Contains(coord)) // Нельзя останавливаться перед комнатами
                return 0;
            var currentCoord = state.Coord[index];
            List<Point> coordlist = AllWays[new KeyValuePair<Point, Point>(currentCoord, coord)];
            foreach (var c in coordlist)
                if (coordIsOccuped(state, c))
                    return 0;
            return coordlist.Count - 1;
        }

        private bool coordIsOccuped(AmphipodState state, Point c)
        {
            foreach (var amphipodCoord in state.Coord) // Нельзя останавливаться там, где кто-то уже стоит
                if (amphipodCoord == c)
                    return true;
            return false;
        }

        List<List<Point>> RoomPoints = new List<List<Point>>() {
            new List<Point>() { new Point(3,2),new Point(3,3)},
            new List<Point>(){ new Point(5,2),new Point(5,3)},
            new List<Point>(){ new Point(7,2),new Point(7,3)},
            new List<Point>() { new Point(9,2),new Point(9,3)}
        };

        public AmphipodCave(string coords)
        {
          /*  var startStateMap = coords.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (var y = 0; y < startStateMap.Length; ++y)
                for (var x = 0; x < startStateMap[0].Length; ++x)
                {
                    if (startStateMap[y][x] >= 'A' && startStateMap[y][x] <= 'D')
                    {
                        var posIndex = (startStateMap[y][x] - 'A') * 2;
                        if (data.[posIndex] != 0)
                            ++posIndex;
                        pos[posIndex] = coord;
                    }
                    ++coord;
                }

            data.StartCoords = new AmphipodState();
            data.StartCoords.Energy = 0;
            var pos = new int[8];
            var coord = 0;
            for (var i = 0; i < coords.Length; ++i)
                if (coords[i] != '#')
                {
                    if (coords[i] >= 'A' && coords[i] <= 'D')
                    {
                        var posIndex = (coords[i] - 'A') * 2;
                        if (pos[posIndex] != 0)
                            ++posIndex;
                        pos[posIndex] = coord;
                    }
                    ++coord;
                }
            data.CaveSize = coord;
            data.StartCoords.Coord = pos;

            var map = mapStringBuilder.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var localCoord = 0;
            for (var y = 0; y < map.Length && localCoord < data.CaveSize; ++y)
                for (var x = 0; x < map[0].Length && localCoord < data.CaveSize; ++x)
                    if (map[y][x] < '#')
                    {
                        FillWays(map, x, y, localCoord);
                        ++localCoord;
                    }*/
        }
        void FillWays(string[] map, int x, int y, int freeCoord)
        {

        }

        void addWays(string[] map, int x, int y)
        {

        }


        public bool isEndState(AmphipodState state)
        {
            if (!RoomPoints[0].Contains(state.Coord[0]))
                return false;
            if (!RoomPoints[0].Contains(state.Coord[1]))
                return false;
            if (!RoomPoints[1].Contains(state.Coord[2]))
                return false;
            if (!RoomPoints[1].Contains(state.Coord[3]))
                return false;
            if (!RoomPoints[2].Contains(state.Coord[4]))
                return false;
            if (!RoomPoints[2].Contains(state.Coord[5]))
                return false;
            if (!RoomPoints[3].Contains(state.Coord[6]))
                return false;
            if (!RoomPoints[3].Contains(state.Coord[7]))
                return false;
            return true;
        }
    }

    class Day23 : DayPattern<AmphipodCave>
    {
        public override void Parse(string path)
        {
            var coords = File.ReadAllText(path);// AnyThings.Parse.ParseToCharMap(path);

            coords = coords.Replace(' ', '#');

            var mapStringBuilder = new StringBuilder();
            foreach (var c in coords)
                if ((c == '.') || (c >= 'A' && c <= 'D'))
                    mapStringBuilder.Append((char)0);
                else
                    mapStringBuilder.Append(c);

            data = new AmphipodCave(coords);
        }
        public override string PartOne()
        {
            Queue<AmphipodState> states = new Queue<AmphipodState>();
            states.Enqueue(data.StartCoords);
            int minEnergy = 1000000;
            while (states.Count > 0)
            {
                var state = states.Dequeue();
                var steps = data.getVariants(state);
                foreach (var s in steps)
                    if (!data.isEndState(s) && s.Energy < minEnergy)
                        states.Enqueue(s);
            }
            return minEnergy.ToString();
        }

        public override string PartTwo()
        {
            long result = 0;
            return result.ToString();
        }
    }
}