using System;
using CSC482_Lab0x02_Sorting.Sorts;

namespace CSC482_Lab0x02_Sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            TestData test = new TestData(1, 10);

            Console.WriteLine(test.IsSorted());

            iSorter<Key> selectionSorter = new Selection();

            selectionSorter.Sort(test.Keys);

            Console.WriteLine(test.IsSorted());

            foreach (var testKey in test.Keys)
            {
                Console.WriteLine(testKey.ToString());
            }

        }
    }
}
