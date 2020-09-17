using System;
using System.Collections.Generic;
using System.Text;

namespace CSC482_Lab0x02_Sorting
{
    public static class ExtensionMethods
    {
        public static void Swap<T>(this List<T> @this, int index1, int index2)
        {
            T temp = @this[index1];
            @this[index1] = @this[index2];
            @this[index2] = temp;
        }
    }
}
