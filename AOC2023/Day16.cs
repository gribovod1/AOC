using AnyThings;

namespace AOC2023
{
    internal class Day16 : DayPattern<char[,]>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            data = new char[text[0].Length, text.Length];
            for (int row = 0; row < text.Length; row++)
                for (int col = 0; col < text[row].Length; col++)
                    data[col, row] = text[row][col];
        }

        public override string PartOne()
        {
            /* Трассировка лучей будет заключаться в последовательной обработке плиток с лучами, при этом каждый шаг луча
             * добавляет в обработку новую плитку, но только если плитка с указанным напралением луча ещё не обрабатывалась
             * (иначе она полностью повторит уже пройденный путь).
             */
            HashSet<(int x, int y)> EnabledMirror = new();
            FillEnabledMirror(new Ray(0, 0, 1, 0), EnabledMirror);
            return EnabledMirror.Count.ToString();
        }

        struct Ray
        {
            public int x;
            public int y;
            public int dx;
            public int dy;

            public Ray(int x, int y, int dx, int dy)
            {
                this.x = x;
                this.y = y;
                this.dx = dx;
                this.dy = dy;
            }
        }
        private void FillEnabledMirror(Ray ray, HashSet<(int x, int y)> Enabled)
        {
            Queue<Ray> Tracing = new();
            HashSet<Ray> Rays = new();
            Tracing.Enqueue(ray);
            while (Tracing.Count > 0)
                ProcessRay(Tracing, Enabled, Rays);
        }

        void ProcessRay(Queue<Ray> Trassing, HashSet<(int x, int y)> Enabled, HashSet<Ray> Rays)
        {
            Ray ray = Trassing.Dequeue();
            if (Rays.Contains(ray)) return;
            if (ray.x < 0 || ray.y < 0 || ray.x >= data.GetLength(0) || ray.y >= data.GetLength(1)) return;
            Rays.Add(ray);
            Enabled.Add((ray.x, ray.y));
            switch (data[ray.x, ray.y])
            {
                case '.':
                    {
                        Trassing.Enqueue(new Ray(ray.x + ray.dx, ray.y + ray.dy, ray.dx, ray.dy));
                        break;
                    }
                case '\\':
                    {
                        if (ray.dx < 0)
                            Trassing.Enqueue(new Ray(ray.x, ray.y - 1, 0, -1));
                        else
                        if (ray.dx > 0)
                            Trassing.Enqueue(new Ray(ray.x, ray.y + 1, 0, 1));
                        else if (ray.dy < 0)
                            Trassing.Enqueue(new Ray(ray.x - 1, ray.y, -1, 0));
                        else
                            Trassing.Enqueue(new Ray(ray.x + 1, ray.y, 1, 0));
                        break;
                    }
                case '|':
                    {
                        if (ray.dx != 0)
                        {
                            Trassing.Enqueue(new Ray(ray.x, ray.y - 1, 0, -1));
                            Trassing.Enqueue(new Ray(ray.x, ray.y + 1, 0, 1));
                        }
                        else
                            Trassing.Enqueue(new Ray(ray.x + ray.dx, ray.y + ray.dy, ray.dx, ray.dy));
                        break;
                    }
                case '/':
                    {
                        if (ray.dx < 0)
                            Trassing.Enqueue(new Ray(ray.x, ray.y + 1, 0, +1));
                        else if (ray.dx > 0)
                            Trassing.Enqueue(new Ray(ray.x, ray.y - 1, 0, -1));
                        else if (ray.dy < 0)
                            Trassing.Enqueue(new Ray(ray.x + 1, ray.y, +1, 0));
                        else
                            Trassing.Enqueue(new Ray(ray.x - 1, ray.y, -1, 0));
                        break;
                    }
                case '-':
                    {
                        if (ray.dy != 0)
                        {
                            Trassing.Enqueue(new Ray(ray.x - 1, ray.y, -1, 0));
                            Trassing.Enqueue(new Ray(ray.x + 1, ray.y, 1, 0));
                        }
                        else
                            Trassing.Enqueue(new Ray(ray.x + ray.dx, ray.y + ray.dy, ray.dx, ray.dy));
                        break;
                    }
            }
        }

        public override string PartTwo()
        {
            /* Процедура трассировки запускается последовательно для всех краёв карты и выбирается максимум */
            int result = 0;
            HashSet<(int x, int y)> EnabledMirror = new();
            for (int x = 0; x < data.GetLength(0); ++x)
            {
                EnabledMirror.Clear();
                FillEnabledMirror(new Ray(x, 0, 0, 1), EnabledMirror);
                if (result < EnabledMirror.Count)
                    result = EnabledMirror.Count;
                EnabledMirror.Clear();
                FillEnabledMirror(new Ray(x, data.GetLength(1) - 1, 0, -1), EnabledMirror);
                if (result < EnabledMirror.Count)
                    result = EnabledMirror.Count;
            }
            for (int y = 0; y < data.GetLength(1); ++y)
            {
                EnabledMirror.Clear();
                FillEnabledMirror(new Ray(0, y, 1, 0), EnabledMirror);
                if (result < EnabledMirror.Count)
                    result = EnabledMirror.Count;
                EnabledMirror.Clear();
                FillEnabledMirror(new Ray(data.GetLength(0) - 1, y, -1, 0), EnabledMirror);
                if (result < EnabledMirror.Count)
                    result = EnabledMirror.Count;
            }
            return result.ToString();
        }
    }
}