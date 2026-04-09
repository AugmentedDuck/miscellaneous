using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class Slowsort : ISortAlgorithm
    {
        public string Name()
        {
            return "Slowsort";
        }
        public void Sort(int[] array)
        {
            SlowsortRecursive(array, 0, array.Length - 1);
        }

        private static void SlowsortRecursive(int[] array, int start, int end)
        {
            if (start >= end)
            {
                return;
            }

            int middle = (int)Math.Floor((start + end) / 2.0f);

            SlowsortRecursive(array, start, middle);
            SlowsortRecursive(array, middle + 1, end);

            if (array[end] < array[middle])
            {
                (array[end], array[middle]) = (array[middle], array[end]);
            }

            SlowsortRecursive(array, start, end - 1);
        }
    }
}
