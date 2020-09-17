using System;

namespace CSC482_Lab0x02_Sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            TestData test = new TestData(7, 33);

            foreach (var testKey in test.Keys)
            {
                Console.WriteLine(testKey.ToString());
            }

        }
    }
}
