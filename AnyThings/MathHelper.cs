using System;
using System.Collections.Generic;

namespace AnyThings
{
    public static class MathHelper
    {
        public static UInt64 GetGCD(UInt64 a, UInt64 b)
        {
            while (b != 0)
            {
                UInt64 temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static UInt64 GetLCM(UInt64 a, UInt64 b)
        {
            return a * b / GetGCD(a, b);
        }

        public static UInt64 GetLCM(List<UInt64> numbers)
        {
            if (numbers.Count == 0) return 0;
            if (numbers.Count == 1) return numbers[0];
            UInt64 result = numbers[0];
            for(int i =1;i < numbers.Count;i++)
            {
                result = GetLCM(result, numbers[i]);
            }
            return result;
        }
    }
}
