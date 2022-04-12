using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;
using Random = HexMapGenerator.Utils.Random;

namespace HexMapGenerator.Generation
{
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

        public void Initialize()
        {
            WorldRandom = new Random(Seed);
        }

        public void Initialize(int seed)
        {
            Seed = seed;
            WorldRandom = new Random(Seed);
        }
    }
}