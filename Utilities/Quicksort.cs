using System.Collections.Generic;
using System.Linq;
using NeOMecS.Physics;

namespace NeOMecS.Utilities;

public static class QuickSort
{
    public static List<Body> SortBodiesByNesting(List<Body> list)
    {
        if(list.Count < 1) return list;

        return Sort(list.ToArray(), 0, list.Count - 1).ToList();
    }

    private static Body[] Sort(Body[] array, int left, int right)
    {
        var a = left;
        var b = right;
        var pivot = array[left];
        while (a <= b)
        {
            while (array[a].GetParentNestingCount() < pivot.GetParentNestingCount())
            {
                a++;
            }

            while (array[b].GetParentNestingCount() > pivot.GetParentNestingCount())
            {
                b--;
            }

            if (a <= b)
            {
                Body swap = array[a];
                array[a] = array[b];
                array[b] = swap;
                a++;
                b--;
            }
        }

        if (left < b)
            array = Sort(array, left, b);
        if (a < right)
            array = Sort(array, a, right);
        return array;
    }
}