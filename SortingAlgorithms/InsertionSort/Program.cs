using System.Diagnostics;
using System.Drawing;

namespace SortingAlgorithms
{
    internal class Program
    {
        readonly Random rng = new();

        static void Main(string[] args)
        {
            ISortAlgorithm algorithm = ChooseAlgorithm();

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

        private static ISortAlgorithm ChooseAlgorithm()
        {
            List<ISortAlgorithm> algorithms = [];

            /*
             * ALGORITHMS TO IMPLEMENT (V.erified, I.mplemented):
             * [V] Heapsort
             * [ ] Introsort
             * [ ] Merge sort
             * [ ] In-Place Merge sort
             * [ ] Tournament sort
             * [ ] Tree sort
             * [ ] Block sort
             * [ ] Smoothsort
             * [ ] Timsort
             * [V] Patience sort
             * [ ] Cubesort
             * [V] Quicksort - Random
             * [V] Quicksort - Median of 3
             * [ ] Fluxsort
             * [ ] Crumsort
             * [ ] Library sort
             * [ ] Shellsort - Geometric
             * [ ] Shellsort - Pratt
             * [ ] Shellsort
             * [ ] Comb sort
             * [V] Insertion sort
             * [V] Bubble sort
             * [ ] Cocktail Shaker sort
             * [ ] Gnome sort
             * [ ] Odd-even sort
             * [ ] Strand sort
             * [ ] Selection sort
             * [ ] Cycle sort
             * 
             * N-C Sorts
             * [ ] Pigeonhole sort
             * [ ] Bucket sort
             * [ ] Counting sort
             * [ ] Radix sort - LSD
             * [ ] Radix sort - MSD
             * [ ] Radix sort - MSD - In-Place
             * [ ] Spreadsort
             * [ ] Burstsort
             * [ ] Flashsort
             * 
             * OTHER ALGORITHMS
             * [V] Bogosort ! VERY SLOW
             * [V] Bogobogosort ! EXTREMELY SLOW
             * [V] Stooge sort
             * [V] Slowsort
             * [V] Thanos sort ! Removes Data
             * [V] Stalin sort ! Removes Data
             * [I] Miracle sort ! DONT TEST
             * [V] Bozosort ! VERY SLOW
             * [I] Quantum sort ! Requires the correct universe
             * [I] Dictator sort
             * [V] Pancake sort
             */

            algorithms.Add(new Heapsort());
            algorithms.Add(new PatienceSort());
            algorithms.Add(new Quicksort(0));
            algorithms.Add(new Quicksort(1));
            algorithms.Add(new Quicksort(2));
            algorithms.Add(new Quicksort(3));
            algorithms.Add(new InsertionSort());
            algorithms.Add(new BubbleSort());
            // NC


            // OTHER
            algorithms.Add(new PancakeSort());
            algorithms.Add(new StoogeSort());
            algorithms.Add(new Slowsort());
            algorithms.Add(new Bogosort());
            algorithms.Add(new Bozosort());
            algorithms.Add(new Bogobogosort());
            algorithms.Add(new ThanosSort());
            algorithms.Add(new StalinSort());
            algorithms.Add(new DictatorSort());
            algorithms.Add(new MiracleSort());
            algorithms.Add(new QuantumSort());

            Console.WriteLine("Please Select Algorithm to Test:");
            
            foreach (ISortAlgorithm algorithm in algorithms)
            {
                Console.WriteLine($"({algorithms.IndexOf(algorithm)}) {algorithm.Name()}");
            }

            string input = Console.ReadLine()!;

            while (int.Parse(input) < 0 || int.Parse(input) > algorithms.Count)
            {
                Console.WriteLine($"Invalid input. Please select a number between 0 and {algorithms.Count}");
                input = Console.ReadLine()!;
            }

            return algorithms[int.Parse(input)];
        }
    }
}
