using AnyThings;

namespace AOC2024
{
    internal class Day09 : DayPattern<string>
    {
        public override void Parse(string singleText)
        {
            data = singleText;
        }

        public override string PartOne()
        {
            long result = 0;
            List<long> disk = Decode();
            List<long> defragment = Pack(disk);
            result = GetControlCode(defragment);
            return result.ToString();
        }

        private long GetControlCode(List<long> defragment)
        {
            long result = 0;
            for (int i = 0; i < defragment.Count && defragment[i] >= 0; ++i)
            {
                result += defragment[i] * i;
            }
            return result;
        }

        private List<long> Pack(List<long> disk)
        {
            int empty = 0;
            int currentBlock = disk.Count - 1;
            while (empty < currentBlock)
            {
                while (disk[currentBlock] < 0)
                    --currentBlock;
                if (empty < currentBlock)
                {
                    while (disk[empty] >= 0)
                        ++empty;
                    if (empty < currentBlock)
                    {
                        disk[empty] = disk[currentBlock];
                        disk[currentBlock] = -1;
                    }
                }
            }
            return disk;
        }

        private List<long> Decode()
        {
            List<long> result = new();
            for (var i = 0; i < data.Length; ++i)
            {
                if (i % 2 != 0)
                {
                    for (var c = 0; c < data[i] - 0x30; ++c)
                        result.Add(-1);
                }
                else
                {
                    for (var c = 0; c < data[i] - 0x30; ++c)
                        result.Add(i / 2);
                }
            }
            return result;
        }

        public override string PartTwo()
        {
            long result = 0;
            List<(long, long)> disk = Decode2();
            List<(long, long)> defragment = Pack2(disk);
            result = GetControlCode2(defragment);
            return result.ToString();
        }
        private List<(long, long)> Decode2()
        {
            List<(long, long)> result = new();
            for (var i = 0; i < data.Length; ++i)
            {
                if (i % 2 != 0)
                    result.Add((-1, data[i] - 0x30));
                else
                    result.Add((i / 2, data[i] - 0x30));
            }
            return result;
        }

        private List<(long, long)> Pack2(List<(long id, long size)> disk)
        {
            int currentBlock = disk.Count - 1;
            while (currentBlock >= 0)
            {
                while (currentBlock >= 0 && disk[currentBlock].id < 0)
                    --currentBlock;
                int empty = 0;
                if (currentBlock >= 0)
                {
                    while (empty < currentBlock && (disk[empty].id >= 0 || disk[empty].size < disk[currentBlock].size))
                        ++empty;
                    if (empty < currentBlock && disk[empty].size >= disk[currentBlock].size)
                    {
                        if (disk[empty].size == disk[currentBlock].size)
                        {
                            disk[empty] = disk[currentBlock];
                            disk[currentBlock] = (-1, disk[currentBlock].size);
                        }
                        else
                        {
                            var emptyParam = disk[empty];
                            disk[empty] = disk[currentBlock];
                            disk[currentBlock] = (-1, disk[currentBlock].size);
                            disk.Insert(empty + 1, (-1, emptyParam.size - disk[empty].size));
                            ++currentBlock;
                        }
                    }
                }
                --currentBlock;
            }
            return disk;
        }

        private long GetControlCode2(List<(long, long)> defragment)
        {
            long result = 0;
            long position = 0;
            for (int i = 0; i < defragment.Count; ++i)
            {
                if (defragment[i].Item1 >= 0)
                {
                    for (int s = 0; s < defragment[i].Item2; ++s)
                    {
                        result += defragment[i].Item1 * position;
                        ++position;
                    }
                }
                else
                {
                    position += defragment[i].Item2;
                }
            }
            return result;
        }

    }
}