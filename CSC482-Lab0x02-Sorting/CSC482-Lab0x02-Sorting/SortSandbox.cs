using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CSC482_Lab0x02_Sorting.Sorts;

namespace CSC482_Lab0x02_Sorting
{
    class SortStats
    {
        public double PrevTimeMicro;
        public double TimeMicro;
        public double ExpectedDoublingRatio;
        public double ActualDoublingRatio;

        public SortStats()
        {
            PrevTimeMicro = 0;
            TimeMicro = 0;
            ExpectedDoublingRatio = 0;
            ActualDoublingRatio = 0;
        }
    }
    class SortSandbox
    {
        private readonly List<int> _sortedValues = new List<int>();

            private const int MaxSecondsPerAlgorithm = 1;
            private const long MaxMicroSecondsPerAlg = MaxSecondsPerAlgorithm * 1000000;

            private const int NMin = 1;
            private const int NMax = int.MaxValue;

            private readonly Random _rand = new Random();
            private readonly Stopwatch _stopwatch = new Stopwatch();


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


            public void RunTimeTests()
            {
                PrintHeader();

                var currentStats = new SortStats();

                for (var n = NMin; n * 2 <= NMax; n *= 2)
                    if (currentStats.PrevTimeMicro < MaxMicroSecondsPerAlg)
                    {
                        PrintIndexColumn(n);
                        for (var k = 6; k <= 48; k *= 2)
                        {
                            var testData = GenerateTestList(n, k);
                            _stopwatch.Restart();
                            SelectionSortRunTimeTests(testData.Keys);
                            _stopwatch.Stop();

                            currentStats.PrevTimeMicro = currentStats.TimeMicro;
                            currentStats.TimeMicro = TicksToMicroseconds(_stopwatch.ElapsedTicks);

                            CalculateSelectionSortDoublingRatios(n, currentStats);

                            PrintData(n, currentStats);

                            //TODO Problem here, the measuring of actual dobuling ratio is wrong for all k after 6
                        }
                        // New Row
                        Console.WriteLine();
                    }
            }

            private void MergeSortRunTimeTests(List<Key> keys)
        {
            iSorter<Key> merge = new Merge();
            SortRunTimeTests(merge, keys);
        }

        private void QuickSortRunTimeTests(List<Key> keys)
        {
            iSorter<Key> quick = new Quick();
            SortRunTimeTests(quick, keys);
        }

        private void SelectionSortRunTimeTests(List<Key> keys)
        {
            iSorter<Key> selection = new Selection();
            SortRunTimeTests(selection, keys);
        }

        private void RadixSortRunTimeTests(List<Key> keys, RadixSortBase d)
        {
            iSorter<Key> radix = new Radix(d);
            SortRunTimeTests(radix, keys);
        }

        private void SortRunTimeTests(iSorter<Key> sorter, List<Key> keys)
        {
            sorter.Sort(keys);
        }

        private void CalculateMergeSortDoublingRatios(int n, SortStats stats)
        {
            if (n <= 2)
            {
                stats.ExpectedDoublingRatio = -1;
                stats.ActualDoublingRatio = -1;
                return;
            }

            stats.ExpectedDoublingRatio = (n*Math.Log2(n)) / (((double)n/2)*(Math.Log2((double)n/2)));
            stats.ActualDoublingRatio = stats.TimeMicro / stats.PrevTimeMicro;
        }

        private void CalculateSelectionSortDoublingRatios(int n, SortStats stats)
        {
            if (n <= 2)
            {
                stats.ExpectedDoublingRatio = -1;
                stats.ActualDoublingRatio = -1;
                return;
            }

            stats.ExpectedDoublingRatio = (n*n) / ((double)n/2 * n/2);
            stats.ActualDoublingRatio = stats.TimeMicro / stats.PrevTimeMicro;
        }

        private void PrintHeader()
        {
            Console.WriteLine(
                " \t\t|k=6 ||  Doubling Ratios   ||k=12    Doubling Ratios   ||k=24     Doubling Ratios  ||k=48    Doubling Ratios   |");
            Console.WriteLine(
                "N\t\t|Time|| Actual || Expected ||Time|| Actual || Expected ||Time|| Actual || Expected ||Time|| Actual || Expected |");
        }

        private void PrintIndexColumn(int n)
        {
            Console.Write($"{n,-15}");
        }
        private void PrintData(int n, SortStats stats)
        {
            var actualDoubleFormatted = stats.ActualDoublingRatio < 0
                ? "na".PadLeft(14)
                : stats.ActualDoublingRatio.ToString("F2").PadLeft(14);
            var expectDoubleFormatted = stats.ExpectedDoublingRatio < 0
                ? "na".PadLeft(17)
                : stats.ExpectedDoublingRatio.ToString("F2").PadLeft(17);

            Console.Write(
                $"{stats.TimeMicro,15:F2} {actualDoubleFormatted} {expectDoubleFormatted}");
        }

        private static double TicksToMicroseconds(long ticks)
        {
            return (double)ticks / Stopwatch.Frequency * 1000000;
        }
    }
}

