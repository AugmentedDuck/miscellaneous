using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class Heapsort : ISortAlgorithm
    {
        public string Name()
        {
            return "Heapsort";
        }
        public void Sort(int[] array)
        {
            Heapify(array);

            for (int i = array.Length - 1; i > 0; i--)
            {
                (array[i], array[0]) = (array[0], array[i]);
                SiftDown(array, i, 0);
            }
        }

        private static void Heapify(int[] array)
        {
            int n = array.Length;

            int start = (n / 2) - 1;

            for (int i = start; i >= 0; i--)
            {
                SiftDown(array, n, i);
            }
        }

        private static void SiftDown(int[] array, int n, int i)
        {
            int largest = i;
            int left = (2 * i) + 1;
            int right = (2 * i) + 2;

            if (left < n && array[left] > array[largest])
            {
                largest = left;
            }

            if (right < n && array[right] > array[largest])
            {
                largest = right;
            }

            if (largest != i)
            {
                (array[i], array[largest]) = (array[largest], array[i]);
                SiftDown(array, n, largest);
            }
        }
    }
}
