using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class Bozosort : ISortAlgorithm
    {
        readonly Random rng = new();


        public string Name()
        {
            return "Bozosort";
        }

        public int[] Sort(int[] array)
        {
            while (!IsSorted(array))
            {
                ShuffleTwo(array);
            }

            return array;
        }

        private void ShuffleTwo(int[] array)
        {
            int idx1 = rng.Next(0, array.Length);
            int idx2;

            do
            {
               idx2 = rng.Next(0, array.Length);
            } while (idx1 == idx2);

            int temp = array[idx2];
            array[idx2] = array[idx1];
            array[idx1] = temp;
        }

        private bool IsSorted(int[] array)
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
