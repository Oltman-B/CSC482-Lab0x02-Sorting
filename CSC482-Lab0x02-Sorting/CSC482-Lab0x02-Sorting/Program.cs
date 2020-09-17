using System;

namespace CSC482_Lab0x02_Sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            TestData test = new TestData(5, 10);

            Console.WriteLine(test.IsSorted());

            test.Keys.Sort();

            Console.WriteLine(test.IsSorted());
            foreach (var testKey in test.Keys)
            {
                Console.WriteLine(testKey.ToString());
            }

        }
    }
}
