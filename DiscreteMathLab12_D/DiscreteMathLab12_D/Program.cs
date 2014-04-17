using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiscreteMathLab12_D
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("multimap.in");
            StreamWriter sw = new StreamWriter("multimap.out");
            StringMultiMap set = new StringMultiMap();
            string s = sr.ReadLine();
            while (!string.IsNullOrEmpty(s))
            {
                string[] parts = s.Split(' ');
                string key = parts[1];
                if (parts[0] == "put")
                    set.Put(key, parts[2]);
                if (parts[0] == "delete")
                    set.Remove(key, parts[2]);
                if (parts[0] == "deleteall")
                    set.RemoveAll(key);
                if (parts[0] == "get")
                {
                    var result = set.GetAll(key);
                    sw.Write(result.Count() + " ");
                    foreach (string str in result)
                    {
                        sw.Write(str + " ");
                    }
                    sw.WriteLine();
                }
                s = sr.ReadLine();
            }
            sw.Close();
        }
    }

    class StringMultiMap
    {
        private const int TableSize = 1 << 18;

        public StringMultiMap()
        {
            _table = new List<KeyValuePair<string, string>>[TableSize];
            for (int i = 0; i < _table.Length; i++)
                _table[i] = new List<KeyValuePair<string, string>>();
            _deleted = new BitArray(TableSize, true);
        }

        private readonly List<KeyValuePair<string, string>>[] _table;
        private readonly BitArray _deleted;

        public void Put(string key, string value)
        {
            int x = h1(key);
            int y = _table[x].FindIndex(p => p.Key == key && p.Value == value);
            if (y == -1)
                _table[x].Add(new KeyValuePair<string, string>(key, value));
        }

        public IEnumerable<string> GetAll(string key)
        {
            int x = h1(key);
            return (from pair in _table[x] where pair.Key == key select pair.Value);
        }

        public void Remove(string key, string value)
        {
            int x = h1(key);
            int pos = _table[x].FindIndex(pair => pair.Key == key && pair.Value == value);
            if (pos != -1)
                _table[x].RemoveAt(pos);
        }

        public void RemoveAll(string key)
        {
            int x = h1(key);
            _table[x].RemoveAll(p => p.Key == key);
        }

        private int h1(string a)
        {
            int result = 0;
            int t = 37;
            int pow = 1;
            foreach (char c in a)
            {
                result += c * (int)Math.Pow(t, pow++);
            }
            return (Math.Sign(result) * result) % TableSize;
        }

    }
}