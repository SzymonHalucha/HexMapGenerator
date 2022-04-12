using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapGenerator.Generation
{
    public class Block
    {
        public float LandValue { get; set; }
        public float BiomeValue { get; set; }
        public bool IsSolid { get; set; }
        public Color BlockColor { get; set; }
        public ResourceData.Type ResourceType { get; set; }
        public string BiomeName { get; set; }
        public GameObject Prefab { get; set; }

        public Block(bool isSolid, float landValue)
        {
            this.IsSolid = isSolid;
            this.LandValue = landValue;
            this.BiomeValue = 0;
            this.BlockColor = Color.black;
            this.ResourceType = ResourceData.Type.None;
            this.BiomeName = "None";
            this.Prefab = null;
        }
    }
}