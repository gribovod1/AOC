using AnyThings;

namespace AOC2022
{
    internal class Day19 : DayPattern<List<string>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine).ToList();
            data = text;
        }

        public override string PartOne()
        {
            var result = 0;
            for (var bp = 0; bp < data.Count; ++bp)
                result += GetBestFabric(data[bp], 24) * (bp + 1);
            return result.ToString();
        }

        public override string PartTwo()
        {
            return "";
            var result = 1;
            for (var bp = 0; bp < data.Count && bp < 3; ++bp)
                result *= GetBestFabric(data[bp], 32);
            return result.ToString();
        }

        private int GetBestFabric(string blueprint, int time)
        {
            var fabric = new GeodeFabric(blueprint);
            return fabric.GetBest(time);
        }
    }

    class GeodeFabric
    {
        public GeodeFabric(string bprint)
        {
            var ss = bprint.Split(' ');
            RobotOreCost = int.Parse(ss[6]);
            RobotClayCost = int.Parse(ss[12]);
            RobotObsidianCost = (int.Parse(ss[18]), int.Parse(ss[21]));
            RobotGeodeCost = (int.Parse(ss[27]), int.Parse(ss[30]));
        }

        public int GetBestRange(int v, Counts robot, Counts resources, int TypeProduce)
        {
            if (v <= 0) return resources.Geode;

            var max =0;

            switch (TypeProduce)
            {
                case 1:
                    {
                        if (RobotOreCost <= resources.Ore)
                        {
                            max = Math.Max(max, GetBestRange(v - 1, robot.DiffOre(1), (resources + robot).DiffOre(-RobotOreCost), TypeProduce));
                            max = Math.Max(max, GetBestRange(v - 1, robot.DiffOre(1), (resources + robot).DiffOre(-RobotOreCost), TypeProduce + 1));
                        }
                        break;
                    }
                case 2:
                    {
                        if (RobotClayCost <= resources.Ore) { 
                        max = Math.Max(max, GetBestRange(v - 1, robot.DiffClay(1), (resources + robot).DiffOre(-RobotClayCost), TypeProduce));
                        max = Math.Max(max, GetBestRange(v - 1, robot.DiffClay(1), (resources + robot).DiffOre(-RobotClayCost), TypeProduce + 1));
                    }
                        break;
                    }
                case 3:
                    {
                        if (RobotObsidianCost.Item1 <= resources.Ore && RobotObsidianCost.Item2 <= resources.Clay)
                        {
                            max = Math.Max(max, GetBestRange(v - 1, robot.DiffObsidian(1), (resources + robot).DiffOre(-RobotObsidianCost.Item1).DiffClay(-RobotObsidianCost.Item2), TypeProduce));
                            max = Math.Max(max, GetBestRange(v - 1, robot.DiffObsidian(1), (resources + robot).DiffOre(-RobotObsidianCost.Item1).DiffClay(-RobotObsidianCost.Item2), TypeProduce + 1));
                        }
                        break;
                    }
                case 4:
                    {
                        if (RobotGeodeCost.Item1 <= resources.Ore && RobotGeodeCost.Item2 <= resources.Obsidian) 
                            max = Math.Max(max, GetBestRange(v - 1, robot.DiffGeode(1), (resources + robot).DiffOre(-RobotGeodeCost.Item1).DiffObsidian(-RobotGeodeCost.Item2), TypeProduce));
                        break;
                    }
            }
            return Math.Max(max, GetBestRange(v - 1, robot, resources + robot, TypeProduce));
        }

        internal int GetBest(int v)
        {            
            return GetBest(v, new Counts(1, 0, 0, 0), new Counts());
            return GetBestRange(v, new Counts(1, 0, 0, 0), new Counts(),1);
        }

        internal int GetBest(int v, Counts robot, Counts resources)
        {/*
            if (v <= 0)
                return resources.Geode;
            var ObsidianHaveMeaning = ((v - 1) * robot.Obsidian + resources.Obsidian <= RobotGeodeCost.Item2) &&
                 ((v - 1) * (robot.Obsidian + 1) + resources.Obsidian > RobotGeodeCost.Item2);
             
                       if (RobotGeodeCost.Item1 <= resources.Ore && RobotGeodeCost.Item2 <= resources.Obsidian)
                           return GetBest(v - 1, robot.DiffGeode(1), resources.Diff(robot.Ore - RobotGeodeCost.Item1, robot.Clay, robot.Obsidian - RobotGeodeCost.Item2, robot.Geode));
                       if ((RobotObsidianCost.Item1 <= resources.Ore && RobotObsidianCost.Item2 <= resources.Clay) && ObsidianHaveMeaning)
                           return GetBest(v - 1, robot.DiffObsidian(1), resources.Diff(robot.Ore - RobotObsidianCost.Item1, robot.Clay - RobotObsidianCost.Item2, robot.Obsidian, robot.Geode));
                       if (RobotClayCost <= resources.Ore && (resources.Clay * RobotObsidianCost.Item1 < RobotObsidianCost.Item2 * resources.Ore))
                           return GetBest(v - 1, robot.DiffClay(1), resources.Diff(robot.Ore - RobotClayCost, robot.Clay, robot.Obsidian, robot.Geode));
                        if (RobotOreCost <= resources.Ore)
                                return GetBest(v - 1, robot.DiffOre(1), resources.Diff(robot.Ore - RobotOreCost, robot.Clay, robot.Obsidian, robot.Geode));
                            return GetBest(v - 1, robot, resources.Diff(robot.Ore, robot.Clay, robot.Obsidian, robot.Geode));
                       return Math.Max(GetBest(v - 1, robot.DiffOre(1), resources.Diff(robot.Ore - RobotOreCost, robot.Clay, robot.Obsidian, robot.Geode)),
                               GetBest(v - 1, robot, resources.Diff(robot.Ore, robot.Clay, robot.Obsidian, robot.Geode)));

                       var max = GetBest(v - 1, robot, resources+ robot);
                       if (RobotGeodeCost.Item1 <= resources.Ore && RobotGeodeCost.Item2 <= resources.Obsidian)// && (RobotGeodeCost.Item1 * resources.Ore == RobotGeodeCost.Item2 * resources.Obsidian))
                           return Math.Max(max, GetBest(v - 1, new Counts(robot.Ore, robot.Clay, robot.Obsidian, robot.Geode + 1), new Counts(resources.Ore + robot.Ore - RobotGeodeCost.Item1, resources.Clay + robot.Clay, resources.Obsidian + robot.Obsidian - RobotGeodeCost.Item2, resources.Geode + robot.Geode)));
                       if ((RobotObsidianCost.Item1 <= resources.Ore && RobotObsidianCost.Item2 <= resources.Clay) && ObsidianHaveMeaning)
                           return Math.Max(max, GetBest(v - 1, new Counts(robot.Ore, robot.Clay, robot.Obsidian + 1, robot.Geode), new Counts(resources.Ore + robot.Ore - RobotObsidianCost.Item1, resources.Clay + robot.Clay - RobotObsidianCost.Item2, resources.Obsidian + robot.Obsidian, resources.Geode + robot.Geode)));
                       if (RobotClayCost <= resources.Ore && (resources.Clay * RobotObsidianCost.Item1 < RobotObsidianCost.Item2 * resources.Ore))
                           max = Math.Max(max, GetBest(v - 1, new Counts(robot.Ore, robot.Clay + 1, robot.Obsidian, robot.Geode), new Counts(resources.Ore + robot.Ore - RobotClayCost, resources.Clay + robot.Clay, resources.Obsidian + robot.Obsidian, resources.Geode + robot.Geode)));
                       if (RobotOreCost <= resources.Ore)
                           max = Math.Max(max, GetBest(v - 1, new Counts(robot.Ore + 1, robot.Clay, robot.Obsidian, robot.Geode), new Counts(resources.Ore + robot.Ore - RobotOreCost, resources.Clay + robot.Clay, resources.Obsidian + robot.Obsidian, resources.Geode + robot.Geode)));
                       return max;
           */


            if (v <= 0)
                return resources.Geode;
            if (RobotGeodeCost.Item1 <= resources.Ore && RobotGeodeCost.Item2 <= resources.Obsidian)
                return GetBest(v - 1, robot.DiffGeode(1), (resources + robot).DiffOre(-RobotGeodeCost.Item1).DiffObsidian(-RobotGeodeCost.Item2));
            var max = GetBest(v - 1, robot, resources + robot);
            if (RobotObsidianCost.Item1 <= resources.Ore && RobotObsidianCost.Item2 <= resources.Clay)
                max = Math.Max(max, GetBest(v - 1, robot.DiffObsidian(1), (resources + robot).DiffOre(-RobotObsidianCost.Item1).DiffClay(-RobotObsidianCost.Item2)));
            if (RobotClayCost <= resources.Ore)
                max = Math.Max(max, GetBest(v - 1, robot.DiffClay(1), (resources + robot).DiffClay(-RobotClayCost)));
            if (RobotOreCost <= resources.Ore)
                max = Math.Max(max, GetBest(v - 1, robot.DiffOre(1), (resources + robot).DiffOre(-RobotOreCost)));
            return max;
        }


        int RobotOreCost;
        int RobotClayCost;
        (int, int) RobotObsidianCost;
        (int, int) RobotGeodeCost;

        internal struct Counts
        {
            public int Ore;
            public int Clay;
            public int Obsidian;
            public int Geode;

            public static Counts operator +(Counts a, Counts b)
            {
                return new Counts(a.Ore + b.Ore, a.Clay + b.Clay, a.Obsidian + b.Obsidian, a.Geode + b.Geode);
            }

            public Counts Diff(int ore, int clay, int obsidian, int geode)
            {
                return new Counts(Ore + ore, Clay + clay, Obsidian + obsidian, Geode + geode);
            }

            public Counts DiffOre(int d)
            {
                return new Counts(Ore + d, Clay, Obsidian, Geode);
            }

            public Counts DiffClay(int d)
            {
                return new Counts(Ore, Clay + d, Obsidian, Geode);
            }

            public Counts DiffObsidian(int d)
            {
                return new Counts(Ore, Clay, Obsidian + d, Geode);
            }

            public Counts DiffGeode(int d)
            {
                return new Counts(Ore, Clay, Obsidian, Geode + d);
            }

            public Counts(int ore, int clay, int obsidian, int geode)
            {
                Ore = ore;
                Clay = clay;
                Obsidian = obsidian;
                Geode = geode;
            }
        }
    }
}