using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class StalinSort : ISortAlgorithm
    {
        public string Name()
        {
            return "Stalin Sort";
        }

        public int[] Sort(int[] array)
        {
            List<int> list = [];
            for (int i = 0; i < array.Length; i++)
            {
                if (i + 1 < array.Length && array[i] < array[i + 1])
                {
                    list.Add(array[i]);
                }
            }

            return [.. list];
        }
    }
}
