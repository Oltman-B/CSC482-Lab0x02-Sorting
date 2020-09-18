using System;
using System.Collections.Generic;
using CSC482_Lab0x02_Sorting.Sorts;

namespace CSC482_Lab0x02_Sorting
{
    class Program
    {
        static void Main(string[] args)
        {

            var sorters = new List<iSorter<Key>>{new Quick(), new Merge(), new Selection()};

            foreach (var sorter in sorters)
            {
                Console.WriteLine($"Testing sorter {sorter.GetType()}");
                TestData test = new TestData(3, 10);
                Console.WriteLine("Sorted? " + test.IsSorted());
                foreach (var testKey in test.Keys)
                {
                    Console.WriteLine(testKey.ToString());
                }
                Console.WriteLine("Now Sorting...");
                sorter.Sort(test.Keys);
                foreach (var testKey in test.Keys)
                {
                    Console.WriteLine(testKey.ToString());
                }
                Console.WriteLine("Sorted?" + test.IsSorted());
                Console.WriteLine();
            }
        }
    }
}
