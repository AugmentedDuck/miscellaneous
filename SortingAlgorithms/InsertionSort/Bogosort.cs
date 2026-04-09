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
            while (!Util.IsSorted(array))
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
    }
}
