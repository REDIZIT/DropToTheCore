using System;
using System.Collections.Generic;
using System.Linq;

public static class LinqExtensions
{
    public static T Random<T>(this IEnumerable<T> ls)
    {
        return ls.ElementAt(UnityEngine.Random.Range(0, ls.Count()));
    }
    public static T Random<T>(this IEnumerable<T> ls, Func<T, float> weightFunc)
    {
        if (ls.Count() == 0) throw new Exception($"Unable to select weighted random element from empty IEnumerable<{typeof(T)}>");

        float sum = ls.Sum(weightFunc);
        float randomValue = UnityEngine.Random.Range(0, sum);

        foreach (T element in ls)
        {
            float elementWeight = weightFunc(element);

            if (randomValue <= elementWeight)
            {
                return element;
            }
            else
            {
                randomValue -= elementWeight;
            }
        }

        throw new Exception("Unable to select random element by weight in " + ls);
    }
}