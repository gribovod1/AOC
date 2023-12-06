using AnyThings;

namespace AOC2023
{
    internal class Day06 : DayPattern<List<(Int64 Time, Int64 Record)>>
    {
        public override void Parse(string singleText)
        {
            var table = singleText.Split(Environment.NewLine);
            var times = table[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var distance = table[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            data = new();
            for (int i = 1; i < times.Length; i++)
                data.Add((Int64.Parse(times[i]), Int64.Parse(distance[i])));
            // For two part
            data.Add((Int64.Parse(table[0].Substring(table[0].IndexOf(':') + 1).Replace(" ", string.Empty)), Int64.Parse(table[1].Substring(table[1].IndexOf(':') + 1).Replace(" ", string.Empty))));
        }

        public override string PartOne()
        {
            // Решение - расстояние между двумя корнями квадратного уравнения
            // -x^2 + Time * x - Record > 0
            // где x - время удержания кнопки, Time - время заезда, Record - текущий рекорд
            Int64 result = 1;
            for (int i = 0; i < data.Count - 1; ++i)
                result *= GetRange(i);
            return result.ToString();
        }

        Int64 GetRange(int index)
        {
            var d = data[index].Time * data[index].Time - 4 * data[index].Record;
            var x1 = (data[index].Time - Math.Sqrt(d)) / 2;
            var x2 = (data[index].Time + Math.Sqrt(d)) / 2;
            if (Math.Abs(Math.Ceiling(x1) - x1) < double.Epsilon)
                x1 = Math.Ceiling(x1) + 1;
            else
                x1 = Math.Ceiling(x1);
            if (Math.Abs(Math.Floor(x2) - x2) < double.Epsilon)
                x2 = Math.Floor(x2) - 1;
            else
                x2 = Math.Floor(x2);
            return (Int64)(Math.Abs(x2 - x1)) + 1;
        }

        public override string PartTwo()
        {
            // Всё то же самое
            return GetRange(data.Count - 1).ToString();
        }
    }
}