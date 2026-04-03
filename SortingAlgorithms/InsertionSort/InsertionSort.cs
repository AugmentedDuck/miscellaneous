using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class InsertionSort : ISortAlgorithm
    {
        public string Name()
        {
            return "Insertion Sort";
        }

        public int[] Sort(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                int key = array[i];
                int j = i - 1;

                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                }

                array[j + 1] = key;
            }


            return array;
        }
    }
}
