using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class StoogeSort : ISortAlgorithm
    {
        public string Name()
        {
            return "Stooge Sort";
        }

        public void Sort(int[] array)
        {
            StoogeSortRecursive(array, 0, array.Length - 1);
        }

        private static void StoogeSortRecursive(int[] array, int left, int right)
        {
            if (array[left] > array[right])
            {
                (array[left], array[right]) = (array[right], array[left]);
            }

            if ((right - left + 1) > 2)
            {
                int third = (right - left + 1) / 3;
                StoogeSortRecursive(array, left, right - third);
                StoogeSortRecursive(array, left + third, right);
                StoogeSortRecursive(array, left, right - third);
            }
        }
    }
}
