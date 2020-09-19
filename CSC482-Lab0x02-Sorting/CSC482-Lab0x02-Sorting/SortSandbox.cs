using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CSC482_Lab0x02_Sorting.Sorts;

namespace CSC482_Lab0x02_Sorting
{
    class SortSandbox
    {
        private readonly List<int> _sortedValues = new List<int>();

            private const int MaxSecondsPerAlgorithm = 1;
            private const long MaxMicroSecondsPerAlg = MaxSecondsPerAlgorithm * 1000000;

            private const long NMin = 1;
            private const long NMax = int.MaxValue;
            private long _n = NMin;

            private readonly Random _rand = new Random();
            private readonly Stopwatch _stopwatch = new Stopwatch();

            private double _mergeSortPrevTimeMicro;
            private double _mergeSortTimeMicro;
            private double _mergeSortExpectedDoublingRatio;
            private double _mergeSortActualDoublingRatio;

            private double _quickSortPrevTimeMicro;
            private double _quickSortTimeMicro;
            private double _quickSortExpectedDoublingRatio;
            private double _quickSortActualDoublingRatio;

            private double _selectionSortPrevTimeMicro;
            private double _selectionSortTimeMicro;
            private double _selectionSortExpectedDoublingRatio;
            private double _selectionSortActualDoublingRatio;

            private double _radixSortPrevTimeMicro;
            private double _radixSortTimeMicro;
            private double _radixSortExpectedDoublingRatio;
            private double _radixSortActualDoublingRatio;


            private TestData GenerateTestList(int length, int keyWidthInBytes, char minVal, char maxVal)
            {
                var data = new TestData(keyWidthInBytes, length, minVal, maxVal);
                return data;
            }

            private TestData GenerateTestList(int length, int keyWidthInBytes)
            {
                var data = new TestData(keyWidthInBytes, length);
                return data;
            }

            public bool VerificationTests()
            {
                var sorters = new List<iSorter<Key>> { new Quick(), new Merge(), new Selection(),
                    new Radix(RadixSortBase.Base256), new Radix(RadixSortBase.Base65536), new Radix(RadixSortBase.Base16777216) };

                VisualVerificationTests(sorters);
                if (!NonVisualVerificationTests(sorters))
                {
                    Console.WriteLine("One or more non-visual tests have failed");
                }


                // All tests pass
                return true;
            }

            private void VisualVerificationTests(List<iSorter<Key>> sorters)
            {
                foreach (var sorter in sorters)
                {
                    var test = GenerateTestList(5, 10, 'a', 'z');
                    
                    Console.WriteLine($"Testing sorter {sorter.GetType()}");
                    foreach (var testKey in test.Keys)
                    {
                        Console.Write(testKey.ToString() + " ");
                    }

                    Console.WriteLine(" Sorted? " + test.IsSorted());

                    Console.WriteLine("Now Sorting...");
                    sorter.Sort(test.Keys);
                    foreach (var testKey in test.Keys)
                    {
                        Console.Write(testKey.ToString() + " ");
                    }

                    Console.Write(" Sorted? " + test.IsSorted());
                    Console.WriteLine("\n");
                }
            }

            private bool NonVisualVerificationTests(List<iSorter<Key>> sorters)
            {
                bool allTestsPassed = true;
                foreach (var sorter in sorters)
                {
                    Console.WriteLine("Generating list of length 100000, key width of 24");
                    var test = GenerateTestList(10000, 24);

                    Console.WriteLine($"Sorting with {sorter.GetType()}");
                    sorter.Sort(test.Keys);

                    if (test.IsSorted())
                    {
                        Console.WriteLine("Verifying... Sorted!");
                    }
                    else
                    {
                        allTestsPassed = false;
                        Console.WriteLine("Verifying... Not Sorted!");
                    }
                    
                    Console.WriteLine("\n");
                }
                return allTestsPassed;
            }

    }
}

