using AOC;
using System;

namespace AOC2015
{
    public class Day21
    {
        public void Exec()
        {
            Tuple<int, int, int>[] weapons = new Tuple<int, int, int>[]{
                new Tuple<int, int, int>(8, 4, 0),
                new Tuple<int, int, int>(10, 5, 0),
                new Tuple<int, int, int>(25, 6, 0),
                new Tuple<int, int, int>(40, 7, 0),
                new Tuple<int, int, int>(74, 8, 0),
            };

            Tuple<int, int, int>[] armor = new Tuple<int, int, int>[]{
                new Tuple<int, int, int>(0, 0, 0),
                new Tuple<int, int, int>(13, 0, 1),
                new Tuple<int, int, int>(31, 0, 2),
                new Tuple<int, int, int>(53, 0, 3),
                new Tuple<int, int, int>(75, 0, 4),
                new Tuple<int, int, int>(102, 0, 5),
            };

            Tuple<int, int, int>[] rings = new Tuple<int, int, int>[]{
                new Tuple<int, int, int>(0, 0, 0),
                new Tuple<int, int, int>(0, 0, 0),
                new Tuple<int, int, int>(25, 1, 0),
                new Tuple<int, int, int>(50, 2, 0),
                new Tuple<int, int, int>(100, 3, 0),
                new Tuple<int, int, int>(20, 0, 1),
                new Tuple<int, int, int>(40, 0, 2),
                new Tuple<int, int, int>(80, 0, 3),
            };

            Unit enemy = new Unit(109, 8, 2);

            var one = partOne(weapons, armor, rings, enemy);
            var two = partTwo(weapons, armor, rings, enemy);
            Console.WriteLine($"partOne: {one} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        class Unit
        {
            public int Health;
            public int Damage;
            public int Armor;
            public Unit(int Health, int Damage, int Armor)
            {
                this.Armor = Armor;
                this.Damage = Damage;
                this.Health = Health;
            }

            public bool Battle(Unit enemy)
            {
                var enemySteps = Math.Ceiling((decimal)Health / Math.Max(1, enemy.Damage - Armor));
                var thisSteps = Math.Ceiling((decimal)enemy.Health / Math.Max(1, Damage - enemy.Armor));
                return thisSteps <= enemySteps;
            }
        }

        int partOne(Tuple<int, int, int>[] weapons, Tuple<int, int, int>[] armor, Tuple<int, int, int>[] rings, Unit enemy)
        {
            int result = int.MaxValue;

            foreach (var w in weapons)
            {
                foreach (var a in armor)
                {
                    for (var r1 = 0; r1 < rings.Length; ++r1)
                        for (var r2 = r1 + 1; r2 < rings.Length; ++r2)
                        {
                            int coins = w.Item1 + a.Item1 + rings[r1].Item1 + rings[r2].Item1;
                            Unit hero = new Unit(100, w.Item2 + a.Item2 + rings[r1].Item2 + rings[r2].Item2, w.Item3 + a.Item3 + rings[r1].Item3 + rings[r2].Item3);
                            if (hero.Battle(enemy) && coins < result)
                            {
                                result = coins;
                            }
                        }
                }
            }
            return result;
        }

        int partTwo(Tuple<int, int, int>[] weapons, Tuple<int, int, int>[] armor, Tuple<int, int, int>[] rings, Unit enemy)
        {
            int result = 0;
            foreach (var w in weapons)
            {
                foreach (var a in armor)
                {
                    for (var r1 = 0; r1 < rings.Length; ++r1)
                        for (var r2 = r1 + 1; r2 < rings.Length; ++r2)
                        {
                            int coins = w.Item1 + a.Item1 + rings[r1].Item1 + rings[r2].Item1;
                            Unit hero = new Unit(100, w.Item2 + a.Item2 + rings[r1].Item2 + rings[r2].Item2, w.Item3 + a.Item3 + rings[r1].Item3 + rings[r2].Item3);
                            if (!hero.Battle(enemy) && coins > result)
                            {
                                result = coins;
                            }
                        }
                }
            }
            return result;
        }
    }
}
