using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class PatienceSort : ISortAlgorithm
    {
        public string Name()
        {
            return "Patience Sort";
        }

        public void Sort(int[] array)
        {
            List<List<int>> piles = GeneratePiles(array);

            for (int i = 0; i < array.Length; i++)
            {
                int smallest = int.MaxValue;
                int smallestIdx = 0;

                for (int j = 0; j < piles.Count; j++)
                {
                    if (piles[j].Last() < smallest)
                    {
                        smallestIdx = j;
                        smallest = piles[j].Last();
                    }
                }

                array[i] = piles[smallestIdx].Last();

                piles[smallestIdx].RemoveAt(piles[smallestIdx].Count - 1);

                if (piles[smallestIdx].Count == 0)
                {
                    piles.RemoveAt(smallestIdx);
                }
            }
        }

        private List<List<int>> GeneratePiles(int[] array)
        {
            List<List<int>> piles = [];

            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < piles.Count + 1; j++)
                {
                    if (j == piles.Count)
                    {
                        piles.Add([]);
                        piles[j].Add(array[i]);
                        break;
                    }

                    if (piles[j].Last() > array[i])
                    {
                        piles[j].Add(array[i]);
                        break;
                    }
                }
            }

            return piles;
        }
    }
}
