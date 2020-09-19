using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    class Key : IComparable<Key>
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        private readonly Random _rand = new Random();

        private readonly byte[] _key;
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
            _rand.NextBytes(_key);
        }

        public void FillKeyRandomBytes(char min, char max)
        {
            // used to fill key with specified range of characters.
            for (int i = 0; i < KeyWidth; i++)
            {
                _key[i] = (byte)_rand.Next(min, max);
            }
        }

        public override string ToString()
        {
            // This will return the bytes as text based on default encoding.
            return System.Text.Encoding.Default.GetString(_key); 
        }

        public string ToStringLiteral()
        {
            var sb = new StringBuilder();
            foreach (var b in _key)
            {
                sb.Append(b + ", ");
            }

            return sb.ToString();
        }

        public byte[] ToBytes()
        {
            return _key;
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
            return a?.CompareTo(b) == 0;
        }
        public static bool operator !=(Key a, Key b)
        {
            return a?.CompareTo(b) != 0;
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
