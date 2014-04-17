using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DiscreteMathLab12_B
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("map.in");
            StreamWriter sw = new StreamWriter("map.out");
            StringMap set = new asd();
            string s = sr.ReadLine();
            while (!string.IsNullOrEmpty(s))
            {
                string[] parts = s.Split(' ');
                string key = parts[1];
                if (parts[0] == "put")
                    set.Put(key, parts[2]);
                if (parts[0] == "delete")
                    set.Remove(key);
                if (parts[0] == "get")
                    sw.WriteLine(set.Get(key) ?? "none");
                s = sr.ReadLine();
            }
            sw.Close();
        }
    }

    class StringMap
    {
        private const int TableSize = 1 << 18;

        public StringMap()
        {
            _table = new List<KeyValuePair<string, string>>[TableSize];
            for (int i = 0; i<_table.Length; i++)
                _table[i] = new List<KeyValuePair<string, string>>();
            _deleted = new BitArray(TableSize, true);
        }

        private readonly List<KeyValuePair<string, string>>[] _table;
        private readonly BitArray _deleted;

        public void Put(string key, string value)
        {
            int x = h1(key);
            int y = _table[x].FindIndex(p => p.Key == key);
            if (y == -1)
                _table[x].Add(new KeyValuePair<string, string>(key, value));
            else
                _table[x][y] = new KeyValuePair<string, string>(key, value);
        }

        public string Get(string item)
        {
            int x = h1(item);
            int i = _table[x].FindIndex(p => p.Key == item);
            return i != -1 ? _table[x][i].Value : null;
        }

        public void Remove(string item)
        {
            int x = h1(item);
            int i = _table[x].FindIndex(p => p.Key == item);
            if (i != -1)
            _table[x].RemoveAt(i);
        }

	private const int b = 37;

        private int h1(string a)
        {
            int result = 0;
            int t = b;
            foreach (char c in a)
            {
                result += c * t;
		t *= b;
            }
            return (Math.Sign(result) * result) % TableSize;
        }

    }
}
