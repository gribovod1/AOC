using AnyThings;

namespace AOC2023
{
    internal class Day03 : DayPattern<string[]>
    {
        public override void Parse(string singleText)
        {
            data = singleText.Split(Environment.NewLine);
        }

        public override string PartOne()
        {
            /*
             Перебором проверяются все символы и если найдено число, то проверить символы вокруг него и если найден символ не буквы, то
             прибавить это число к общей сумме
             */
            int result = 0;
            for (int i = 0; i < data.Length; ++i) {
                int j = 0;
                while (j < data[i].Length){
                    if (char.IsDigit(data[i][j])) {
                        int end = j + 1;
                        while (end < data[i].Length && char.IsDigit(data[i][end])) {
                        ++end;
                        }
                        int number = int.Parse(data[i].Substring(j, end - j));
                        if (IsNearDetail(data, i, j, end - j, gear, number)) {
                            result += number;
                        }
                        j = end + 1;
                    } else {
                        ++j;
                    }
                }
            }
            return result.ToString();
        }

        public override string PartTwo()
        {
            /*
             Ранее, при поиске всех "не букв", были выделены числа, которые соседствуют с '*'
             */
            Int64 result2 = 0;
            foreach(var g in gear) {
                if (g.Value.count == 2)
                result2 += g.Value.value;
            }
            return result2.ToString();
        }

        Dictionary<Int64, (int count, Int64 value)> gear = new();
        private static bool IsNearDetail(string[] data, int row, int start, int Length, Dictionary<Int64, (int count, Int64 value)> gear, int number)
        {
          int p1x = start - 1;
          int p1y = row - 1;
          int p2x = start + Length;
          int p2y = row + 1;
          for (int i = p1y; i <= p2y; ++i)
            for(int j = p1x; j <= p2x; ++j) {
              if (i >= 0 && i < data.Length && j >= 0 && j < data[i].Length && data[i][j] != '.' && !char.IsLetterOrDigit(data[i][j])) {
                if (data[i][j] == '*') {
                    // (Вторая часть) Координаты звёздочки используются как ключ, чтобы перемножить числа, соседствующие с этой звёздочкой
                  if (gear.ContainsKey(i * data[i].Length + j))
                    gear[i * data[i].Length + j] = (gear[i * data[i].Length + j].count + 1,gear[i * data[i].Length + j].value * number);
                  else 
                    gear[i * data[i].Length + j] = (1, number);
                }
                return true;
              }
            }
          return false;
        }
    }
}