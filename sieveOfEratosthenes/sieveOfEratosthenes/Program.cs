
using System.Diagnostics;

namespace sieveOfEratosthenes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int CheckToNumber = ReadConsoleToInt("To what number should be tested?");

            int[] numberLine = new int[CheckToNumber + 1];

            for (int i = 0; i < numberLine.Length; i++)
            {
                numberLine[i] = i;
            }


            Stopwatch sw = new Stopwatch();
            bool isPrime;

            //Console.WriteLine("Running Prime Check");
            //
            //sw.Start();
            //isPrime = SlowSieveOfEratosthenes(CheckToNumber);
            //sw.Stop();
            //
            //Console.WriteLine($"{sw.Elapsed} : {isPrime}");

            Console.WriteLine("Running Prime Check");

            sw.Restart();
            isPrime = FastSieveOfEratosthenes(CheckToNumber);
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} : {isPrime}");

            Console.WriteLine("Running Find All Primes");

            sw.Restart();
            int[] primesFast = FastSieveOfEratosthenes(numberLine);
            sw.Stop();

            Console.WriteLine(sw.Elapsed.ToString());

            //foreach (int prime in primesFast)
            //{
            //    Console.WriteLine(prime);
            //}

            //Console.WriteLine("Running Find All Primes");
            //
            //sw.Restart();
            //int[] primesSlow = SlowSieveOfEratosthenes(numberLine);
            //sw.Stop();
            //
            //Console.WriteLine(sw.Elapsed.ToString());
            //
            //foreach (int prime in primesSlow)
            //{
            //    Console.WriteLine(prime);
            //}
        }

        private static int[] SlowSieveOfEratosthenes(int[] numberLine)
        {
            foreach (int currentNumber in numberLine)
            {

                if (currentNumber == 0 || currentNumber == 1) { continue; }

                //Console.WriteLine($"{currentNumber}");
             
                for(int i = 0; i < numberLine.Length; i++)
                {
                    if (numberLine[i] == currentNumber || numberLine[i] == 0) { continue; }

                    if (numberLine[i] % currentNumber == 0)
                    {
                        numberLine[i] = 0;
                    }
                }
            }

            numberLine = numberLine.Where(x => x != 0).ToArray();

            return numberLine;
        }

        static bool SlowSieveOfEratosthenes(int CheckNumber)
        {
            bool[] primes = new bool[CheckNumber + 1];

            for (int i = 0; i < primes.Length; i++)
            {
                primes[i] = true;
            }

            primes[0] = false;
            primes[1] = false;

            for (int i = 2; i < primes.Length; i++)
            {
                if (!primes[i]) { continue; }

                for (int j = 2 * i; j < primes.Length; j += i)
                {
                    primes[j] = false;
                }
            }

            return primes[CheckNumber];
        }

        static int[] FastSieveOfEratosthenes(int[] numberline)
        {
            for (int i = 0; i * i < numberline.Length; i++)
            {
                if (numberline[i] == 0 || numberline[i] == 1) { continue; }

                for (int j = 2 * i; j < numberline.Length; j += i)
                {
                    numberline[j] = 0;
                }
            }

            numberline = numberline.Where(x => x != 0).ToArray();

            return numberline;
        }

        static bool FastSieveOfEratosthenes(int CheckNumber)
        {
            bool[] primes = new bool[CheckNumber + 1];

            for (int i = 0; i < primes.Length; i++)
            {
                primes[i] = true;
            }

            primes[0] = false;
            primes[1] = false;

            for (int i = 2; i * i < primes.Length; i++)
            {
                if (!primes[i]) { continue; }
                if (!primes[CheckNumber]) { return false; }

                for (int j = 2 * i; j < primes.Length; j += i)
                {
                    primes[j] = false;
                }
            }

            return primes[CheckNumber];
        }

        private static int ReadConsoleToInt(string question)
        {
            Console.WriteLine(question);

            while (true)
            {
                try
                {
                    string input = Console.ReadLine()!;
                    
                    int inputInt = int.Parse(input);

                    return inputInt;
                }
                catch (Exception)
                {
                    Console.WriteLine("Enter a number");    
                }
            }
        }
    }
}
