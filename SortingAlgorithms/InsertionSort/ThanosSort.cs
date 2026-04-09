using System;
using System.Collections.Generic;
using System.Text;

namespace SortingAlgorithms
{
    internal class ThanosSort : ISortAlgorithm
    {
        public string Name()
        {
            return "Thanos Sort";
        }

        public int[] Sort(int[] array)
        {
            List<int> list = [.. array];

            int idx = 0;

            Random rng = new();

            while (true)
            {
                if (idx + 1 < list.Count && list[idx] > list[idx + 1])
                {
                    for (int i = 0; i < list.Count / 2; i++)
                    {
                        int idxRemove = rng.Next(0, list.Count);
                        list.RemoveAt(idxRemove);
                    }

                    idx = 0;
                    continue;
                }

                if (idx + 1 >= list.Count)
                {
                    break;
                }

                idx++;
            }

            return [.. list];
        }
    }
}
