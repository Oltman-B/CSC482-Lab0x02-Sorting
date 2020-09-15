using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
    class TestData
    {
        private List<Key> _keys;

        public int KeyByteWidth { get; private set; }
        public int Count { get; private set; }
        public TestData(int keyByteWidth, int elementCount)
        {
            KeyByteWidth = keyByteWidth;
            Count = elementCount;
        }
    }
}
