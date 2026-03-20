using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace TurboTorsten
{
    internal class Program
    {
        const int numberOfSimulations = 1_000_000;
        const int maxExpectedRounds = 50_000;

        static void Main(string[] args)
        {
            Console.WriteLine($".NET {Environment.Version} + AVX2 {Avx2.IsSupported} - Simulating {numberOfSimulations} simulations");
            var sw = Stopwatch.StartNew();

            int processorCount = Environment.ProcessorCount;
            long[][] workerHistograms = new long[processorCount][];
            for (int i = 0; i < processorCount; i++) workerHistograms[i] = new long[maxExpectedRounds];

            int simsPerWorker = numberOfSimulations / processorCount;

            Parallel.For(0, processorCount, workerId =>
            {
                long[] localHist = workerHistograms[workerId];

                uint rngState = (uint)(workerId + 1) * 123456789u;

                for (int i = 0; i < simsPerWorker; i++)
                {
                    int rounds = SimulateGame(ref rngState);
                    if (rounds < maxExpectedRounds)
                    {
                        localHist[rounds]++;
                    }
                }
            });

            // Merge results at the very end
            long[] globalHistogram = new long[maxExpectedRounds];
            for (int i = 0; i < maxExpectedRounds; i++)
            {
                for (int w = 0; w < processorCount; w++)
                {
                    globalHistogram[i] += workerHistograms[w][i];
                }
            }

            Console.WriteLine($"Simulated {numberOfSimulations:N0} games in {sw.ElapsedMilliseconds}ms.");

            using (var distWriter = new StreamWriter("rounds_distribution.csv"))
            using (var cdfWriter = new StreamWriter("CDF.csv"))
            {
                distWriter.WriteLine("Rounds;Count;Probability");
                cdfWriter.WriteLine("Rounds;CDF");

                double cumulativeProbability = 0;
                for (int i = 0; i < globalHistogram.Length; i++)
                {
                    if (globalHistogram[i] == 0 && cumulativeProbability >= 0.999999) continue;

                    double prob = (double)globalHistogram[i] / numberOfSimulations;
                    cumulativeProbability += prob;

                    distWriter.WriteLine($"{i};{globalHistogram[i]};{prob}");
                    cdfWriter.WriteLine($"{i};{cumulativeProbability}");
                }
            }

            Console.WriteLine("Results saved to rounds_distribution.csv and CDF.csv");
            Console.ReadLine();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int SimulateGame(ref uint rngState)
        {
            int rounds = 0;
            int consecutiveMisses = 0;
            int shotMask = 0;

            while (consecutiveMisses < 6)
            {
                int roll = (int)((Xorshift32(ref rngState) * 6UL) >> 32);
                int dice = 1 << roll;

                int hit = ((shotMask & dice) == 0) ? 1 : 0;

                shotMask ^= dice;

                rounds += hit;

                consecutiveMisses = (consecutiveMisses + 1) * (1 - hit);
            }

            return rounds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint Xorshift32(ref uint state)
        {
            uint x = state;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            state = x;
            return x;
        }
    }
}
