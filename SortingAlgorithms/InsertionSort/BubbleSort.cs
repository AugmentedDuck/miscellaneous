using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class BubbleSort : ISortAlgorithm
    {
        public string Name()
        {
            return "Bubble Sort";
        }

        public void Sort(int[] array)
        {
            int length = array.Length;
            while (length >= 1)
            {
                int newLength = 0;
                for (int i = 1; i < length; i++)
                {
                    if (array[i - 1] > array[i])
                    {
                        (array[i], array[i - 1]) = (array[i - 1], array[i]);
                        newLength = i;
                    }
                }

                length = newLength;
            }
        }
    }
}
