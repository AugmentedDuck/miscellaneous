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

        public void Sort(int[] array)
        {
            List<int> list = [];
            list.Add(array[0]);

            for (int i = 0; i < array.Length; i++)
            {
                if (i + 1 < array.Length && array[i] < array[i + 1])
                {
                    list.Add(array[i + 1]);
                }
            }

            int idx = 0;

            while (idx < array.Length)
            {
                if (list.Count > idx)
                {
                    array[idx] = list[idx];
                }
                else
                {
                    array[idx] = 0;
                }
                idx++;
            } 
        }
    }
}
