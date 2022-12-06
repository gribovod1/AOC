using AnyThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2021
{

    class Packet
    {
        List<Packet> content = new List<Packet>();

        long _Value;
        public long Value
        {
            get
            {
                long value = 0;
                switch (Type)
                {
                    case 0:
                        {
                            foreach (var p in content)
                                value += p.Value;
                            break;
                        }
                    case 1:
                        {
                            value = 1;
                            foreach (var p in content)
                                value *= p.Value;
                            break;
                        }
                    case 2:
                        {
                            value = content.Min(p => p.Value);
                            break;
                        }
                    case 3:
                        {
                            value = content.Max(p => p.Value);
                            break;
                        }
                    case 4:
                        {
                            value = _Value;
                            break;
                        }
                    case 5:
                        {
                            value = content[0].Value > content[1].Value ? 1 : 0;
                            break;
                        }
                    case 6:
                        {
                            value = content[0].Value < content[1].Value ? 1 : 0;
                            break;
                        }
                    case 7:
                        {
                            value = content[0].Value == content[1].Value ? 1 : 0;
                            break;
                        }
                }
                return value;
            }
            private set
            {
                _Value = value;
            }
        }

        public int Type { get; private set; }
        public int Version { get; private set; }

        public int getSummaryVersion()
        {
            int result = Version;
            foreach (var p in content)
                result += p.getSummaryVersion();
            return result;
        }

        public Packet(string data, ref int offset)
        {
            var start = offset;
            Version = getInt(data, 3, ref offset);
            Type = getInt(data, 3, ref offset);

            if (Type == 4)
            {
                bool notEnd;
                do
                {
                    notEnd = getBool(data, ref offset);
                    Value = (Value << 4) + getInt(data, 4, ref offset);
                } while (notEnd);
            }
            else
            {
                if (getBool(data, ref offset))
                {
                    var count = getInt(data, 11, ref offset);
                    for (var i = 0; i < count; ++i)
                        content.Add(new Packet(data, ref offset));
                }
                else
                {
                    var length = getInt(data, 15, ref offset);
                    var end = offset + length;
                    while (offset < end)
                        content.Add(new Packet(data, ref offset));
                }
            }
        }

        int getInt(string data, int size, ref int offset)
        {
            var result = Convert.ToInt32(data.Substring(offset, size), 2);
            offset += size;
            return result;
        }

        bool getBool(string data, ref int offset)
        {
            offset += 1;
            return data[offset - 1] == '1';
        }
    }
    class Day16 : DayPattern<Packet>
    {
        public override void Parse(string path)
        {
            var s = System.IO.File.ReadAllText(path);
            var sb = new StringBuilder();
            foreach (var c in s)
                sb.Append(getString(c));
            var dataString = sb.ToString();
            var offset = 0;
            data = new Packet(dataString, ref offset);
        }

        string getString(char c)
        {
            var result = string.Empty;
            if (char.IsDigit(c))
                result = Convert.ToString(c - 0x30, 2);
            else
                result = Convert.ToString(c - 0x41 + 10, 2);
            return result.PadLeft(4, '0');
        }

        public override string PartOne()
        {
            return data.getSummaryVersion().ToString();
        }

        public override string PartTwo()
        {
            return data.Value.ToString();
        }
    }
}