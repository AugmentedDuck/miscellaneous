using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class Util
    {
        public static bool IsSorted(int[] array)
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
