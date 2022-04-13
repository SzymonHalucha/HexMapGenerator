using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = HexMapGenerator.Utils.Random;

namespace HexMapGenerator.Utils
{
    /// <summary>
    /// This class is responsible for generating random points at equal distances from each other.
    /// Source: https://www.youtube.com/watch?v=7WcmyxyFO7o
    /// </summary>
    public static class BlueNoise
    {
        /// <summary>
        /// Generates a list with random points equally spaced from each other.
        /// </summary>
        /// <param name="random">Selected random number generator.</param>
        /// <param name="radius">Distance between points.</param>
        /// <param name="hasCenterPoint">Generating a center point (0.5, 0.5).</param>
        /// <param name="sampling">The maximum number of attempts to select the correct position for a point.</param>
        /// <returns>Returns a list of points.</returns>
        public static List<Vector2> Generate(Random random, float radius, bool hasCenterPoint, int sampling = 10)
        {
            float mapSize = 1f;
            float cellSize = radius / Mathf.Sqrt(2f);

            int gridSize = Mathf.CeilToInt(mapSize / cellSize);
            int[,] grid = new int[gridSize, gridSize];

            List<Vector2> allPoints = new List<Vector2>();
            List<Vector2> activePoints = new List<Vector2>();

            if (hasCenterPoint) AddPoint(new Vector2(mapSize / 2f, mapSize / 2f), allPoints, activePoints, grid, cellSize);
            else activePoints.Add(new Vector2(mapSize / 2f, mapSize / 2f));

            while (activePoints.Count > 0)
            {
                int randomIndex = random.Range(activePoints.Count);
                Vector2 current = activePoints[randomIndex];

                float seed = random.Value();
                float epsilon = 0.0000001f;
                float radiusEpsilon = radius + epsilon;

                bool isPointAdded = false;

                for (int i = 0; i < sampling; i++)
                {
                    float randomAngle = 2f * Mathf.PI * (seed + 1f * (float)i / (float)sampling);

                    Vector2 next = new Vector2();
                    next.x = current.x + radiusEpsilon * Mathf.Cos(randomAngle);
                    next.y = current.y + radiusEpsilon * Mathf.Sin(randomAngle);

                    if (next.x >= 0 && next.y >= 0 && next.x < mapSize && next.y < mapSize)
                    {
                        if (!IsPointValid(next, allPoints, grid, cellSize, radius)) continue;

                        AddPoint(next, allPoints, activePoints, grid, cellSize);
                        isPointAdded = true;
                        break;
                    }
                }

                if (!isPointAdded)
                {
                    Vector2 value = activePoints[activePoints.Count - 1];
                    activePoints.RemoveAt(activePoints.Count - 1);

                    if (randomIndex < activePoints.Count) activePoints[randomIndex] = value;

                    activePoints.Remove(current);
                }
            }

            return allPoints;
        }

        private static void AddPoint(Vector2 position, List<Vector2> allPoints, List<Vector2> activePoints, int[,] grid, float cellSize)
        {
            allPoints.Add(position);
            activePoints.Add(position);

            int gridX = (int)(position.x / cellSize);
            int gridY = (int)(position.y / cellSize);
            grid[gridX, gridY] = allPoints.Count;
        }

        private static bool IsPointValid(Vector2 position, List<Vector2> allPoints, int[,] grid, float cellSize, float radius)
        {
            int gridX = (int)(position.x / cellSize);
            int gridY = (int)(position.y / cellSize);

            int startX = Mathf.Max(gridX - 2, 0);
            int startY = Mathf.Max(gridY - 2, 0);

            int endX = Mathf.Min(gridX + 3, grid.GetLength(0));
            int endY = Mathf.Min(gridY + 3, grid.GetLength(1));

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    int index = grid[x, y] - 1;
                    if (index <= -1) continue;

                    float squareDistance = (position - allPoints[index]).sqrMagnitude;
                    if (squareDistance < radius * radius) return false;
                }
            }

            return true;
        }
    }
}