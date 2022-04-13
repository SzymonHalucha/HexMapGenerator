using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;
using Random = HexMapGenerator.Utils.Random;

namespace HexMapGenerator.Generation
{
    /// <summary>
    /// This class stores the information needed to generate a complete map.
    /// </summary>
    [CreateAssetMenu(menuName = "Hex/Generator Data", fileName = "New Generation Data", order = 0)]
    public class GeneratorData : ScriptableObject
    {
        public int Seed = 0;
        public Vector2Int Size = new Vector2Int(64, 64);
        public GradientNoiseData LandNoiseData;
        public GradientNoiseData BiomesNoiseData;
        public float LandMaskRadius;
        public List<BiomeData> Biomes;

        public Random WorldRandom;

        /// <summary>
        /// Initializes a random number generator with the selected seed.
        /// </summary>
        public void Initialize()
        {
            WorldRandom = new Random(Seed);
        }

        /// <summary>
        /// Initializes a random number generator with a new seed.
        /// </summary>
        /// <param name="seed">New seed.</param>
        public void Initialize(int seed)
        {
            Seed = seed;
            WorldRandom = new Random(Seed);
        }
    }
}