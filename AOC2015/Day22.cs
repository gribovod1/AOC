using AnyThings;
using System;
using System.Collections.Generic;

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

        public bool cast()
        {
            switch (type)
            {
                case TypeStep.Missile:
                    {
                        if (!cast(53)) return false;
                        boss_hit -= 4;
                        break;
                    }
                case TypeStep.Drain:
                    {
                        if (!cast(73)) return false;
                        player_hit = Math.Min(50, player_hit + 2);
                        boss_hit -= 2;
                        break;
                    }
                case TypeStep.Shield:
                    {
                        if (!cast(113) || shield > 0) return false;
                        shield = 6;
                        break;
                    }
                case TypeStep.Poison:
                    {
                        if (!cast(173) || poison > 0) return false;
                        poison = 6;
                        break;
                    }
                case TypeStep.Recharge:
                    {
                        if (!cast(229) || recharge > 0) return false;
                        recharge = 5;
                        break;
                    }
            }
            return true;
        }

        public int GetMinMana(ref int currResult, bool hard = false)
        {

            // Boss step
            CounterCalc();
            if (boss_hit <= 0) return player_mana_summary;
            player_hit -= Math.Max(boss_damage - (shield > 0 ? 7 : 0), 1);
            if (hard) --player_hit;
            if (player_hit <= 0) return -1;

            // Player step
            CounterCalc();
            if (boss_hit <= 0) return player_mana_summary;
            if (!cast() || player_mana_summary >= currResult)
                return -1;
            if (boss_hit <= 0) return player_mana_summary;
            var ss = GetNextPlayerSteps();
            int result = int.MaxValue;
            foreach (var s in ss)
            {
                var variant = s.GetMinMana(ref currResult, hard);
                if (variant >= 0)
                {
                    result = Math.Min(variant, result);
                    if (result < currResult)
                        currResult = result;
                }
            }

            return result < int.MaxValue ? result : -1;
        }
    }

    internal class Day22 : DayPattern<Queue<Step>>
    {
        public override void Parse(string path)
        {
            data = new Queue<Step>();
            Step.boss_damage = 9;
        }

        public override string PartOne()
        {
            data.Clear();
            var f = new Step { player_hit = 50 + Step.boss_damage, player_mana = 500, boss_hit = 58 };
            f.AddAllPlayerSteps(data);
            var result = int.MaxValue;
            foreach (var s in data)
            {
                var m = s.GetMinMana(ref result);
                if (m > 0 && m < result)
                    result = m;
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            data.Clear();
            var f = new Step { player_hit = 50 + Step.boss_damage, player_mana = 500, boss_hit = 58 };
            f.AddAllPlayerSteps(data);
            var result = int.MaxValue;
            foreach (var s in data)
            {
                var m = s.GetMinMana(ref result, true);
                if (m > 0 && m < result)
                    result = m;
            }
            return result.ToString();
        }
    }
}