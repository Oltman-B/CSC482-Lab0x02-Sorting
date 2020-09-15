using System;

namespace CSC482_Lab0x02_Sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            Key a = new Key(4);
            Key b = new Key(4);

            if (a < b)
            {
                Console.WriteLine($"a-width={a.KeyWidth} < b-width={b.KeyWidth}");
            }
            else if (a == b)
            {
                Console.WriteLine("Equal!");
            }

        }
    }
}
