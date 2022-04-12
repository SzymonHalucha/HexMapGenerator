using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;

namespace HexMapGenerator.Generation
{
    [CreateAssetMenu(menuName = "Hex/Resource Data", fileName = "New Resource Data", order = 0)]
    public class ResourceData : ScriptableObject
    {
        public enum Type { None, Wood, Stone, Berries }

        public Type ResourceType;
        public WeightedRandom<GameObject>[] TileVariants;
    }
}