using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting.Sorts
{
    // Decided to limit possible radix values to these enums.
    // anything over Base16777216 will overflow an integer and can't be used
    // to lookup in an array, so a different implementation would be required to expand
    // into the range of x64
    internal enum RadixSortBase
    {
        Base256 = 1,
        Base65536,
        Base16777216
    }

    internal class Radix : iSorter<Key>
    {
        private readonly int _segmentByteSize;
        private int _segmentCount;

        public Radix(RadixSortBase radix)
        {
            _segmentByteSize = (int) radix;
        }

        public void Sort(List<Key> keys)
        {
            // Build dictionary to map keys to their segments for easy/fast lookup.
            var keysToSegments = MapKeysToSegments(keys);

            // Need to perform inner algorithm once for each segment in the keys
            RadixSort(keys, keysToSegments);
        }

        private void RadixSort(List<Key> keys, Dictionary<Key, int[]> keyToSegments)
        {
            // copy keys to input
            var input = new List<Key>(keys);

            // calculate the maximum value needed for lookup into count array
            var radixMax = CalculateRadixMax();

            for (var curSegment = _segmentCount - 1; curSegment >= 0; curSegment--)
            {
                // Count number of occurrences of each segment ('digit')
                var counts = new int[radixMax];
                foreach (var key in keys)
                    // get current digit from dictionary and use it as an index
                    // into the count array
                    counts[keyToSegments[key][curSegment]]++;

                // Prefix Sum calculation used to calculate how many open 'spaces'
                // this current segment will require in the sub-sorted array.
                for (var i = 1; i < counts.Length; i++) counts[i] = counts[i] + counts[i - 1];

                // Use prefixSum to move element to correct position in output
                for (var i = input.Count - 1; i >= 0; i--)
                {
                    var index = keyToSegments[input[i]][curSegment];
                    keys[counts[index]-- - 1] = input[i];
                }

                // Copy input back to output (original list)
                for (var i = 0; i < input.Count; i++) input[i] = keys[i];
            }
        }

        private Dictionary<Key, int[]> MapKeysToSegments(List<Key> keys)
        {
            var keyToSegments = new Dictionary<Key, int[]>();
            foreach (var key in keys) keyToSegments.Add(key, SplitKeyIntoSegments(key));

            return keyToSegments;
        }

        private int[] SplitKeyIntoSegments(Key key)
        {
            // This function pre-computes the 'segments' based on whatever
            // radix passed to the sort. They can then be stored in a dictionary
            // for fast lookup
            var keyBytes = key.ToBytes();
            _segmentCount = key.KeyWidth / _segmentByteSize;
            var result = new int[_segmentCount];

            for (int i = 0, k = 0; i < key.KeyWidth - 1; i += _segmentByteSize)
            {
                // Build final value from each byte in keys
                var exponent = 8 * (_segmentByteSize - 1);
                var tempResult = 0;
                for (var j = 0; j < _segmentByteSize; j++)
                {
                    tempResult += keyBytes[i + j] * (int) Math.Pow(2, exponent);
                    exponent -= 8;
                }

                result[k++] = tempResult;
            }

            return result;
        }

        private int CalculateRadixMax()
        {
            // Use the enum's definition as number of bytes
            // to calculate the max value of this radix
            return (int) Math.Pow(2, 8 * _segmentByteSize);
        }
    }
}
