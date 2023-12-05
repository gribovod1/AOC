using AnyThings;

namespace AOC2023
{
    internal class Day05 : DayPattern<(Int64[] Seeds, List<List<(Int64 StartSource, Int64 DestinationMove, Int64 Length)>> Maps)>
    {
        public override void Parse(string singleText)
        {
            var maps = singleText.Split(Environment.NewLine + Environment.NewLine);
            string[] SeedsTExt = maps[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Int64[] Seeds = new Int64[SeedsTExt.Length - 1];
            for (int i = 1; i < SeedsTExt.Length; i++) { Seeds[i - 1] = Int64.Parse(SeedsTExt[i]); }

            List<List<(Int64 StartSource, Int64 StartDestination, Int64 Length)>> Maps = new();
            for (var m = 1; m < maps.Length; m++)
            {
                List<(Int64 StartSource, Int64 StartDestination, Int64 Length)> map = new();
                string[] mapText = maps[m].Split(Environment.NewLine);
                for (int d = 1; d < mapText.Length; d++)
                {
                    string[] dText = mapText[d].Split(' ');
                    var startSource = Int64.Parse(dText[1]);
                    var startDestination = Int64.Parse(dText[0]);
                    var length = Int64.Parse(dText[2]);
                    map.Add((startSource, startDestination - startSource, length));
                }
                map.Sort(MapSort);
                Maps.Add(map);
            }
            data = (Seeds, Maps);
        }

        int MapSort((Int64 StartSource, Int64 StartDestination, Int64 Length) x, (Int64 StartSource, Int64 StartDestination, Int64 Length) y)
        {
            return x.StartSource.CompareTo(y.StartSource);
        }

        public override string PartOne()
        {
            // Находятся попадание в диапазон и проверяются на уровень ниже
            Int64 result = int.MaxValue;
            for (int i = 0; i < data.Seeds.Length; ++i)
            {
                Int64 value = FindEnd2(0, data.Seeds[i], 1);
                if (result > value)
                    result = value;
            }
            return result.ToString();
        }

        Int64 FindEnd(int CurrentMap, Int64 CurrentIndex)
        {
            if (CurrentMap >= data.Maps.Count) return CurrentIndex;
            for (var d = 0; d < data.Maps[CurrentMap].Count; ++d)
            {
                if (CurrentIndex >= data.Maps[CurrentMap][d].StartSource && CurrentIndex <= data.Maps[CurrentMap][d].StartSource + data.Maps[CurrentMap][d].Length)
                    return FindEnd(CurrentMap + 1, CurrentIndex + data.Maps[CurrentMap][d].DestinationMove);
            }
            return FindEnd(CurrentMap + 1, CurrentIndex);
        }

        public override string PartTwo()
        {
            // Находятся пересечения дапазонов и проверяются на уровень карт ниже
            Int64 result = int.MaxValue;
            for (int i = 0; i < data.Seeds.Length; i += 2)
            {
                Int64 value = FindEnd2(0, data.Seeds[i], data.Seeds[i + 1]);
                if (result > value)
                    result = value;
            }
            return result.ToString();
        }

        Int64 FindEnd2(int CurrentMap, Int64 CurrentIndex, Int64 CurrentLength)
        {
            if (CurrentMap >= data.Maps.Count) return CurrentIndex;
            Int64 result = Int64.MaxValue;
            Int64 value = 0;
            var d = 0;
            while ((d < data.Maps[CurrentMap].Count) && (CurrentLength > 0) && (CurrentIndex + CurrentLength >= data.Maps[CurrentMap][d].StartSource))
            {
                if (data.Maps[CurrentMap][d].StartSource > CurrentIndex)
                {
                    var l = Math.Min(CurrentLength, data.Maps[CurrentMap][d].StartSource - CurrentIndex);
                    value = FindEnd2(CurrentMap + 1, CurrentIndex, l);
                    if (result > value) result = value;
                    CurrentLength -= l;
                    CurrentIndex += l;
                } else if (data.Maps[CurrentMap][d].StartSource <= CurrentIndex && data.Maps[CurrentMap][d].StartSource + data.Maps[CurrentMap][d].Length >= CurrentIndex)
                {
                    var l = Math.Min(CurrentLength,Math.Min(CurrentLength, data.Maps[CurrentMap][d].Length - (CurrentIndex - data.Maps[CurrentMap][d].StartSource)));
                    value = FindEnd2(CurrentMap + 1, CurrentIndex + data.Maps[CurrentMap][d].DestinationMove, l);
                    if (result > value) result = value;
                    CurrentLength -= l;
                    CurrentIndex += l;
                    ++d;
                } else
                {
                    ++d;
                }
            }
            if (CurrentLength > 0)
            {
                value = FindEnd2(CurrentMap + 1, CurrentIndex, CurrentLength);
                if (result > value) result = value;
            }
            return result;
        }
    }
}