using System.Diagnostics;
using System.Drawing;

namespace SortingAlgorithms
{
    internal class Program
    {
        readonly Random rng = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Please Select Algorithm to Test:");
            //Console.WriteLine("(1) Heapsort");
            //Console.WriteLine("(2) Introsort");
            //Console.WriteLine("(3) Merge sort");
            //Console.WriteLine("(4) In-Place Merge sort");
            //Console.WriteLine("(5) Tournament sort");
            //Console.WriteLine("(6) Tree sort");
            //Console.WriteLine("(7) Block sort");
            //Console.WriteLine("(8) Smoothsort");
            //Console.WriteLine("(9) Timsort");
            //Console.WriteLine("(10) Patience sort");
            //Console.WriteLine("(11) Cubesort");
            //Console.WriteLine("(12) Quicksort");
            //Console.WriteLine("(13) Fluxsort");
            //Console.WriteLine("(14) Crumsort");
            //Console.WriteLine("(15) Library sort");
            //Console.WriteLine("(16) Shellsort");
            //Console.WriteLine("(17) Comb sort");
            Console.WriteLine("(18) Insertion sort [V]");
            Console.WriteLine("(19) Bubble sort [V]");
            //Console.WriteLine("(20) Cocktail Shaker sort");
            //Console.WriteLine("(21) Gnome sort");
            //Console.WriteLine("(22) Odd-even sort");
            //Console.WriteLine("(23) Strand sort");
            //Console.WriteLine("(24) Selection sort");
            //Console.WriteLine("(25) Cycle sort");
            //Console.WriteLine("(26) Pigeonhole sort");
            //Console.WriteLine("(27) Bucket sort");
            //Console.WriteLine("(28) Counting sort");
            //Console.WriteLine("(29) Radix sort");
            //Console.WriteLine("(30) Spreadsort");
            //Console.WriteLine("(31) Burstsort");
            //Console.WriteLine("(32) Flashsort");
            Console.WriteLine("! (33) Bogosort ! [V]");
            //Console.WriteLine("! (34) Bogo-Bogosort !");
            //Console.WriteLine("(35) Stooge sort");
            //Console.WriteLine("(36) Slowsort");
            Console.WriteLine("(37) Thanos sort - Removes Data");
            Console.WriteLine("(38) Stalin sort - Removes Data [V]");
            Console.WriteLine("! (39) Miracle sort ! [N/A]");
            Console.WriteLine("! (40) Bozosort ! [V]");
            Console.WriteLine("(41) Quantum sort - Requires the correct universe [N/A]");
            Console.WriteLine("(42) Dictator sort [N/A]");
            //Console.WriteLine("(43) Pancake sort");
            //Console.WriteLine("(44) Bitonic sort");

            string input = Console.ReadLine()!;

            while (int.Parse(input) < 1 || int.Parse(input) > 44)
            {
                Console.WriteLine("Invalid input. Please select a number between 1 and 44.");
                input = Console.ReadLine()!;
            }

            ISortAlgorithm algorithm = ChooseAlgorithm(input);

            Console.WriteLine(TestCorrectness(algorithm));



            /* Disabled for testing correctness of algorithms
            Console.WriteLine($"{algorithm.Name()}\nSize of Array;Time (ms);Large Span numbers\n");

            int arraySize = 16;

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(TestAlgorithm(algorithm, arraySize));
                arraySize *= 2;
            }
            */
        }

        private static string TestCorrectness(ISortAlgorithm algorithm)
        {
            int[] array = new int[16];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Random().Next(int.MinValue, int.MaxValue);
            }

            int[] expected = (int[])array.Clone();
            Array.Sort(expected);

            int[] actual = (int[])array.Clone();
            algorithm.Sort(actual);
            
            if (actual.Length == expected.Length)
            {
                for (int i = 0; i < actual.Length; i++)
                {
                    if (actual[i] != expected[i])
                    {
                        return "Algorithm is incorrect, length IS equal but data IS NOT";
                    }
                }
                return "Algorithm is Correct";
            }
            else
            {
                expected = (int[])actual.Clone();
                Array.Sort(expected);
                for (int i = 0; i < actual.Length; i++)
                {
                    if (actual[i] != expected[i])
                    {
                        return "Algorithm is incorrect, length AND data ARE NOT equal";
                    }
                }
                return "Algorithm is incorrect, length IS NOT equal but data IS";
            }
        }

        private static string TestAlgorithm(ISortAlgorithm algorithm, int size)
        {
            int[] array = new int[size];

            int rounds = 33_554_432 / size;

            string result = "";

            for (int k = 0; k < rounds / 2; k++)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new Random().Next(int.MinValue, int.MaxValue);
                }

                Stopwatch stopwatch = Stopwatch.StartNew();

                algorithm.Sort(array);

                Console.WriteLine("");

                result += $"{size};{stopwatch.ElapsedMilliseconds};True\n";

                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    break;
                }
            }

            for (int k = 0; k < rounds / 2; k++)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new Random().Next(size);
                }

                Stopwatch stopwatch = Stopwatch.StartNew();

                algorithm.Sort(array);

                result += $"{size};{stopwatch.ElapsedMilliseconds};False\n";

                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    break;
                }
            }

            return result;
        }

        private static ISortAlgorithm ChooseAlgorithm(string choice)
        {
            ISortAlgorithm[] algorithms = new ISortAlgorithm[44];

            algorithms[18 - 1] = new InsertionSort();
            algorithms[19 - 1] = new BubbleSort();
            algorithms[33 - 1] = new Bogosort();
            algorithms[37 - 1] = new ThanosSort();
            algorithms[38 - 1] = new StalinSort();
            algorithms[39 - 1] = new MiracleSort();
            algorithms[40 - 1] = new Bozosort();
            algorithms[41 - 1] = new QuantumSort();
            algorithms[42 - 1] = new DictatorSort();

            return algorithms[int.Parse(choice) - 1];
        }
    }
}
