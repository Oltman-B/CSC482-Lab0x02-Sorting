using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace CSC482_Lab0x02_Sorting.Sorts
{
    class Quick : iSorter<Key>
    {
        public void Sort(List<Key> keys)
        {
            QuickSort(keys, 0, keys.Count - 1);
        }

        private void QuickSort(List<Key> keys, int low, int high)
        {
            // if low == high, just return. base case
            if (low < high)
            {
                int partition = Partition(keys, low, high);

                // Sort everything left of partition
                QuickSort(keys, low, partition - 1);
                // Sort everything right of the partition
                QuickSort(keys, partition + 1, high);
            }
        }

        private int Pivot(List<Key> keys, int low, int high)
        {
            // Using "Median-of-three" algorithm to choose pivot from https://en.wikipedia.org/wiki/Quicksort
            int mid = low + (high - low) / 2; // prevents overflow over (high + low) / 2
            if(keys[mid] < keys[low])
                keys.Swap(low, mid);
            if(keys[high] < keys[low])
                keys.Swap(low, high);
            if(keys[mid] < keys[high])
                keys.Swap(mid, high);

            return high;
        }

        private int Partition(List<Key> keys, int low, int high)
        {
            // Choose a pivot point all elements less than this will be to the left
            // all elements greater than this will be moved to the right.
            int pivotIdx = Pivot(keys, low, high);

            // smallest will point to next open position for swap
            int smallest = low - 1;
            for (int i = low; i < high; i++)
            {
                // iterate through the segment from low to hih. With smallest pointer
                // set to left check to see if each element is smaller than pifot
                // if it is, it should be moved to the smaller portion of the list.
                // swap it with what is in the next open position and increment smallest
                if (keys[i] < keys[pivotIdx])
                {
                    smallest++;
                    keys.Swap(smallest, i);
                }
            }

            // Once we have swapped all elements, we still need to move the pivot to it's proper location.
            // after iterating the list, swap pivot into the position directly after last smallest item.
            // this forces some item larger than the pivot to pivot's previous position, making room for the
            // pivot in the 'middle' of the list.
            keys.Swap(smallest+1, pivotIdx);

            // return the new pivot index
            return smallest + 1;
        }
    }
}
