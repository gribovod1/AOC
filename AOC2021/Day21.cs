using AnyThings;
using System;
using System.Collections.Generic;

namespace AOC2021
{
    class Day21 : DayPattern<Game21>
    {
        public override void Parse(string path)
        {
            data = new Game21(6, 8);
        }

        public override string PartOne()
        {
            while (!data.end())
                data.step();
            return (data.PitchCount * Math.Min(data.Points1, data.Points2)).ToString();
        }

        public override string PartTwo()
        {
            var states = new List<UniverseState>() { new UniverseState(0, 0, 0, 6, 8, 1) };
            game_two(states);

            ulong p1Wins = 0;
            ulong p2Wins = 0;
            foreach (var s in states)
                if (s.PlayerWin == 1)
                    p1Wins += s.UniverseCount;
                else
                    p2Wins += s.UniverseCount;
            return Math.Max(p1Wins, p2Wins).ToString();
        }

        /// <summary>
        /// Three pitches of dice for one player and calculate states
        /// </summary>
        /// <param name="current">Current player</param>
        /// <param name="two">Other player</param>
        /// <returns>Return "true" if any state is modifyed</returns>
        void game_two(List<UniverseState> states)
        {
            bool m1;
            bool m2;
            do
            {
                m1 = pitch(1, states);
                m2 = pitch(2, states);
            }
            while (m1 || m2);
        }

        bool pitch(int player, List<UniverseState> states)
        {
            var modify = false;
            var CurrentPlayerOldStates = new List<UniverseState>(states);
            states.Clear();
            for (var index = 0; index < CurrentPlayerOldStates.Count; ++index)
                if (!CurrentPlayerOldStates[index].End)
                {
                    modify |= true;
                    foreach (var su in StepsAndUniverses)
                        states.Add(CurrentPlayerOldStates[index].walk(player, su.Key, su.Value));
                }
                else
                    states.Add(CurrentPlayerOldStates[index]);

            if (modify)
                collapseUniverses(states);
            return modify;
        }

        private static void collapseUniverses(List<UniverseState> states)
        {
            for (var index = 0; index < states.Count; ++index)
                for (var next = index + 1; next < states.Count; ++next)
                    if (states[index].merge(states[next]))
                    {
                        states.RemoveAt(next);
                        --next;
                    }
        }

        /// <summary>
        /// Steps count - Universe count
        /// </summary>
        List<KeyValuePair<int, ulong>> StepsAndUniverses = new List<KeyValuePair<int, ulong>>() {   new KeyValuePair<int, ulong>(3,1),
                                                                                                    new KeyValuePair<int, ulong>(4,3),
                                                                                                    new KeyValuePair<int, ulong>(5,6),
                                                                                                    new KeyValuePair<int, ulong>(6,7),
                                                                                                    new KeyValuePair<int, ulong>(7,6),
                                                                                                    new KeyValuePair<int, ulong>(8,3),
                                                                                                    new KeyValuePair<int, ulong>(9,1)};
    }

    internal class Game21
    {
        internal int Dice;
        internal int PitchCount;
        internal int StepCount;
        internal int Points1;
        internal int Points2;
        private int Start1;
        private int Start2;
        private int Position1;
        private int Position2;

        public Game21(int v1, int v2)
        {
            this.Start1 = v1;
            this.Start2 = v2;
            Position1 = Start1 - 1;
            Position2 = Start2 - 1;
            PitchCount = 0;
            Dice = 100;
        }

        internal bool end()
        {
            return Points1 >= 1000 || Points2 >= 1000;
        }

        internal void step()
        {
            if (end()) return;
            ++StepCount;
            var points = pitch() + pitch() + pitch();
            if ((StepCount & 1) == 1)
            {
                Position1 = (Position1 + points) % 10;
                Points1 += Position1 + 1;
            }
            else
            {
                Position2 = (Position2 + points) % 10;
                Points2 += Position2 + 1;
            }
        }

        int pitch()
        {
            ++PitchCount;
            ++Dice;
            if (Dice > 100)
                Dice = 1;
            return Dice;
        }
    }

    class UniverseState
    {
        public int Points1;
        public int Points2;
        public int Steps;
        public int Position1;
        public int Position2;
        public ulong UniverseCount;

        public UniverseState(int points1, int points2, int steps, int position1, int position2, ulong universeCount)
        {
            Points1 = points1;
            Points2 = points2;
            Steps = steps;
            Position1 = position1;
            Position2 = position2;
            UniverseCount = universeCount;
        }

        public bool End
        {
            get
            {
                return Points1 >= 21 || Points2 >= 21;
            }
        }

        public int PlayerWin
        {
            get
            {
                return Points1 >= 21 ? 1 : (Points2 >= 21 ? 2 : 0);
            }
        }

        public override bool Equals(object obj)
        {
            var s = obj as UniverseState;
            if (s == null)
                return false;
            if (End && PlayerWin == s.PlayerWin)
                return Steps == s.Steps;
            else
                return Points1 == s.Points1 && Points2 == s.Points2 && Steps == s.Steps && Position1 == s.Position1 && Position2 == s.Position2;
        }

        public bool merge(UniverseState state)
        {
            if (Equals(state))
            {
                UniverseCount += state.UniverseCount;
                return true;
            }
            return false;
        }

        internal UniverseState walk(int player, int steps, ulong universeCount)
        {
            var newPosition = (player == 1 ? Position1 : Position2) + steps;
            if (newPosition > 10)
                newPosition = newPosition - 10;
            var points1 = Points1 + (player == 1 ? newPosition : 0);
            var points2 = Points2 + (player == 2 ? newPosition : 0);
            var position1 = player == 1 ? newPosition : Position1;
            var position2 = player == 2 ? newPosition : Position2;
            return new UniverseState(points1, points2, Steps + 1, position1, position2, UniverseCount * universeCount);
        }
    }
}