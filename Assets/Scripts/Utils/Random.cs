using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapGenerator.Utils
{
    public class Random
    {
        private System.Random _random;

        public Random() => this._random = new System.Random(0);
        public Random(int seed) => this._random = new System.Random(seed);

        public int Range(int max) => _random.Next(max);
        public int Range(int min, int max) => _random.Next(min, max);
        public float Range(float max) => (float)(_random.NextDouble() * max);
        public float Range(float min, float max) => (float)(min + _random.NextDouble() * max);
        public float Value() => (float)_random.NextDouble();
        public void Seed(int seed) => _random = new System.Random(seed);
    }
}