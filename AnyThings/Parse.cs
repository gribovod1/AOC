using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyThings
{
    public class Parse
    {
        public static char[,] PathToCharMap(string path)
        {
            var data = File.ReadAllLines(path);
            char[,] temp = new char[data[1].Length, data[0].Length];
            for (var yc = 0; yc < data.Length; ++yc)
                for (var xc = 0; xc < data[yc].Length; ++xc)
                    temp[xc, yc] = data[yc][xc];
            return temp;
        }

        public static char[,] TextToCharMap(string path)
        {
            var data = File.ReadAllLines(path);
            char[,] temp = new char[data[1].Length, data[0].Length];
            for (var yc = 0; yc < data.Length; ++yc)
                for (var xc = 0; xc < data[yc].Length; ++xc)
                    temp[xc, yc] = data[yc][xc];
            return temp;
        }

        public static int[,] ParseToIntMap(string path)
        {
            var data = File.ReadAllLines(path);
            int[,] temp = new int[data[1].Length, data[0].Length];
            for (var yc = 0; yc < data.Length; ++yc)
                for (var xc = 0; xc < data[yc].Length; ++xc)
                    temp[xc, yc] = data[yc][xc] - 0x30;
            return temp;
        }

        public static Record parseToRecord(string line, string divider)
        {
            var result = new Record();
            return result;
        }

        public static List<int> parseToList(string line, string divider)
        {
            var result = new List<int>();
            return result;
        }

        public static List<KeyValuePair<string, int>> parseToKeyValue(string line, string keyValueDivider, string divider)
        {
            var result = new List<KeyValuePair<string, int>>();
            int prevEnd = 0;
            int index = line.IndexOf(keyValueDivider);
            while (index >= 0)
            {
                int end = line.IndexOf(divider, index);
                if (end < 0)
                    end = line.Length;
                result.Add(new KeyValuePair<string, int>(line.Substring(prevEnd, index - prevEnd), int.Parse(line.Substring(index+keyValueDivider.Length, end - index + keyValueDivider.Length))));
                prevEnd = end;
                index = line.IndexOf(keyValueDivider, prevEnd);
            }
            return result;
        }

        public struct Record
        {
        }
    }
}
