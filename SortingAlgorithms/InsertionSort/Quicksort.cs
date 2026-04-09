using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class Quicksort : ISortAlgorithm
    {
        int type = 0;

        readonly Random rng = new();

        public Quicksort(int type)
        {
            this.type = type;
        }

        public string Name()
        {
            switch (type)
            {
                case 1:
                    return "Quicksort: Random pivot";
                case 2:
                    return "Quicksort: Median of 3 - Deterministic";
                case 3:
                    return "Quicksort: Median of 3 - Randomized";
                default:
                    return "Quicksort: Last index is choosen as pivot";
            }
        }
        public void Sort(int[] array)
        {
            QuickSort(array, 0, array.Length - 1);
        }

        private void QuickSort(int[] array, int low, int high)
        {
            if (low >= high || low < 0)
            {
                return;
            }

            int pivot = PartitionLomuto(array, low, high);

            QuickSort(array, low, pivot - 1);
            QuickSort(array, pivot + 1, high);
        }
        /*
        private int PartitionHoare(int[] array, int low, int high)
        {
            int pivot = array[low];

            int leftIdx = low - 1;
            int rightIdx = high + 1;

            while (true)
            {
                do
                {
                    leftIdx++;
                }
                while (array[leftIdx] < pivot);

                do
                {
                    rightIdx--;
                } while (array[rightIdx] > pivot);

                if (leftIdx >= rightIdx)
                {
                    return rightIdx;
                }

                (array[leftIdx], array[rightIdx]) = (array[rightIdx], array[leftIdx]);
            }
        }
        */
        private int PartitionLomuto(int[] array, int low, int high)
        {
            int pivotIndex = high;

            switch (type)
            {
                case 1:
                    pivotIndex = rng.Next(low, high + 1);
                    break;
                case 2:
                    int midIndex = (low + high) / 2;

                    int a = array[low];
                    int b = array[midIndex];
                    int c = array[high];

                    if ((a <= b && b <= c) || (c <= b && b <= a)) { pivotIndex = midIndex; }
                    else if ((b <= a && a <= c) || (c <= a && a <= b)) { pivotIndex = low; }
                    else { pivotIndex = high; }
                    
                    break;
                case 3:
                    int i1 = rng.Next(low, high + 1);
                    int i2 = rng.Next(low, high + 1);
                    int i3 = rng.Next(low, high + 1);

                    int e1 = array[i1];
                    int e2 = array[i2];
                    int e3 = array[i3];

                    if ((e1 <= e2 && e2 <= e3) || (e3 <= e2 && e2 <= e1)) { pivotIndex = i2; }
                    else if ((e2 <= e1 && e1 <= e3) || (e3 <= e1 && e1 <= e2)) { pivotIndex = i1; }
                    else { pivotIndex = i3; }
                    
                    break;
            }

            (array[pivotIndex], array[high]) = (array[high], array[pivotIndex]);

            int pivot = array[high];

            int idx = low;

            for (int i = low; i < high; i++)
            {
                if (array[i] < pivot)
                {
                    (array[idx], array[i]) = (array[i], array[idx]);
                    idx++;
                }
            }

            (array[idx], array[high]) = (array[high], array[idx]);
            return idx;
        }
    }
}
