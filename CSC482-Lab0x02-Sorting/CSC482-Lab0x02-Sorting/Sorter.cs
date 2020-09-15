using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
    abstract class Sorter : IComparer<Key>
    {
        public int Compare(Key x, Key y)
        {
            if (x == null && y == null) return 0;
            else if (x == null) return -1;
            else if (y == null) return 1;
            else return x.CompareTo(y);
        }

        public abstract void Sort(List<Key> keys);
    }
}
