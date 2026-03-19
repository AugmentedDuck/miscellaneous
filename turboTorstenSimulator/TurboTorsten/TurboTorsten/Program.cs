namespace TurboTorsten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            List<int> rounds = new List<int>();

            for (int i = 0; i < 1000000; i++)
            {
                rounds.Add(simulateGame());
            }

            int averageRound = 0;
            //int averageDrinks = 0;

            foreach (int round in rounds)
            {
                averageRound += round;
                //averageDrinks += round[0];
            }

            averageRound /= rounds.Count;
            //averageDrinks /= rounds.Count;

            rounds.Sort();

            int roundMean = rounds[rounds.Count / 2];

            Console.WriteLine(averageRound.ToString() + " rounds\nThe mean number of rounds is: " + roundMean + "\nMin: " + rounds.Min() + "\nMax: " + rounds.Max());
        }

        static int simulateGame()
        {
            int rounds = 0;
            int currentRound = 0;
            int drinks = 0;


            bool[] shotGlasses = { false, false, false, false, false, false };

            Random rng = new();

            while (currentRound != 6)
            {
                int dice = rng.Next(0, 6);

                if (!shotGlasses[dice])
                {
                    shotGlasses[dice] = true;
                    rounds++;
                    currentRound = 0;
                } else
                {
                    shotGlasses[dice] = false;
                    currentRound++;
                    drinks++;
                }
            }

            int[] data = { drinks, rounds };

            return rounds;
        }
    }
}
