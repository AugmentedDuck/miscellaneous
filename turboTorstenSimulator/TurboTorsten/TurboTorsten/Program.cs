namespace TurboTorsten
{
    internal class Program
    {
        static readonly Random rng = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Running TurboTorsten Simulation");

            const int numberOfSimulations = 1000000;

            long sumRounds = 0;
            int min = int.MaxValue;
            int max = int.MinValue;
            
            Dictionary<int, int> roundsDistribution = new();

            for (int i = 0; i < numberOfSimulations; i++)
            {
                int rounds = simulateGame();

                sumRounds += rounds;

                min = rounds < min ? rounds : min;
                max = rounds > max ? rounds : max;

                if (roundsDistribution.ContainsKey(rounds))
                {
                    roundsDistribution[rounds]++;
                } 
                else
                {
                    roundsDistribution[rounds] = 1;
                }
            }

            List<KeyValuePair<int, int>> sortedDistribution = roundsDistribution.OrderBy(kvp => kvp.Key).ToList();

            using (StreamWriter writer = new("rounds_distribution.csv"))
            {
                writer.WriteLine("Rounds;Count;Probability");
                foreach (var kvp in sortedDistribution)
                {
                    double probability = (double)kvp.Value / numberOfSimulations;
                    writer.WriteLine($"{kvp.Key};{kvp.Value};{probability}");
                }
            }

            using (StreamWriter writer = new("CDF.csv"))
            {
                writer.WriteLine("Rounds;CDF");

                double cumulativeProbability = 0.0;

                foreach (var kvp in sortedDistribution)
                {
                    double probability = (double)kvp.Value / numberOfSimulations;
                    cumulativeProbability += probability;

                    writer.WriteLine($"{kvp.Key};{cumulativeProbability}");
                }
            }

            double average = (double)sumRounds / numberOfSimulations;

            Console.WriteLine(
                $"{average} rounds\n" +
                $"Min: {min}\n" +
                $"Max: {max}"
            );
        }

        static int simulateGame()
        {
            int rounds = 0;
            int currentRound = 0;

            bool[] shotGlasses = { false, false, false, false, false, false };


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
                }
            }

            return rounds;
        }
    }
}
