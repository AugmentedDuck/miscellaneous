using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class MiracleSort : ISortAlgorithm
    {
        public string Name()
        {
            return "Miracle Sort";
        }

        public int[] Sort(int[] array)
        {
            while (!IsSorted(array)) { Thread.Sleep(1000); }

            return array;
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
