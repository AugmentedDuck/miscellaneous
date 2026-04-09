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

        public void Sort(int[] array)
        {
            while (!Util.IsSorted(array)) { Thread.Sleep(1000); }
        }
    }
}
