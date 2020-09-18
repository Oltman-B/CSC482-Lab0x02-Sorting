using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CSC482_Lab0x02_Sorting.Sorts
{
    class Selection : iSorter<Key>
    {
        // t(N) ~ a*N^2  O(N^2)
        public void Sort(List<Key> keys)
        {
            // iterate through list
            for (int i = 0; i < keys.Count - 1; i++)
            { 
                // start from one index after i, set selection to index i
                // update selection if smaller value found.
                int selection = i;
                for (int j = i + 1; j < keys.Count; j++)
                {
                    // update selected index to point to the smallest element seen yet
                    if (keys[j] < keys[selection])
                    {
                        selection = j;
                    }
                }
                // Swap key at i with selection, i now equals smallest index after previous
                keys.Swap(i, selection);
            }
        }
    }
}
