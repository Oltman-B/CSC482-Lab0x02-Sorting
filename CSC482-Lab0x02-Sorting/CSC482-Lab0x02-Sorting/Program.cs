using System;

namespace CSC482_Lab0x02_Sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            TestData test = new TestData(2, 1000);

            foreach (var testKey in test.Keys)
            {
                Console.WriteLine(testKey.ToString());
            }

            test.Keys.Sort();

            foreach (var testKey in test.Keys)
            {
                Console.WriteLine(testKey.ToString());
            }

        }
    }
}
