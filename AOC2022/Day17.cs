using AnyThings;

namespace AOC2022
{
    internal class Day17 : DayPattern<string>
    {
        public override void Parse(string singleText)
        {
            data = singleText;
            StonePatterns.Add(new int[]{0b000111100});
            StonePatterns.Add(new int[]{0b000010000, 0b000111000, 0b000010000});
            StonePatterns.Add(new int[]{0b000111000, 0b000001000, 0b000001000});
            StonePatterns.Add(new int[]{0b000100000, 0b000100000, 0b000100000, 0b000100000});
            StonePatterns.Add(new int[]{0b000110000, 0b000110000});
        }

        public override string PartOne()
        {
            CurrentPattern = StonePatterns.Count - 1;
            StreamNumber = data.Length - 1;
            for (var i = 0; i < 2022; ++i)
            {
                NewStone();
                do
                {
                    HorizontalMove();
                }
                while (VerticalMove());
                AnotherBrickInTheWall();
            }

            return $"Tower size: {Tower.Count}";
        }

        public override string PartTwo()
        {
            return 0.ToString();
        }

        void NewStone() {
            CurrentPattern = (CurrentPattern + 1) % StonePatterns.Count;
            CurrentCoordinate = Tower.Count + 3;
            CurrentStone = (int[])StonePatterns[CurrentPattern].Clone();
        }

        void AnotherBrickInTheWall() {
            for(int p = 0; p <CurrentStone.Length; ++p) {
                if (CurrentCoordinate + p < Tower.Count)
                    Tower[CurrentCoordinate + p] |= CurrentStone[p];
                else
                    Tower.Add(CurrentStone[p]);
            }
        }

        void HorizontalMove() {
            // Если шаблоны камня и башни не накладываются после перемещения (нет совпадающих '1'), то камень можно переместить 
            // Перемещение в горизонтали - смещение бит в шаблоне камня по направлению потока
            StreamNumber = (StreamNumber + 1) % data.Length;
            for(int p = 0; p < CurrentStone.Length; ++p) {
                int value = data[StreamNumber] == '<' ? CurrentStone[p] << 1 : CurrentStone[p] >> 1;
                if (((value & 0b100000001) != 0) || ((CurrentCoordinate + p < Tower.Count) && (Tower[CurrentCoordinate + p] & value) != 0))
                    return;
            }
            for(int p = 0; p < CurrentStone.Length; ++p) {
                CurrentStone[p] = data[StreamNumber] == '<' ? CurrentStone[p] << 1 : CurrentStone[p] >> 1;
            }
        }

        bool VerticalMove() {
             // Если шаблоны камня и башни не накладываются после перемещения (нет совпадающих '1'), то камень можно переместить
             // перемещение по вертикали - смещение координаты камня вниз
           var tempCoordinate = CurrentCoordinate - 1;
            if (tempCoordinate < 0) {
                return false;
            }
            for(int p = 0; p < CurrentStone.Length; ++p) {
                if ((tempCoordinate + p < Tower.Count) && (Tower[tempCoordinate + p] & CurrentStone[p]) != 0)
                    return false;
            }
            CurrentCoordinate = tempCoordinate;
            return true;
        }

        List<int[]> StonePatterns = new ();
        List<int> Tower =new();
        List<int> Hashes = new();
        int[] CurrentStone;
        int CurrentCoordinate;

        int CurrentPattern = 0;

        int StreamNumber = 0;
    }
}