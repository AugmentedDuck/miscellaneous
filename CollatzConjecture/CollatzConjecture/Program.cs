using System.Diagnostics;

namespace CollatzConjecture
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What number do you want to test?");

            ulong testNumber;

            while (true)
            {
                try
                {
                    string input = Console.ReadLine()!;

                    testNumber = Convert.ToUInt64(input);

                    break;
                }
                catch (Exception error)
                {
                    Console.WriteLine($"Something went wrong, try again\n{error}");
                }
            }

            Stopwatch stopwatch = new();
            
            stopwatch.Start();
            CollatzRecursive(testNumber);
            stopwatch.Stop();

            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");

            stopwatch.Reset();

            stopwatch.Start();
            CollatzIterative(testNumber);
            stopwatch.Stop();

            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        }

        private static void CollatzRecursive(ulong testNumber)
        {
            Console.WriteLine(testNumber.ToString());

            if (testNumber == 1)
            {
                return;
            } 
            else if (testNumber % 2 == 0)
            {
                testNumber /= 2;
                CollatzRecursive(testNumber);
            }
            else if (testNumber % 2 == 1)
            {
                testNumber *= 3;
                testNumber++;
                CollatzRecursive(testNumber);
            }
        }

        private static void CollatzIterative(ulong testNumber)
        {
            while (testNumber != 1)
            {
                Console.WriteLine(testNumber.ToString());

                if ( testNumber % 2 == 0 )
                {
                    testNumber /= 2;
                }
                else
                {
                    testNumber *= 3;
                    testNumber++;
                }
            }
        }
    }
}
