using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// new System.Random().Shuffle(normalPaintings);
/// </summary>
/// 
namespace bonbon
{
    static class RandomExtension
    {
        public static void Shuffle<T>(this System.Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static void Shuffle<T>(this System.Random rng, List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

        public static bool RandomPercentage(float percent)
        {
            if (percent >= 100.0f)
                return true;
            else if (percent <= 0f)
                return false;

            return UnityEngine.Random.value * 100 <= percent;
        }

        /// <summary>
        /// Random with percentage result. Example usage: RandomPercentage(20, 30, 50) will return 1, 2 or 3, with the ratio of 20%, 30%, 50%.
        /// </summary>
        /// <param name="percentages">Array of percentages, sum of all array  must be equals to 100.</param>
        /// <returns>From 1 to percentages.Length, with the corresponding percentage.</returns>
        public static int RandomPercentage(params float[] percentages)
        {
            // If there is only 1 param, random between X% and (100-X)%
            if (percentages.Length == 1)
                return UnityEngine.Random.Range(0.0f, 100.0f) <= percentages[0] ? 0 : 1;

            float ratio = UnityEngine.Random.Range(0.0f, MathUtils.Sum(percentages));
            float total = 0;
            for (int i = 0; i < percentages.Length; i++)
            {
                if (ratio >= total && ratio <= total + percentages[i])
                {
                    return i + 1;
                }
                else
                {
                    total += percentages[i];
                }
            }
            return 0;
        }
    }
}