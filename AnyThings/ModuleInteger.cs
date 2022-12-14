using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AnyThings
{
    public class ModuleInteger
    {
        static int[] SimpleNumbers = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101 };

        internal Dictionary<int, int> Modules = new Dictionary<int, int>();
        public ModuleInteger(int value)
        {
            foreach (var sn in SimpleNumbers)
            {
                var m = value % sn;
                if (m != 0) Modules.Add(sn, m);
            }
        }

        ModuleInteger()
        {

        }

        public static ModuleInteger operator +(ModuleInteger x, ModuleInteger y)
        {
            var result = new ModuleInteger();
            var ms = x.Modules.Keys.Union(y.Modules.Keys);
            foreach (var m in ms)
            {
                var rm = ((x.Modules.ContainsKey(m) ? x.Modules[m] : 0) + (y.Modules.ContainsKey(m) ? y.Modules[m] : 0)) % m;
                if (rm != 0)
                    result.Modules.Add(m, rm);
            }
            return result;
        }

        public static ModuleInteger operator *(ModuleInteger x, ModuleInteger y)
        {
            var result = new ModuleInteger();
            var ms = x.Modules.Keys.Intersect(y.Modules.Keys);
            foreach (var m in ms)
            {
                var rm = (x.Modules[m] * y.Modules[m]) % m;
                if (rm != 0)
                    result.Modules.Add(m, rm);
            }
            return result;
        }

        public static ModuleInteger operator +(ModuleInteger x, int y)
        {
            return x + new ModuleInteger(y);
        }

        public static ModuleInteger operator *(ModuleInteger x, int y)
        {
            return x * new ModuleInteger(y);
        }

        public static ModuleInteger operator /(ModuleInteger x, int y)
        {
            return new ModuleInteger(y);
        }

        public bool IsSimpleDivided(int divisor)
        {
            return !Modules.ContainsKey(divisor);
        }
    }
}