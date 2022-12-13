using AnyThings;

namespace AOC2022
{

    class Round
    {
        public string Elve;
        public string You;
        public int Points = 0;

        public Round(string elve, string you)
        {
            Elve = elve;
            You = you;
        }

        public int getPoints(string elve, string you)
        {
            int points = 0;
            switch (you)
            {
                case "X":
                    {//Stone
                        points = 1;
                        if (elve == "A") points += 3;
                        if (elve == "C") points += 6;
                        break;
                    }
                case "Y":
                    {//Paper
                        points = 2;
                        if (elve == "B") points += 3;
                        if (elve == "A") points += 6;
                        break;
                    }
                case "Z":
                    {//Scissor
                        points = 3;
                        if (elve == "C") points += 3;
                        if (elve == "B") points += 6;
                        break;
                    }
            }
            return points;
        }

        public int getPoints2(string elve, string you)
        {
            int points = 0;
            switch (you)
            {
                case "X":
                    {
                        points = 0;
                        if (elve == "A") points += 3;
                        if (elve == "B") points += 1;
                        if (elve == "C") points += 2;
                        break;
                    }
                case "Y":
                    {
                        points = 3;
                        if (elve == "A") points += 1;
                        if (elve == "B") points += 2;
                        if (elve == "C") points += 3;
                        break;
                    }
                case "Z":
                    {
                        points = 6;
                        if (elve == "A") points += 2;
                        if (elve == "B") points += 3;
                        if (elve == "C") points += 1;
                        break;
                    }
            }
            return points;
        }
    }
    internal class Day02 : DayPattern<List<Round>>
    {
        public override void Parse(string path)
        {
            var text = File.ReadAllText(path);
            var scanText = text.Split(Environment.NewLine);
            data = new List<Round>();
            for (var i = 0; i < scanText.Length; ++i)
            {
                var vs = scanText[i].Split(' ');
                data.Add(new Round(vs[0], vs[1]));
            }
        }

        public override string PartOne()
        {
            int result = 0;
            foreach (var r in data)
            {
                result += r.getPoints(r.Elve, r.You);
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            int result = 0;
            foreach (var r in data)
            {
                result += r.getPoints2(r.Elve, r.You);
            }
            return result.ToString();
        }
    }
}