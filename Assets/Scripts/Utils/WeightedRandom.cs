using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = HexMapGenerator.Utils.Random;

namespace HexMapGenerator.Utils
{
    /// <summary>
    /// This class provides a weighted random draw.
    /// </summary>
    /// <typeparam name="T">Selected generic element.</typeparam>
    [Serializable]
    public class WeightedRandom<T>
    {
        public T Value;
        [Range(0, 1f)] public float Weight;

        public WeightedRandom(T value, float weight)
        {
            this.Value = value;
            this.Weight = weight;
        }

        /// <summary>
        /// Randomize by weight an element from the selected array. 
        /// </summary>
        /// <param name="random">Selected random number generator.</param>
        /// <param name="array">Selected array.</param>
        /// <returns>Returns the drawn element.</returns>
        public static T GetRandomValueFromArray(Random random, WeightedRandom<T>[] array)
        {
            float weightSum = 0;
            float weightCurrent = 0;
            float weightPrevious = 0;

            foreach (WeightedRandom<T> item in array)
                weightSum += item.Weight;

            float selectedWeight = random.Range(0, weightSum);

            foreach (WeightedRandom<T> item in array)
            {
                weightCurrent += item.Weight;

                if (selectedWeight <= weightCurrent && selectedWeight >= weightPrevious)
                    return item.Value;

                weightPrevious += item.Weight;
            }

            return default(T);
        }

        /// <summary>
        /// Randomize by weight an element from the selected list. 
        /// </summary>
        /// <param name="random">Selected random number generator.</param>
        /// <param name="list">Selected list.</param>
        /// <returns>Returns the drawn element.</returns>
        public static T GetRandomValueFromList(Random random, List<WeightedRandom<T>> list)
        {
            float weightSum = 0;
            float weightCurrent = 0;
            float weightPrevious = 0;

            foreach (WeightedRandom<T> item in list)
                weightSum += item.Weight;

            float selectedWeight = random.Range(0, weightSum);

            foreach (WeightedRandom<T> item in list)
            {
                weightCurrent += item.Weight;

                if (selectedWeight <= weightCurrent && selectedWeight >= weightPrevious)
                    return item.Value;

                weightPrevious += item.Weight;
            }

            return default(T);
        }
    }
}