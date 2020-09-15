using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
    class Key : IComparable<Key>
    {
        private byte[] _key;
        public int KeyWidth { get; }

        public Key(int width)
        {
            _key = new byte[width];
            KeyWidth = width;
        }
        public int CompareTo(Key other)
        {
            // If this key is longer than other key, and other key is valid (doesn't start with 0)
            // then this is the larger value, return positive 1.
            if (KeyWidth > other.KeyWidth) return 1;
            // Else if this is shorter, it is less, so return -1
            if (KeyWidth < other.KeyWidth) return -1;

            // Keys are the same length, check each byte for first mismatch. If this._keys[i] > other._keys[i] return 1
            // If equal, continue, if less, return -1
            for (int i = 0; i < _key.Length; i++)
            {
                if (_key[i] > other._key[i]) return 1;
                if (_key[i] < other._key[i]) return -1;
            }

            // keys are equal.
            return 0;
        }

        public static bool operator <(Key a, Key b)
        {
            return a.CompareTo(b) == -1;
        }
        public static bool operator >(Key a, Key b)
        {
            return a.CompareTo(b) == 1;
        }
        public static bool operator ==(Key a, Key b)
        {
            return a.CompareTo(b) == 0;
        }
        public static bool operator !=(Key a, Key b)
        {
            return a.CompareTo(b) != 0;
        }
    }
}
