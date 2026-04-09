using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace SortingAlgorithms
{
    internal class PancakeSort : ISortAlgorithm
    {
        public string Name()
        {
            return "Pancake Sort";
        }
        public void Sort(int[] array)
        {
            int n = array.Length;

            while (n > 1)
            {
                int maxIdx = MaxIndex(array, n);

                if (maxIdx != n - 1)
                {
                    if (maxIdx != 0)
                    {
                        ReverseArray(array, maxIdx);
                    }
                    ReverseArray(array, n - 1);
                }

                n--;
            }
        }

        private static void ReverseArray(int[] array, int right)
        {
            int left = 0;

            while (left < right)
            {
                (array[left], array[right]) = (array[right], array[left]);
                right--;
                left++;
            }
        }

        private static int MaxIndex(int[] array, int n)
        {
            int idx = 0;

            for (int i = 0; i < n; i++)
            {
                if (array[i] > array[idx])
                {
                    idx = i;
                }
            }

            return idx;
        }
    }
}
