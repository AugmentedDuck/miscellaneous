using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class Bogobogosort : ISortAlgorithm
    {
        readonly Random rng = new();

        public string Name()
        {
            return "Bogobogosort";
        }

        public void Sort(int[] array)
        {
            BogobogosortRecursive(array);
        }

        private void BogobogosortRecursive(int[] array)
        {
            int idx = 2;

            while (!Util.IsSorted(array))
            {
                Bogosort(array[..idx]);
                idx++;
                
                if (!Util.IsSorted(array[..idx]))
                {
                    Shuffle(array);
                    idx = 2;
                }
            }
        }

        private void Bogosort(int[] array)
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
