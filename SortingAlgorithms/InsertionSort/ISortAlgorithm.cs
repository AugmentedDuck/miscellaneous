using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal interface ISortAlgorithm
    {
        public int[] Sort(int[] array);

        public string Name();
    }
}
