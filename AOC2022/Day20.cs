using AnyThings;

namespace AOC2022
{
    internal class Day20 : DayPattern<List<NumberNode>>
    {
        public override void Parse(string singleText)
        {
            var text = singleText.Split(Environment.NewLine).ToList();
            data = new List<NumberNode>();
            foreach (var item in text)
            {
                data.Add(new NumberNode(int.Parse(item), data.Count > 0 ? data[^1] : null, null));
                if (data.Count > 1)
                    data[^2].Next = data[^1];
                if (data[^1].value == 0)
                    start = data[^1];
            }
            data[0].Prev = data[^1];
            data[^1].Next = data[0];
        }

        public override string PartOne()
        {
          /*  (int,int)[] positions = new (int, int)[data.Count];
            int[] numbers = new int[data.Count];
            for (var i = 0; i < data.Count; ++i)
            {
                numbers[i] = data[i].value;
                positions[i] = (data[i].value,i);
            }

            for(var i=0; i < positions.Length; ++i)
            {
                if (positions[i].Item1 == 0) continue;
                var currPos = positions[i].Item2;
            }*/


            foreach (var n in data)
                n.Move3();
           // n.Move2(n.value.mod(data.Count));
            var x = start.GetNumberDistance(1000/* % data.Count*/);
            var y = start.GetNumberDistance(2000/* % data.Count*/);
            var z = start.GetNumberDistance(3000 /*% data.Count*/);
            return (x + y + z).ToString();
        }

        public override string PartTwo()
        {
            var result = 1;
            /*   for (var bp = 0; bp < data.Count; ++bp)
                   result = 0;*/
            return result.ToString();
        }

        NumberNode start;
    }

    class NumberNode
    {
        public NumberNode Next = null;
        public NumberNode Prev = null;

         int _value;
        public int value { get { return _value; } }

        public NumberNode(int v, NumberNode prev, NumberNode next)
        {
            _value = v;
            Prev = prev;
            Next = next;
        }

        internal void Move(int distance, ref NumberNode start)
        {
            var nn = this;
            if (distance > 0)
                for (var i = 0; i < distance; ++i)
                    nn = nn.Next;
            else
                for (var i = 0; i <= -distance; ++i)
                    nn = nn.Prev;
            if (nn != this)
            {
                if (nn == start)
                    start = this;
                else if (start == this)
                {
                    if (value > 0)
                        start = Next;
                    else
                        start = Prev;
                }
                Prev.Next = Next;
                Next.Prev = Prev;

                Prev = nn;
                Next = nn.Next;

                nn.Next.Prev = this;
                nn.Next = this;

            }
        }

        internal void Move()
        {
            var nn = this;
            if (value == 0) return;
            if (value > 0)
            {
                for (var i = 0; i < value; ++i)
                    nn = nn.Next;
                if (nn == this || nn == this.Prev) return;
                Prev.Next = Next;
                Next.Prev = Prev;

                Prev = nn;
                Next = nn.Next;

                nn.Next.Prev = this;
                nn.Next = this;
            }
            else
            {
                for (var i = 0; i < -value; ++i)
                    nn = nn.Prev;
                if (nn == this || nn == this.Next) return;
                Prev.Next = Next;
                Next.Prev = Prev;

                Prev = nn.Prev;
                Next = nn;

                nn.Prev.Next = this;
                nn.Prev = this;
            }
        }

        internal void Move2(int dist)
        {
            if (dist == 0) return;
            var nn = this;
            for (var i = 0; i < dist; ++i)
                nn = nn.Next;
            if (nn == this)
                return;
            if (nn == this.Prev)
                return;
            Prev.Next = Next;
            Next.Prev = Prev;

            Prev = nn;
            Next = nn.Next;

            nn.Next.Prev = this;
            nn.Next = this;
        }

        internal void Move3()
        {
            var nn = this;
            if (value == 0) return;
            if (value > 0)
                for (var i = 0; i < value; ++i)
                    nn = nn.Next;
            else
            {
                for (var i = 0; i <= -value; ++i)
                    nn = nn.Prev;
            }
            if (nn == this || nn == this.Prev /*|| nn == this.Next*/) return;

            Prev.Next = Next;
            Next.Prev = Prev;

            Prev = nn;
            Next = nn.Next;

            nn.Next.Prev = this;
            nn.Next = this;
        }

        public int GetNumberDistance(int distance)
        {
            var nn = this;
            for (var i = 0; i < distance; ++i)
                nn = nn.Next;
            return nn.value;
        }
    }

    public static class ExtNum
    {
        public static int mod(this int a, int n)
        {
            int result = a % n;
            if ((result < 0 && n > 0) || (result > 0 && n < 0))
            {
                result += n;
            }
            return result;
        }
    }
}