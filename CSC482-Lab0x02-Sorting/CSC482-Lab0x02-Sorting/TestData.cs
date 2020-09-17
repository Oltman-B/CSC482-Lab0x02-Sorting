using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
    class TestData
    {
        public List<Key> Keys { get; } = new List<Key>();

        public int KeyByteWidth { get; private set; }
        public int Count { get; private set; }

        public TestData(int keyByteWidth, int elementCount)
        {
            KeyByteWidth = keyByteWidth;
            Count = elementCount;
            FillKeyList();
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
