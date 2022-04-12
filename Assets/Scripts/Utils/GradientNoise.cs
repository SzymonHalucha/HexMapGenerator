using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = HexMapGenerator.Utils.Random;

namespace HexMapGenerator.Utils
{
    public static class GradientNoise
    {
        public static float[,] Generate(Random random, GradientNoiseData data, int width, int height)
        {
            float[,] array = new float[width, height];
            Vector2[] octavesOffset = new Vector2[data.Octaves];

            for (int i = 0; i < data.Octaves; i++)
            {
                float offsetX = random.Range(-10000f, 10000f);
                float offsetY = random.Range(-10000f, 10000f);
                octavesOffset[i] = new Vector2(offsetX, offsetY);
            }

            float maxValue = float.MinValue;
            float minValue = float.MaxValue;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float perlin = 0;
                    float amplitude = 1f;
                    float frequency = 1f;

                    for (int i = 0; i < data.Octaves; i++)
                    {
                        float scaledX = (x + octavesOffset[i].x - (width / 2f)) / data.Scale * frequency;
                        float scaledY = (y + octavesOffset[i].y - (height / 2f)) / data.Scale * frequency;
                        perlin += (Mathf.PerlinNoise(scaledX, scaledY) * 2f - 1f) * amplitude;

                        frequency *= data.Lacunarity;
                        amplitude *= data.Persistance;
                    }

                    if (perlin > maxValue) maxValue = perlin;
                    else if (perlin < minValue) minValue = perlin;

                    array[x, y] = perlin;
                }
            }

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    array[x, y] = MathfExtend.Map(array[x, y], minValue, maxValue, 0, 1f);

            return array;
        }
    }
}