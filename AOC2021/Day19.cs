using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using AnyThings;

namespace AOC2021
{
    class Day19 : DayPattern<List<Scaner>>
    {
        public override void Parse(string path)
        {
            var text = File.ReadAllText(path);
            var scanText = text.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            data = new List<Scaner>();
            for(var i=0;i < scanText.Length;++i)
                data.Add(new Scaner(scanText[i]));
        }

        public override string PartOne()
        {
            var result = data[0].Clone() as Scaner;
            data[0].IsAdded = true;
            bool addedScaner;
            do
            {
                addedScaner = false;
                for (var i = 0; i < data.Count; ++i)
                    if (!data[i].IsAdded && data[i].FindPosition(result))
                    {
                         result.CopyBeacons(data[i]);
                        data[i].IsAdded = true;
                        addedScaner = true;
                    }
            } while (addedScaner);
            return result.GetBeaconCount().ToString();
        }

        public override string PartTwo()
        {
            return 0.ToString();
        }
    }

    internal class Scaner
    {
        public Scaner(string text)
        {
            var ss = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var h = ss[0].Split(' ');
            this.Number = int.Parse(h[2]);
            Beacons = new List<Point3D>();
            for(var i = 1; i <ss.Length;++i)
                Beacons.Add(new Point3D(ss[i]));
        }

        public Scaner(Scaner scaner)
        {
            IsAdded = scaner.IsAdded;
            Number = scaner.Number;
            Beacons = new List<Point3D>(scaner.Beacons);
        }

        public bool IsAdded { get; internal set; }
        public int Number { get; }
        private List<Point3D> Beacons { get; }

        internal object Clone()
        {
            return new Scaner(this);
        }

        internal bool FindPosition(Scaner result)
        {
            List<Distance> distance = IntersectDistance(result);
            if (distance.Count >= 66)
            {
                for(var x = 0; x < 4; ++x)
                {
                    if (RotateAndMove(result, distance[0].point3D1))
                        return true;
                    Rotate(0);
                }
                Rotate(1);
                if (RotateAndMove(result, distance[0].point3D1))
                    return true;
                Rotate(1);
                Rotate(1);
                if (RotateAndMove(result, distance[0].point3D1))
                    return true;
            }
            return false;
        }

        bool RotateAndMove(Scaner result, Point3D basePoint)
        {
            for (var z = 0; z < 4; ++z)
            {
                for (var i = 0; i < result.Beacons.Count; ++i)
                {
                    Move(basePoint - result.Beacons[i]);
                    if (Intersect(result))
                        return true;
                }
                Rotate(2);
            }
            return false;
        }

        private void Move(Point3D p)
        {
            for (var i = 0; i < Beacons.Count; ++i)
                Beacons[i].Add(p);
        }

        private List<Distance> IntersectDistance(Scaner result)
        {
            return result.GetDistances().Intersect(GetDistances()).ToList();
        }

        private List<Distance> GetDistances()
        {
            List<Distance> result = new List<Distance>();
            for(var i=0;i<Beacons.Count; ++i)
                for(var j = i+1;j<Beacons.Count; ++j)
                    result.Add(new Distance(Beacons[i], Beacons[j]));
            return result;
        }

        private void Flip(int axis)
        {
            for (var b =0; b < Beacons.Count;++b)
                Beacons[b].Coords[axis] *= -1;
        }

        private void Rotate(int axis)
        {
            for (var b = 0; b < Beacons.Count; ++b)
                Beacons[b].Rotate(axis);
        }

        private bool Intersect(Scaner result)
        {
            var hashes = GetBeaconsHash();
            var otherHashes = result.GetBeaconsHash();
            hashes.IntersectWith(otherHashes);
            return hashes.Count >= 6;
        }

        private HashSet<Point3D> GetBeaconsHash()
        {
            var result = new HashSet<Point3D>();
            for(var i=0;i < Beacons.Count; ++i)
                result.Add(Beacons[i]);
            return result;
        }

        internal int GetBeaconCount()
        {
            return Beacons.Count;
        }

        internal void CopyBeacons(Scaner scaner)
        {
            var hashes = GetBeaconsHash();
            for (var i = 0; i < scaner.Beacons.Count; ++i)
                if (!hashes.Contains(scaner.Beacons[i]))
                    Beacons.Add(scaner.Beacons[i]);
        }

        private class Point3D
        {
            public Point3D(string text)
            {
                var ss = text.Split(',');
                Coords = new int[3];
                Coords[0] = int.Parse(ss[0]);
                Coords[1] = int.Parse(ss[1]);
                Coords[2] = int.Parse(ss[2]);
            }

            public Point3D(int x, int y, int z)
            {
                Coords = new int[3];
                Coords[0] = x;
                Coords[1] = y;
                Coords[2] = z;
            }

            public static Point3D operator -(Point3D x, Point3D y)
            {
                return new Point3D(x.Coords[0] - y.Coords[0], x.Coords[1] - y.Coords[1], x.Coords[2] - y.Coords[2]);
            }

            public int x { get { return Coords[0]; } set { Coords[0] = value; } }
            public int y { get { return Coords[1]; } set { Coords[1] = value; } }
            public int z { get { return Coords[2]; } set { Coords[2] = value; } }

            public int[] Coords { get; set; }

            internal void Rotate(int axis)
            {
                switch (axis)
                {
                    case 0:
                        {
                            RotateCoords(1, 2);
                            break;
                        }
                    case 1:
                        {
                            RotateCoords(0, 2);
                            break;
                        }
                    case 2:
                        {
                            RotateCoords(0, 1);
                            break;
                        }
                }
            }

            private void RotateCoords(int i1, int i2)
            {
                var t = Coords[i1];
                Coords[i1] = Coords[i2];
                Coords[i2] = -t;
            }

            public override int GetHashCode()
            {
                return Coords[0] + Coords[1] * 1000 + Coords[2] * 1000000;
            }

            public override bool Equals(object obj)
            {
                var p = obj as Point3D;
                if (p == null)
                    return base.Equals(obj);
                return p.Coords[0] == Coords[0] && p.Coords[1] == Coords[1] && p.Coords[2] == Coords[2];
            }

            public override string ToString()
            {
                return $"{Coords[0]},{Coords[1]},{Coords[2]}";
            }

            internal void Add(Point3D p)
            {
                Coords[0] += p.Coords[0];
                Coords[1] += p.Coords[1];
                Coords[2] += p.Coords[2];
            }
        }

        private class Distance
        {
            public Point3D point3D1;
            public Point3D point3D2;

            public Distance(Point3D point3D1, Point3D point3D2)
            {
                this.point3D1 = point3D1;
                this.point3D2 = point3D2;
                Dist =  (point3D1.Coords[0] - point3D2.Coords[0]) * (point3D1.Coords[0] - point3D2.Coords[0]) +
                        (point3D1.Coords[1] - point3D2.Coords[1]) * (point3D1.Coords[1] - point3D2.Coords[1]) +
                        (point3D1.Coords[2] - point3D2.Coords[2]) * (point3D1.Coords[2] - point3D2.Coords[2]);
            }

            public long Dist { get; set; }
            public override bool Equals(object obj)
            {
                if (obj is Distance)
                    return Dist == (obj as Distance).Dist;
                return base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return Dist.GetHashCode();
            }
        }
    }
}