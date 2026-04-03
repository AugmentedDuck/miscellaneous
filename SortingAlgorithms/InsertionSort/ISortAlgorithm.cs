using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal interface ISortAlgorithm
    {
        public void Sort(int[] array);

        public string Name();
    }
}
