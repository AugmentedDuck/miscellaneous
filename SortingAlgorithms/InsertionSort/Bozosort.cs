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

        public void Sort(int[] array)
        {
            while (!Util.IsSorted(array))
            {
                ShuffleTwo(array);
            }
        }

        private void ShuffleTwo(int[] array)
        {
            int idx1 = rng.Next(0, array.Length);
            int idx2;

            do
            {
               idx2 = rng.Next(0, array.Length);
            } while (idx1 == idx2);

            (array[idx1], array[idx2]) = (array[idx2], array[idx1]);
        }
    }
}
