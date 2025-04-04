
namespace sieveOfEratosthenes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int CheckToNumber = ReadConsoleToInt("To what number should be tested?");

            int[] numberLine = new int[CheckToNumber - 2];

            for (int i = 0; i < numberLine.Length; i++)
            {
                numberLine[i] = i + 2;
            }

            Console.WriteLine("Running");

            int[] primes = SieveOfEratosthenes(numberLine);

            foreach (int prime in primes)
            {
                Console.WriteLine(prime);
            }
        }

        private static int[] SieveOfEratosthenes(int[] numberLine)
        {
            foreach (int currentNumber in numberLine)
            {
                for(int i = 0; i < numberLine.Length; i++)
                {
                    if (numberLine[i] == currentNumber || numberLine[i] == 0 || currentNumber == 0) { continue; }

                    if (numberLine[i] % currentNumber == 0)
                    {
                        numberLine[i] = 0;
                    }
                }
            }

            numberLine = numberLine.Where(x => x != 0).ToArray();

            return numberLine;
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
