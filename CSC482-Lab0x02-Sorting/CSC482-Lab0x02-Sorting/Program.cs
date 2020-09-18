using System;
using CSC482_Lab0x02_Sorting.Sorts;

namespace CSC482_Lab0x02_Sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            TestData test = new TestData(10, 30);

            Console.WriteLine(test.IsSorted());

            //iSorter<Key> selectionSorter = new Selection();
            iSorter<Key> mergeSorter = new Merge();

            //selectionSorter.Sort(test.Keys);
            mergeSorter.Sort(test.Keys);

            foreach (var testKey in test.Keys)
            {
                Console.WriteLine(testKey.ToString());
            }

            Console.WriteLine(test.IsSorted());

        }
    }
}
