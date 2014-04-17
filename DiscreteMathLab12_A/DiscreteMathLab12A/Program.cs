using System;
using System.Collections;
using System.IO;

namespace DiscreteMathLab12A
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("set.in");
            StreamWriter sw = new StreamWriter("set.out");
            IntSet set = new IntSet();
            string s = sr.ReadLine();
            while (s != null)
            {
                string[] parts = s.Split(' ');
                int arg = int.Parse(parts[1]);
                if (parts[0] == "insert")
                    set.Put(arg);
                if (parts[0] == "delete")
                    set.Remove(arg);
                if (parts[0] == "exists")
                    sw.WriteLine(set.Contains(arg) ? "true" : "false");
                s = sr.ReadLine();
            }
            sw.Close();
        }
    }

    class IntSet
    {
        private const int TableSize = 1 << 23;

        public IntSet()
        {
            _table = new int?[TableSize];
            _deleted = new BitArray(TableSize, true);
        }

        private readonly int?[] _table;
        private readonly BitArray _deleted;

        public void Put(int item)
        {
            if (Contains(item)) return;
            int x = H1(item),
                y = H2(item);
            for (int i = 0; i < TableSize; i++)
            {
                if (_table[x] == null || _deleted[x])
                {
                    _table[x] = item;
                    _deleted[x] = false;
                    return;
                }
                x = (x + y) % TableSize;
            }
        }

        public bool Contains(int item)
        {
            int x = H1(item),
                y = H2(item);
            for (int i = 0; i < TableSize; i++)
            {
                if (_table[x] != null)
                {
                    if (_table[x] == item && !_deleted[x])
                        return true;
                }
                else
                    return false;
                x = (x + y) % TableSize;
            }
            return false;
        }

        public void Remove(int item)
        {
            int x = H1(item),
                y = H2(item);
            for (int i = 0; i < TableSize; i++)
            {
                if (_table[x] != null)
                {
                    if (_table[x] == item)
                    {
                        _deleted[x] = true;
                        return;
                    }
                }
                else 
                    return;
                x = (x + y) % TableSize;
            }
        }

        private static int H1(int a)
        {
            return (Math.Sign(a)*a) % 1000007;
        }

        private static int H2(int a)
        {
            return 2*((Math.Sign(a) * a) % 312057) + 1;
        }
    }
}