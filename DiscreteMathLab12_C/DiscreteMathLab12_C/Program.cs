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
            StreamReader sr = new StreamReader("linkedmap.in");
            StreamWriter sw = new StreamWriter("linkedmap.out");
            StringMap set = new StringMap();
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
                if (parts[0] == "prev")
                    sw.WriteLine(set.PrevValue(key) ?? "none");
                if (parts[0] == "next")
                    sw.WriteLine(set.NextValue(key) ?? "none");
                s = sr.ReadLine();
            }
            sw.Close();
        }
    }

    class StringMap
    {
        class Entry
        {
            public KeyValuePair<string, string> item;

            public Entry prev,
                         next;

            public Entry(string key, string value, Entry prev, Entry next)
            {
                item = new KeyValuePair<string, string>(key, value);
                this.prev = prev;
                this.next = next;
            }

            public string Key { get { return item.Key; } }
            public string Value { get { return item.Value; } set { item = new KeyValuePair<string, string>(item.Key, value); } }
        }

        private const int TableSize = 1 << 18;

        public StringMap()
        {
            _table = new List<Entry>[TableSize];
            for (int i = 0; i < _table.Length; i++)
                _table[i] = new List<Entry>();
            _deleted = new BitArray(TableSize, true);
            _head = _tail = null;
        }

        private readonly List<Entry>[] _table;
        private readonly BitArray _deleted;
        private Entry _head, _tail;

        public void Put(string key, string value)
        {
            int x = h1(key);
            int y = _table[x].FindIndex(p => p.Key == key);
            if (y == -1)
            {
                Entry e = new Entry(key, value, _tail, null);
                _table[x].Add(e);
                if (_tail != null)
                    _tail.next = e;
                _tail = e;
            }
            else
                _table[x][y].Value = value;
        }

        public string Get(string item)
        {
            Entry e = getEntry(item);
            return e != null ? e.Value : null;
        }

        private Entry getEntry(string key)
        {
            int x = h1(key);
            int i = _table[x].FindIndex(p => p.Key == key);
            return i != -1 ? _table[x][i] : null;
        }

        public void Remove(string item)
        {
            int x = h1(item);
            int i = _table[x].FindIndex(p => p.Key == item);
            if (i != -1)
            {
                Entry e = _table[x][i];
                if (_tail == e) _tail = e.prev;
                if (e.prev != null) e.prev.next = e.next;
                if (e.next != null) e.next.prev = e.prev;
                
                _table[x].RemoveAt(i);
            }
        }

        public string NextValue(string key)
        {
            Entry e = getEntry(key);
            if (e != null && e.next != null) return e.next.Value;
            else
                return null;
        }

        public string PrevValue(string key)
        {
            Entry e = getEntry(key);
            if (e != null && e.prev != null) return e.prev.Value;
            else
                return null;
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