using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;
using Random = HexMapGenerator.Utils.Random;

namespace HexMapGenerator.Generation
{
    /// <summary>
    /// This class allows you to generate map terrain and biomes and prefabs placed on the map.
    /// </summary>
    public static class Generator
    {
        /// <summary>
        /// Generates terrain in the form of an island.
        /// </summary>
        /// <param name="data">Information needed to generate a terrain according to specifications.</param>
        /// <param name="world">Storage location of the generated map.</param>
        public static void GenerateLand(GeneratorData data, Block[,] world)
        {
            float[,] land = GradientNoise.Generate(data.WorldRandom, data.LandNoiseData, data.Size.x, data.Size.y);
            float[,] mask = GradientMask.Generate(GradientMask.MaskType.Disk, data.LandMaskRadius, data.Size.x, data.Size.y);

            for (int x = 0; x < data.Size.x; x++)
            {
                for (int y = 0; y < data.Size.y; y++)
                {
                    float value = Mathf.Clamp01(land[x, y] - mask[x, y]);
                    world[x, y] = new Block(value > 0 ? true : false, value);
                }
            }
        }

        /// <summary>
        /// Generates biomes and prefabs specific to given biome types.
        /// </summary>
        /// <param name="data">Information needed to generate a biomes according to specifications.</param>
        /// <param name="world">Storage location of the generated map.</param>
        public static void GenerateBiomes(GeneratorData data, Block[,] world)
        {
            float[,] biomes = GradientNoise.Generate(data.WorldRandom, data.BiomesNoiseData, data.Size.x, data.Size.y);

            for (int x = 0; x < data.Size.x; x++)
                for (int y = 0; y < data.Size.y; y++)
                    world[x, y].BiomeValue = biomes[x, y];

            for (int i = 0; i < data.Biomes.Count; i++)
            {
                GenerateTilesColors(data.Biomes[i], world, data.WorldRandom);
                GenerateTilesPrefabs(data.Biomes[i], world, data.WorldRandom);
            }
        }

        /// <summary>
        /// Assigns the hexagon a color drawn from the variants for the given biome.
        /// </summary>
        /// <param name="biome">Selected biome.</param>
        /// <param name="world">Storage location of the generated map.</param>
        /// <param name="random">Selected random number generator.</param>
        private static void GenerateTilesColors(BiomeData biome, Block[,] world, Random random)
        {
            int width = world.GetLength(0);
            int height = world.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!world[x, y].IsSolid) continue;
                    if (!biome.LandThreshold.IsValueInRange(world[x, y].LandValue)) continue;
                    if (!biome.BiomeThreshold.IsValueInRange(world[x, y].BiomeValue)) continue;

                    world[x, y].BlockColor = WeightedRandom<Color>.GetRandomValueFromArray(random, biome.Colors);
                    world[x, y].BiomeName = biome.name;
                }
            }
        }

        /// <summary>
        /// Generates prefabs on appropriate hexagons specific to the selected biome.
        /// </summary>
        /// <param name="biome">Selected biome.</param>
        /// <param name="world">Storage location of the generated map.</param>
        /// <param name="random">Selected random number generator.</param>
        private static void GenerateTilesPrefabs(BiomeData biome, Block[,] world, Random random)
        {
            int width = world.GetLength(0);
            int height = world.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!world[x, y].IsSolid) continue;
                    if (!biome.LandThreshold.IsValueInRange(world[x, y].LandValue)) continue;
                    if (!biome.BiomeThreshold.IsValueInRange(world[x, y].BiomeValue)) continue;

                    if (biome.ResourceVariants.Length <= 0) continue;

                    ResourceData resource = WeightedRandom<ResourceData>.GetRandomValueFromArray(random, biome.ResourceVariants);
                    world[x, y].ResourceType = resource.ResourceType;

                    if (resource.TileVariants.Length <= 0) continue;
                    if (world[x, y].Prefab != null) continue;

                    GameObject variant = WeightedRandom<GameObject>.GetRandomValueFromArray(random, resource.TileVariants);
                    world[x, y].Prefab = MonoBehaviour.Instantiate(variant, MathfExtend.SegmentToPosition(x, y), Quaternion.identity);
                    world[x, y].Prefab.name = $"Resource_{x}_{y}";
                }
            }
        }
    }
}