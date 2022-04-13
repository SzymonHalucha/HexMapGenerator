using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;
using Range = HexMapGenerator.Utils.Range;

namespace HexMapGenerator.Generation
{
    /// <summary>
    /// This class allows you to create new biomes from within the unity editor.
    /// </summary>
    [CreateAssetMenu(menuName = "Hex/Biome Data", fileName = "New Biome Data", order = 0)]
    public class BiomeData : ScriptableObject
    {
        public Range LandThreshold;
        public Range BiomeThreshold;
        public WeightedRandom<Color>[] Colors;
        public WeightedRandom<ResourceData>[] ResourceVariants;
    }
}