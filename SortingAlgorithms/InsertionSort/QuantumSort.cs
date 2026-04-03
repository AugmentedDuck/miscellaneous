using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class QuantumSort : ISortAlgorithm
    {
        readonly Random rng = new();

        public string Name()
        {
            return "Quantum Sort";
        }

        public int[] Sort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int idx2 = rng.Next(i, array.Length);

                int temp = array[i];
                array[i] = array[idx2];
                array[idx2] = temp;
            }

            return array;
        }
    }
}
