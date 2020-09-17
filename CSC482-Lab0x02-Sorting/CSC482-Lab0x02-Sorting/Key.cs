using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
    class Key : IComparable<Key>
    {
        private Random rand = new Random();

        private byte[] _key;
        public int KeyWidth { get; }
        public Key(int width)
        {
            _key = new byte[width];
            KeyWidth = width;
        }
        public int CompareTo(Key other)
        {
            //Check prefix of keys (should alwasy be same width, but this will handle
            // keys of different widths.)
            for (int i = 0; i < _key.Length && i < other._key.Length; i++)
            {
                if (_key[i] > other._key[i]) return 1;
                if (_key[i] < other._key[i]) return -1;
            }

            // When we get here, we know that shorter key matches longer key's prefix
            // if this is longer key, it comes after, if this is shorter, it comes before
            // else they are equal keys.
            if (_key.Length > other._key.Length) return -1;
            else if (_key.Length < other._key.Length) return 1;
            else return 0; // Keys are equal
        }

        public void FillKeyRandomBytes()
        {
            //ToDo add extra byte for zero index when printing if necessary
            rand.NextBytes(_key);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var b in _key)
            {
                sb.Append(b + ", ");
            }

            return sb.ToString();
            //return System.Text.Encoding.Default.GetString(_key); // This will return the bytes as text based on default encoding.
        }

        #region Key Comparison Operator Overloads
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
        public static bool operator <=(Key a, Key b)
        {
            return a.CompareTo(b) <= 0;
        }
        public static bool operator >=(Key a, Key b)
        {
            return a.CompareTo(b) >= 0;
        }

        #endregion
    }
}
