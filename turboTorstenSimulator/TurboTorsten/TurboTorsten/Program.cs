namespace TurboTorsten
{
    internal class Program
    {
        static ThreadLocal<Random> rng = new(() => new Random(Guid.NewGuid().GetHashCode()));

        static void Main(string[] args)
        {
            Console.WriteLine("Running TurboTorsten Simulation");

            const int numberOfSimulations = 10000000;
           
            Dictionary<int, int> globalHistogram = new();

            object lockObj = new();

            Parallel.For(0, numberOfSimulations,
                () => new Dictionary<int, int>(),

                (i, state, localHist) =>
                {
                    int rounds = simulateGame();

                    if (localHist.ContainsKey(rounds))
                    {
                        localHist[rounds]++;
                    }
                    else
                    {
                        localHist[rounds] = 1;
                    }

                    return localHist;
                },

                localHist =>
                {
                    lock (lockObj)
                    {
                        foreach (var kvp in localHist)
                        {
                            if (globalHistogram.ContainsKey(kvp.Key))
                            {
                                globalHistogram[kvp.Key] += kvp.Value;
                            }
                            else
                            {
                                globalHistogram[kvp.Key] = kvp.Value;
                            }
                        }
                    }
                }
            );

            var sortedHistogram = globalHistogram.OrderBy(kvp => kvp.Key).ToList();

            using (StreamWriter writer = new("rounds_distribution.csv"))
            {
                writer.WriteLine("Rounds;Count;Probability");

                foreach (var kvp in sortedHistogram)
                {
                    double probability = (double)kvp.Value / numberOfSimulations;
                    writer.WriteLine($"{kvp.Key};{kvp.Value};{probability}");
                }
            }

            using (StreamWriter writer = new("CDF.csv"))
            {
                writer.WriteLine("Rounds;CDF");

                double cumulativeProbability = 0;

                foreach (var kvp in sortedHistogram)
                {
                    double probability = (double)kvp.Value / numberOfSimulations;
                    cumulativeProbability += probability;
                    writer.WriteLine($"{kvp.Key};{cumulativeProbability}");
                }
            }

            Console.WriteLine("Simulation completed. Results saved to rounds_distribution.csv and CDF.csv");
        }

        static int simulateGame()
        {
            int rounds = 0;
            int currentRound = 0;

            int shotMask = 0;


            while (currentRound != 6)
            {
                int dice = rng.Value.Next(0, 6);

                if ((shotMask & (1 << dice)) == 0)
                {
                    shotMask |= (1 << dice);
                    rounds++;
                    currentRound = 0;
                } else
                {
                    shotMask &= ~(1 << dice);
                    currentRound++;
                }
            }

            return rounds;
        }
    }
}
