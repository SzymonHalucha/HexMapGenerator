using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapGenerator.Utils
{
    /// <summary>
    /// This class is responsible for generating a gradient mask of one of three types to choose from.
    /// </summary>
    public static class GradientMask
    {
        public enum MaskType { Disk, Manhattan, Euclidean }

        /// <summary>
        /// Generates a 2D array with a gradient mask.
        /// </summary>
        /// <param name="type">Selected mask type.</param>
        /// <param name="radius">Radius of the mask.</param>
        /// <param name="width">The width of the array.</param>
        /// <param name="height">The height of the array.</param>
        /// <param name="inverse">Invert the array values.</param>
        /// <returns>Returns a 2D floating point array of the gradient mask.</returns>
        public static float[,] Generate(MaskType type, float radius, int width, int height, bool inverse = false)
        {
            float[,] array = new float[width, height];
            float minValue = float.MaxValue;
            float maxValue = float.MinValue;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float sampleX = MathfExtend.Map(x, 0, width - 1, -1f, 1f);
                    float sampleY = MathfExtend.Map(y, 0, height - 1, -1f, 1f);
                    float distance = 0;

                    switch (type)
                    {
                        case MaskType.Disk:
                            distance = 1f - (sampleX * sampleX + sampleY * sampleY);
                            break;

                        case MaskType.Manhattan:
                            distance = 1f - (Mathf.Abs(sampleX) + Mathf.Abs(sampleY));
                            break;

                        case MaskType.Euclidean:
                            distance = 1f - Mathf.Sqrt((sampleX * sampleX) + (sampleY * sampleY));
                            break;
                    }

                    distance = Mathf.Pow(distance, radius);

                    if (maxValue < distance) maxValue = distance;
                    else if (minValue > distance) minValue = distance;

                    array[x, y] = distance;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (inverse)
                    {
                        array[x, y] = MathfExtend.Map(array[x, y], minValue, maxValue, 0, 1f);
                        if (float.IsNaN(array[x, y])) array[x, y] = 0;
                    }
                    else
                    {
                        array[x, y] = MathfExtend.Map(array[x, y], minValue, maxValue, 1f, 0);
                        if (float.IsNaN(array[x, y])) array[x, y] = 1f;
                    }
                }
            }

            return array;
        }
    }
}