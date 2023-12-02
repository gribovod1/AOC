using AnyThings;

namespace AOC2023
{
    internal class Day2 : DayPattern<List<List<GameRound_Day2>>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine).ToList();
            data = new List<List<GameRound_Day2>>();
            foreach (var g in text)
            {
                data.Add(new List<GameRound_Day2> { });
                var index = g.IndexOf(":");
                var round = g.Substring(index + 1).Split(';', StringSplitOptions.RemoveEmptyEntries);
                for (var r = 0; r < round.Length; r++)
                {
                    var colors = round[r].Replace(',', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var game = new GameRound_Day2();
                    int cIndex = 1;
                    while (cIndex < colors.Length)
                    {
                        if (colors[cIndex] == "red")
                            game.red = int.Parse(colors[cIndex - 1]);
                        if (colors[cIndex] == "green")
                            game.green = int.Parse(colors[cIndex - 1]);
                        if (colors[cIndex] == "blue")
                            game.blue = int.Parse(colors[cIndex - 1]);
                        cIndex += 2;
                    }
                    data[data.Count - 1].Add(game);
                }
            }
        }

        public override string PartOne()
        {
            /*
             Перебором проверяются раунды, не превышает ли количество кубиков заданное.
             */
            int result = 0;
            for (int i = 0; i < data.Count; ++i)
                if (GetValidGame(i, 12, 13, 14))
                    result += i + 1;
            return result.ToString();
        }

        public override string PartTwo()
        {
            /*
             Перебором находятся минимальные количества выпадавших кубиков
             */
            Int64 result = 0;
            for (int i = 0; i < data.Count; ++i)
            {
                (int r, int g, int b) = GetMinimum(i);
                result += r * g * b;
            }
            return result.ToString();
        }

        private bool GetValidGame(int game, int red, int green, int blue)
        {
            for (int i = 0; i < data[game].Count(); ++i)
            {
                if (data[game][i].red > red) return false;
                if (data[game][i].green > green) return false;
                if (data[game][i].blue > blue) return false;
            }
            return true;
        }

        private (int r, int g, int b) GetMinimum(int game)
        {
            int r = 0; int g = 0; int b = 0;
            for (int i = 0; i < data[game].Count(); ++i)
            {
                if (data[game][i].red > r) r = data[game][i].red;
                if (data[game][i].green > g) g = data[game][i].green;
                if (data[game][i].blue > b) b = data[game][i].blue;
            }
            return (r, g, b);
        }
    }

    public class GameRound_Day2
    {
        public int red = 0;
        public int green = 0;
        public int blue = 0;
    }
}