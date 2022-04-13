using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapGenerator.Utils
{
    /// <summary>
    /// This class stores information about the characteristics of the gradient noise.
    /// </summary>
    [CreateAssetMenu(menuName = "Hex/Gradient Noise Data", fileName = "New Gradient Noise Data", order = 1)]
    public class GradientNoiseData : ScriptableObject
    {
        [Range(0, 16f)] public int Octaves = 6;
        [Range(0, 128f)] public float Scale = 40f;
        [Range(0, 8f)] public float Lacunarity = 1.25f;
        [Range(0, 1f)] public float Persistance = 0.75f;
    }
}