using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace TurboTorsten
{
    internal class Program
    {
        const int numberOfSimulations = 10_000_000;
        const int maxExpectedRounds = 50_000;

        static void Main(string[] args)
        {
            Console.WriteLine($".NET {Environment.Version} + AVX2 {Avx2.IsSupported} - Simulating {numberOfSimulations} simulations");
            var sw = Stopwatch.StartNew();

            int vectorSize = Vector256<int>.Count;

            int processorCount = Environment.ProcessorCount;
            long[][] workerHistograms = new long[processorCount][];
            for (int i = 0; i < processorCount; i++) workerHistograms[i] = new long[maxExpectedRounds];

            int simsPerWorker = (numberOfSimulations / processorCount); // vectorSize;

            Parallel.For(0, processorCount, workerId =>
            {
                long[] localHist = workerHistograms[workerId];

                uint baseSeed = (uint)(workerId + 1) * 123456789u;

                // Vector256<uint> rngState = Vector256.Create(baseSeed, baseSeed + 1, baseSeed + 2, baseSeed + 3, baseSeed + 4, baseSeed + 5, baseSeed + 6, baseSeed + 7);

                for (int i = 0; i < simsPerWorker; i++)
                {
                    // Vector256<int> results = SimulateGame(ref rngState);
                    int rounds = SimulateGame(ref baseSeed);

                    if (rounds < maxExpectedRounds)
                    {
                        localHist[rounds]++;
                    }
                }
            });

            // Merge results
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
            // Console.ReadLine();
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

                int hit = ((shotMask & dice) ^ dice) >> (int)roll;

                shotMask ^= dice;

                rounds += hit;

                consecutiveMisses = (consecutiveMisses + 1) & (hit - 1);
            }

            return rounds;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector256<int> SimulateGame(ref Vector256<uint> rngStates)
        {
            Vector256<int> rounds = Vector256<int>.Zero;
            Vector256<int> consecutiveMisses = Vector256<int>.Zero;
            Vector256<int> shotMask = Vector256<int>.Zero;

            Vector256<int> activeMask = Vector256.Create(-1);
            Vector256<int> limit = Vector256.Create(6);

            while (activeMask != Vector256<int>.Zero)
            {
                // int roll = (int)((Xorshift32(ref rngStates) * 6UL) >> 32);
                Vector256<uint> rng = Xorshift32(ref rngStates);

                var (low, high) = Vector256.Widen(rng);
                Vector256<uint> rollLow = Vector256.ShiftRightLogical(low * 6, 32).AsUInt32();
                Vector256<uint> rollHigh = Vector256.ShiftRightLogical(high * 6, 32).AsUInt32();

                Vector256<uint> roll = Vector256.Narrow(rollLow, rollHigh).AsUInt32();

                // int dice = 1 << roll;
                Vector256<int> dice = Avx2.ShiftLeftLogicalVariable(Vector256.Create(1), roll);

                // int hit = ((shotMask & dice) ^ dice) >> (int)roll;
                Vector256<int> hit = Avx2.ShiftRightLogicalVariable(Vector256.Xor(Avx2.And(shotMask, dice), dice), roll);
                hit = Avx2.And(hit, activeMask);

                // shotMask ^= dice;
                shotMask = Vector256.Xor(shotMask, Avx2.And(dice, activeMask));

                // rounds += hit;
                rounds = Vector256.Add(rounds, hit);

                // consecutiveMisses = (consecutiveMisses + 1) & (hit - 1);
                Vector256<int> nextMisses = Avx2.And(Vector256.Add(consecutiveMisses, Vector256.Create(1)), Vector256.Subtract(hit, Vector256.Create(1)));
                consecutiveMisses = Vector256.ConditionalSelect(activeMask, nextMisses, consecutiveMisses);

                Vector256<int> stillRunning = Vector256.LessThan(consecutiveMisses, limit);
                activeMask = Avx2.And(activeMask, stillRunning);
            }

            return rounds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector256<int> CombineAndShift(Vector256<ulong> low, Vector256<ulong> high)
        {
            // Shift right 32 and cast/shuffle to get our 8 integers back into one Vector256
            var rLow = Vector256.ShiftRightLogical(low, 32).AsInt32();
            var rHigh = Vector256.ShiftRightLogical(high, 32).AsInt32();

            // This part depends on .NET version, but Vector256.Create effectively combines them
            return Vector256.Shuffle(rLow, Vector256.Create(0, 2, 4, 6, 0, 0, 0, 0)) // Just an example of the complexity here
                   + Vector256.Shuffle(rHigh, Vector256.Create(0, 0, 0, 0, 0, 2, 4, 6));
            // Optimization: In .NET 8+, use Vector256.Narrow() for much cleaner code!
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector256<uint> Xorshift32(ref Vector256<uint> states)
        {
            Vector256<uint> x = states;
            x = Avx2.Xor(x, Avx2.ShiftLeftLogical(x, 13));
            x = Avx2.Xor(x, Avx2.ShiftRightLogical(x, 17));
            x = Avx2.Xor(x, Avx2.ShiftLeftLogical(x, 5));

            states = x;

            return x;
        }
    }
}
