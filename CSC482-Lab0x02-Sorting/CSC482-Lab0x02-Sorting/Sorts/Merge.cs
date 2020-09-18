using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CSC482_Lab0x02_Sorting.Sorts
{
    class Merge : iSorter<Key>
    {
        public void Sort(List<Key> keys)
        {
            // copy source keys so that we can overwrite original list with sorted list and not have to return it.
            var copyOfKeys = new List<Key>(keys);
            DivideAndConquer(copyOfKeys, 0, keys.Count, keys);
        }

        // Some guidance from https://en.wikipedia.org/wiki/Merge_sort
        private void DivideAndConquer(List<Key> tempKeys, int start, int end, List<Key> actualKeys)
        {
            if (end - start <= 1) return;
            int splitPoint = (start + end) / 2;

            // 1. First partition the keyList into an upper and lower half.
            DivideAndConquer(actualKeys, start, splitPoint, tempKeys);
            DivideAndConquer(actualKeys, splitPoint, end, tempKeys);
            // 2. Then use temp copy of keys list to store merged result from resultKeys
            MergeKeys(tempKeys, start, splitPoint, end, actualKeys);
            // 3. This will alternate between the lists at every recursive call. That way
            // we only need to copy the original list one time.
        }

        private void MergeKeys(List<Key> sourceKeys, int start, int splitPoint, int end, List<Key> mergedKeys)
        {
            int leftP = start;
            int rightP = splitPoint;

            // Keeps track of next open index in the final mergedKeys list
            int i = start;

            // Continue looping while either left or right pointer haven't reached
            // the end of their partitions.
            while (leftP < splitPoint || rightP < end)
            {
                // Handle case where both pointers are within their partition
                if (leftP < splitPoint && rightP < end)
                {
                    // Add lowest val to actualKeys list
                    if (sourceKeys[leftP] <= sourceKeys[rightP])
                    {
                        mergedKeys[i] = sourceKeys[leftP++];
                    }
                    else
                    {
                        mergedKeys[i] = sourceKeys[rightP++];
                    }
                }
                // Handle case where only the left pointer is in its partition
                else if (leftP < splitPoint && rightP >= end)
                {
                    mergedKeys[i] = sourceKeys[leftP++];
                }
                // Finally, only right pointer is in its partition
                else
                {
                    mergedKeys[i] = sourceKeys[rightP++];
                }
                // Always increment the write pointer in final mergedKeys list
                i++;
            }
        }
    }
}
