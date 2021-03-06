using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapGenerator.Utils
{
    /// <summary>
    /// This class is responsible for generating blur.
    /// </summary>
    public static class BoxBlur
    {
        /// <summary>
        /// Generates a 2D array with blurred values of the original array.
        /// </summary>
        /// <param name="values">Selected array for blurring.</param>
        /// <param name="radius">Blur strength.</param>
        /// <returns>Returns a 2D floating point array with blurred values.</returns>
        public static float[,] Generate(float[,] values, int radius)
        {
            int halfRadius = radius / 2;
            int width = values.GetLength(0);
            int height = values.GetLength(1);
            float minValue = float.MaxValue;
            float maxValue = float.MinValue;
            float[,] blur = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float sum = 0;

                    for (int localX = x - halfRadius; localX < x + halfRadius; localX++)
                    {
                        for (int localY = y - halfRadius; localY < y + halfRadius; localY++)
                        {
                            if (localX < 0 || localY < 0 || localX >= width || localY >= height)
                                sum += 0;
                            else
                                sum += values[localX, localY];
                        }
                    }

                    blur[x, y] = sum / (radius * radius);

                    if (minValue >= blur[x, y]) minValue = blur[x, y];
                    if (maxValue <= blur[x, y]) maxValue = blur[x, y];
                }
            }

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    blur[x, y] = MathfExtend.Map(blur[x, y], minValue, maxValue, 0, 1f);

            return blur;
        }
    }
}