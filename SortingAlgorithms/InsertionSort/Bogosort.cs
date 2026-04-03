using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class Bogosort : ISortAlgorithm
    {
        readonly Random rng = new();


        public string Name()
        {
            return "Bogosort";
        }

        public void Sort(int[] array)
        {
            while (!IsSorted(array))
            {
                Shuffle(array);
            }
        }

        private void Shuffle(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int idx2 = rng.Next(i, array.Length);

                (array[idx2], array[i]) = (array[i], array[idx2]);
            }
        }

        private static bool IsSorted(int[] array)
        {
            int idx = 0;

            while (idx < array.Length)
            {
                if (idx + 1 < array.Length && array[idx] > array[idx + 1])
                {
                    return false;
                }
                idx++;
            }

            return true;
        }
    }
}
