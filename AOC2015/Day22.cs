using AnyThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2015
{
    class Step
    {
        public enum TypeStep
        {
            Missile,
            Drain,
            Shield,
            Poison,
            Recharge
        }
        public TypeStep type;
        public static int boss_damage = 0;
        public int player_hit = 0;
        public int player_armor = 0;
        public int player_mana = 0;
        public int player_mana_summary = 0;
        public int boss_hit = 0;

        public int shield = 0;
        public int poison = 0;
        public int recharge = 0;

        public Step()
        {

        }

        public Step(Step source, TypeStep type)
        {
            this.type = type;
            player_hit = source.player_hit;
            player_armor = source.player_armor;
            player_mana = source.player_mana;
            player_mana_summary = source.player_mana_summary;
            boss_hit = source.boss_hit;
            shield = source.shield;
            poison = source.poison;
            recharge = source.recharge;
        }

    void CounterCalc()
        {
            if (poison > 0)
            {
                boss_hit -= 3;
                --poison;
            }
            if (shield > 0)
            {
                player_armor += 7;
                --shield;
            }
            if (recharge > 0)
            {
                player_mana += 101;
                --recharge;
            }
        }

        List<Step> GetNextPlayerSteps()
        {
            var result = new List<Step>
            {
                new Step(this, TypeStep.Recharge),
                new Step(this, TypeStep.Poison),
                new Step(this, TypeStep.Shield),
                new Step(this, TypeStep.Drain),
                new Step(this, TypeStep.Missile),
            };
            return result;
        }

        public void AddAllPlayerSteps(Queue<Step> data)
        {
            var ss = GetNextPlayerSteps();
            foreach (var s in ss)
                data.Enqueue(s);
        }

        bool cast(int price)
        {
            if (player_mana < price) return false;
            player_mana -= price;
            player_mana_summary += price;
            return true;
        }

        public int Play(Queue<Step> data)
        {
            CounterCalc();
            if (boss_hit <= 0) return player_mana_summary;
            switch (type)
            {
                case TypeStep.Missile:
                    {
                        if (!cast(53)) return -1;
                        boss_hit -= 4;
                        break;
                    }
                case TypeStep.Drain:
                    {
                        if (!cast(73)) return -1;
                        player_hit += 2;
                        boss_hit -= 2;
                        break;
                    }
                case TypeStep.Shield:
                    {
                        if (!cast(113) || shield > 0) return -1;
                        shield = 6;
                        break;
                    }
                case TypeStep.Poison:
                    {
                        if (!cast(173) || poison > 0) return -1;
                        poison = 6;
                        break;
                    }
                case TypeStep.Recharge:
                    {
                        if (!cast(229) || recharge > 0) return -1;
                        recharge = 5;
                        break;
                    }
            }
            if (boss_hit <= 0) return player_mana_summary;

            //Boss step
            CounterCalc();
            if (boss_hit <= 0) return player_mana_summary;
            player_hit -= Math.Max(boss_damage - player_armor, 1);
            if (player_hit > 0) AddAllPlayerSteps(data);

            return -1;
        }


        public int GetMinMana(ref int currResult)
        {
            if (player_mana_summary >= currResult) return -1;
            if (player_mana < 53) return -1;
            if (player_hit <= 0) return -1;
            if (boss_hit <= 0) return player_mana_summary;
            int result = int.MaxValue;

            // Player step
            CounterCalc();
            if (boss_hit <= 0) return player_mana_summary;
            var ss = GetNextPlayerSteps();
            foreach(var s in ss)
            {
                var variant = s.GetMinMana(ref currResult);
                if (variant >= 0)
                    result = Math.Min(variant, result);
            }
            if (result < currResult)
                currResult = result;

            // Boss step
            CounterCalc();
            if (boss_hit <= 0) return player_mana_summary;
            player_hit -= Math.Max(boss_damage - player_armor, 1);
            if (player_hit <= 0) return -1;

            return result < int.MaxValue ? result : -1;
        }
    }

    internal class Day22 : DayPattern<Queue<Step>>
    {
        public override void Parse(string path)
        {
            data = new Queue<Step>();
       //      var s = new Step { player_hit = 10, player_mana = 250, boss_hit = 13 };Step.boss_damage = 8;
           var s = new Step { player_hit = 50, player_mana = 500, boss_hit = 58 }; Step.boss_damage = 9;
            s.AddAllPlayerSteps(data);
        }

        public override string PartOne()
        {
            var result = int.MaxValue;
         //   return data.Dequeue().getMinMana(ref result).ToString();

            while (data.Count > 0)
            {
                var s = data.Dequeue();
                if (s.player_mana_summary < result)
                {
                    var mana = s.Play(data);
                    if (mana > 0 && mana < result)
                        result = mana;
                }
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            int result = 0;
            return result.ToString();
        }
    }
}