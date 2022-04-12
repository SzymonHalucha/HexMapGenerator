using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapGenerator.Utils
{
    [Serializable]
    public class Range
    {
        public float Min;
        public float Max;

        public Range()
        {
            this.Min = 0;
            this.Max = 0;
        }

        public Range(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }

        public bool IsValueInRange(float value)
        {
            value = (float)Math.Round(value, 2);
            if (value >= Min && value <= Max) return true;
            else return false;
        }

        public void Set(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}