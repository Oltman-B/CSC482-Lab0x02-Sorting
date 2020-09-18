using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
    // ISorter can only be implemented by types that implement IComparable<T>
    interface iSorter<T> where T : IComparable<T>
    {
        public void Sort(List<T> keys);
    }
}
