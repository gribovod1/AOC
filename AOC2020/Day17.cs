using AOC;
using System;
using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public class Day17
    {
        class Cube
        {
            public int x;
            public int y;
            public int z;
            public int w;
            public char state;
            public Cube(int x, int y, int z, int w, char state)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
                this.state = state;
            }
            public bool isNeibor(int x, int y, int z, int w)
            {
                if (this.x == x && this.y == y && this.z == z && this.w == w)
                    return false;
                return this.x >= x - 1 && this.x <= x + 1 &&
                    this.y >= y - 1 && this.y <= y + 1 &&
                    this.z >= z - 1 && this.z <= z + 1 &&
                    this.w >= w - 1 && this.w <= w + 1;
            }
            public override string ToString()
            {
                return x+" "+y+" "+ z + " " + w + " " + state;
            }
        }

        public void Exec()
        {
            var startCubesText = File.ReadAllLines(@"Data\day17.txt");

            /*
            var space = new Space();
            for (var y = 0; y < startCubesText.Length; ++y)
                for (var x = 0; x < startCubesText[y].Length; ++x)
                    addCube(space, x, y, 0, startCubesText[y][x]);
                    */
            var cubes = new List<Cube>();
            for (var y = 0; y < startCubesText.Length; ++y)
                for (var x = 0; x < startCubesText[y].Length; ++x)
                    if (startCubesText[y][x]=='#')
                    cubes.Add(new Cube(x, y, 0, 0, startCubesText[y][x]));

            var one = partOne(cubes);
            var two = partTwo();
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(one.ToString());
        }
        /*
        void spaceProcess(Space space, Action<int, int, int> proc)
        {
            for(var x = space.Keys[0]-1;x <= space.Keys[space.Keys.Count-1]+1;++x)
                for (var y = space.Keys[0] - 1; y <= space.Keys[space.Keys.Count - 1] + 1; ++y)
                    for (var z = space.Keys[0] - 1; z <= space.Keys[space.Keys.Count - 1] + 1; ++z)
                    {

                    }
            var xE = space.Keys.GetEnumerator();
            while (xE.MoveNext())
            {
                var yE = space[xE.Current].Keys.GetEnumerator();
                while (yE.MoveNext())
                {
                    var zE = space[xE.Current][yE.Current].Keys.GetEnumerator();
                    while (zE.MoveNext())
                        proc(xE.Current, yE.Current, zE.Current);
                }
            }
        }

        void addCube(Space space, int x, int y, int z, char state)
        {
            if (!space.ContainsKey(x))
                space.Add(x, new Dictionary<int, Dictionary<int, char>>());
            if (!space[x].ContainsKey(y))
                space[x].Add(y, new Dictionary<int, char>());
            if (!space[x][y].ContainsKey(z))
                space[x][y].Add(z, state);
            else
                space[x][y][z] = state;
        }
        */
        int partOne(List<Cube> cubes)
        {
            for (var i = 0; i < 6; ++i)
                cubes = step(cubes);
            return count(cubes);
        }

        List<Cube> step(List<Cube> cubes)
        {
            var newCubes = new List<Cube>();
            for (var i = 0; i < cubes.Count; i++)
            {
                var countNeibor = 0;

                for (var xN = cubes[i].x - 1; xN <= cubes[i].x + 1; ++xN)
                    for (var yN = cubes[i].y - 1; yN <= cubes[i].y + 1; ++yN)
                        for (var zN = cubes[i].z - 1; zN <= cubes[i].z + 1; ++zN)
                            for (var wN = cubes[i].w - 1;wN <= cubes[i].w + 1; ++wN)
                                if (xN != cubes[i].x || yN != cubes[i].y || zN != cubes[i].z || wN != cubes[i].w)
                            {
                                var c = cubeAtCoord(cubes, xN, yN, zN, wN);
                                if (c != null && c.state=='#')
                                    ++countNeibor;
                                else
                                {
                                    var n = countActionNeibor(cubes, xN, yN, zN, wN);
                                    if (n == 3 && cubeAtCoord(newCubes,xN, yN, zN, wN) ==null)
                                        newCubes.Add(new Cube(xN, yN, zN, wN, '#'));
                                }
                            }
                if (countNeibor == 2 || countNeibor == 3)
                    newCubes.Add(cubes[i]);
            }
            return newCubes;
        }

        Cube cubeAtCoord(List<Cube> cubes, int x, int y, int z, int w)
        {
            for (var i = 0; i < cubes.Count; ++i)
                if (cubes[i].x == x && cubes[i].y == y && cubes[i].z == z && cubes[i].w == w)
                    return cubes[i];
            return null;
        }

        int countActionNeibor(List<Cube> cubes,int x,int y, int z, int w)
        {
            int count = 0;
            for (var i = 0; i < cubes.Count; ++i)
                if (cubes[i].isNeibor(x,y,z,w) && cubes[i].state == '#')
                    ++count;
            return count;
        }

        int count(List<Cube> cubes)
        {
            int count = 0;
            for (var i = 0; i < cubes.Count; ++i)
                if (cubes[i].state == '#')
                    ++count;
            return count;
        }
        /*
        void spaceProcess(Space space, Action<int,int,int> proc)
        {
            var xE = space.Keys.GetEnumerator();
            while (xE.MoveNext())
            {
                var yE = space[xE.Current].Keys.GetEnumerator();
                while (yE.MoveNext())
                {
                    var zE = space[xE.Current][yE.Current].Keys.GetEnumerator();
                    while (zE.MoveNext())
                        proc(xE.Current,yE.Current,zE.Current);
                }
            }
        }

        Space step(Space space)
        {
            var newSpace = new Space();

            spaceProcess(space, (x,y,z) =>
            {
                var countActive = countActiveNeibor(space, x, y, z);
                if (getState(space, x, y, z) == '#')
                {
                    if (countActive >= 2 && countActive <= 3)
                        addCube(newSpace, x, y, z, '#');
                }
                else
                {
                    if (countActive == 3)
                        addCube(newSpace, x, y, z, '#');
                }
            });

            for (var x = 0; x < space.Count; ++x)
                for (var y = 0; y < space[x].Count; ++y)
                    for (var z = 0; z < space[x][y].Count; ++z)
                    {
                        var countActive = countActiveNeibor(space, x, y, z);
                        if (getState(space, x, y, z) == '#')
                        {
                            if (countActive >= 2 && countActive <= 3)
                                addCube(newSpace, x, y, z, '#');
                        }
                        else
                        {
                            if (countActive == 3)
                                addCube(newSpace, x, y, z, '#');
                        }
                    }
            return newSpace;
        }

        char getState(Space space, int x, int y, int z)
        {
            if (!space.ContainsKey(x))
                return '.';
            if (!space[x].ContainsKey(y))
                return '.';
            if (!space[x][y].ContainsKey(z))
                return '.';
            return space[x][y][z];
        }

        int countActiveNeibor(Space space, int xC, int yC, int zC)
        {
            var count = 0;
            for (var x = xC-1; x <= xC+1; ++x)
                for (var y = yC-1; y <= yC+1; ++y)
                    for (var z = zC-1; z <= zC+1; ++z)
                        if (getState(space,x,y,z) == '#')
                            ++count;
            if (getState(space, xC, yC, zC) == '#')
                --count;
            return count;
        }

        int count(Space space)
        {
            var count = 0;
            for (var x = 0; x < space.Count; ++x)
                for (var y = 0; y < space[x].Count; ++y)
                    for (var z = 0; z < space[x][y].Count; ++z)
                        if (space[x][y][z] == '#')
                            ++count;
            return count;
        }
        */
        ulong partTwo()
        {
            ulong mult = 0;
            return mult;
        }
    }
}