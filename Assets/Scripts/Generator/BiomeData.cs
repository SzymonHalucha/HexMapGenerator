using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;
using Range = HexMapGenerator.Utils.Range;

namespace HexMapGenerator.Generation
{
    [CreateAssetMenu(menuName = "Hex/Biome Data", fileName = "New Biome Data", order = 0)]
    public class BiomeData : ScriptableObject
    {
        public Range LandThreshold;
        public Range BiomeThreshold;
        public WeightedRandom<Color>[] Colors;
        public WeightedRandom<ResourceData>[] ResourceVariants;
    }
}