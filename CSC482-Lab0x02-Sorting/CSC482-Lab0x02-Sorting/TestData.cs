﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
    class TestData
    {
        public List<Key> Keys { get; } = new List<Key>();

        public int KeyByteWidth { get; }
        public int Count { get; }

        public TestData(int keyByteWidth, int elementCount)
        {
            KeyByteWidth = keyByteWidth;
            Count = elementCount;
            FillKeyList();
        }

        public bool IsSorted()
        {
            // If at any point a preceding key is greater than a following key
            // the list is NOT sorted (only care about ascending order now.)
            for (int i = 0; i < Keys.Count - 1; i++)
            {
                if (Keys[i] > Keys[i + 1]) return false;
            }

            return true;
        }

        private void FillKeyList()
        {
            for (int i = 0; i < Count; i++)
            {
                Key newKey = new Key(KeyByteWidth);
                newKey.FillKeyRandomBytes();
                Keys.Add(newKey);
            }
        }

        
    }

}
