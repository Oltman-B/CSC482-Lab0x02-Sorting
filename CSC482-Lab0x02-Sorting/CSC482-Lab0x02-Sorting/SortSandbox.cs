using CSC482_Lab0x02_Sorting.Sorts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        private const double MaxSecondsPerAlgorithm = 25;
        private const double MaxMicroSecondsPerAlg = MaxSecondsPerAlgorithm * 1000000;

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
            var sorters = new List<iSorter<Key>>
            {
                new Quick(), new Merge(), new Selection(),
                new Radix(RadixSortBase.Base256), new Radix(RadixSortBase.Base65536),
                new Radix(RadixSortBase.Base16777216)
            };

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
            var sorters = new List<iSorter<Key>>
            {
                new Quick(), new Merge(), new Selection(),
                new Radix(RadixSortBase.Base256), new Radix(RadixSortBase.Base65536),
                new Radix(RadixSortBase.Base16777216)
            };

            foreach (var sorter in sorters)
            {
                SortTestRuntime(sorter);
            }
        }

        private void SortTestRuntime(iSorter<Key> sorter)
        {
            PrintHeader(sorter);

            // This can probably be handled in a better way, but we need to keep track of
            // the sort stats for each K, so that we can calculate a row and save previous times.
            const int differentKCount = 4;
            var currentStats = new List<SortStats>();
            for (int i = 0; i < differentKCount; i++)
            {
                currentStats.Add(new SortStats());
            }

            for (var n = NMin; n * 2 < NMax; n *= 2)
            {
                // break if largest k value takes longer than allowed time. Check n > 2 to ignore first few
                // iterations of test, they tend to have a longer wind-up time.
                if (currentStats[^1].TimeMicro > MaxMicroSecondsPerAlg || n > 1048576)
                {
                    PrintSortTerminationMessage(sorter);
                    break;
                }

                PrintIndexColumn(n);
                for (int k = 6, kIndex = 0; k <= 48; kIndex++, k *= 2)
                {
                    var testData = GenerateTestList(n, k);
                    _stopwatch.Restart();
                    sorter.Sort(testData.Keys);
                    _stopwatch.Stop();

                    currentStats[kIndex].PrevTimeMicro = currentStats[kIndex].TimeMicro;
                    currentStats[kIndex].TimeMicro = TicksToMicroseconds(_stopwatch.ElapsedTicks);

                    CalculateDoublingRatios(n, k, currentStats[kIndex], sorter);

                    PrintData(n, currentStats[kIndex]);
                }

                // New Row
                Console.WriteLine();
            }
        }

        private void CalculateDoublingRatios(int n, int k, SortStats stats, iSorter<Key> sorter)
        {
            // Switch on sorter type so that appropriate doubling calculation method can be called.
            switch (sorter)
            {
                case Merge m:
                    CalculateMergeSortDoublingRatios(n, stats);
                    break;
                case Quick q:
                    CalculateQuickSortDoublingRatios(n, stats);
                    break;
                case Selection s:
                    CalculateSelectionSortDoublingRatios(n, stats);
                    break;
                case Radix r:
                    CalculateRadixSortDoublingRatios(n, k, stats);
                    break;
            }
        }

        private void CalculateMergeSortDoublingRatios(int n, SortStats stats)
        {
            if (n <= 2)
            {
                stats.ExpectedDoublingRatio = -1;
                stats.ActualDoublingRatio = -1;
                return;
            }

            stats.ExpectedDoublingRatio = (n * Math.Log2(n)) / (((double)n / 2) * (Math.Log2((double)n / 2)));
            stats.ActualDoublingRatio = stats.TimeMicro / stats.PrevTimeMicro;
        }

        private void CalculateQuickSortDoublingRatios(int n, SortStats stats)
        {
            if (n <= 2)
            {
                stats.ExpectedDoublingRatio = -1;
                stats.ActualDoublingRatio = -1;
                return;
            }

            stats.ExpectedDoublingRatio = (n * Math.Log2(n)) / (((double)n / 2) * (Math.Log2((double)n / 2)));
            stats.ActualDoublingRatio = stats.TimeMicro / stats.PrevTimeMicro;
        }

        private void CalculateRadixSortDoublingRatios(int n, int k, SortStats stats)
        {
            if (n <= 2)
            {
                stats.ExpectedDoublingRatio = -1;
                stats.ActualDoublingRatio = -1;
                return;
            }

            stats.ExpectedDoublingRatio = (n * k) / (((double)n / 2) * ((double)k));
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

            stats.ExpectedDoublingRatio = (n * n) / ((double)n / 2 * n / 2);
            stats.ActualDoublingRatio = stats.TimeMicro / stats.PrevTimeMicro;
        }

        private void PrintHeader(iSorter<Key> sorter)
        {
            Console.WriteLine($"Starting run-time tests for {sorter.GetType().Name} sort...\n");
            Console.WriteLine(
                " \t\t\t        k=6|    |  Doubling Ratios   |          k=12|    |  Doubling Ratios   |          k=24|    |   Doubling Ratios  |          k=48|      Doubling Ratios   |");
            Console.WriteLine(
                "N\t\t\t       Time|    | Actual  | Expected |          Time|    | Actual  | Expected |          Time|    | Actual  | Expected |          Time|    | Actual  | Expected|");
        }

        private void PrintSortTerminationMessage(iSorter<Key> sorter)
        {
            Console.WriteLine($"{sorter.GetType().Name} sort exceeded allotted time, terminating...\n");
        }

        private void PrintIndexColumn(int n)
        {
            Console.Write($"{n,-15}");
        }
        private void PrintData(int n, SortStats stats)
        {
            var actualDoubleFormatted = stats.ActualDoublingRatio < 0
                ? "na".PadLeft(12)
                : stats.ActualDoublingRatio.ToString("F2").PadLeft(12);
            var expectDoubleFormatted = stats.ExpectedDoublingRatio < 0
                ? "na".PadLeft(7)
                : stats.ExpectedDoublingRatio.ToString("F2").PadLeft(7);

            Console.Write(
                $"{stats.TimeMicro,20:F2} {actualDoubleFormatted} {expectDoubleFormatted}");
        }

        private static double TicksToMicroseconds(long ticks)
        {
            return (double)ticks / Stopwatch.Frequency * 1000000;
        }
    }
}

