using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> RandomPick<T>(this IEnumerable<T> enumerable, int maximumCount, bool uniquePick)
    {
        List<T> lookupList = enumerable.ToList();

        if (uniquePick)
        {
            maximumCount = System.Math.Min(maximumCount, lookupList.Count);

            for (int count = 0; count < maximumCount; ++count)
            {
                int lookupIndex = Random.Range(count, lookupList.Count);
                yield return lookupList[lookupIndex];
                lookupList[lookupIndex] = lookupList[count];
            }
        }
        else
        {
            for (int count = 0; count < maximumCount; ++count)
            {
                int lookupIndex = Random.Range(0, lookupList.Count);
                yield return lookupList[lookupIndex];
            }
        }
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
        List<T> lookupList = enumerable.ToList();

        for (int count = 0; count < lookupList.Count; ++count)
        {
            int lookupIndex = Random.Range(count, lookupList.Count);
            yield return lookupList[lookupIndex];
            lookupList[lookupIndex] = lookupList[count];
        }
    }
}
