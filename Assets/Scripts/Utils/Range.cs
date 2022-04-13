using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapGenerator.Utils
{
    /// <summary>
    /// This class allows you to store a numeric range.
    /// </summary>
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

        /// <summary>
        /// Checks if the value is in the range. Min and Max inclusive.
        /// </summary>
        /// <param name="value">Check value.</param>
        /// <returns>Returns true if in range, returns false if not.</returns>
        public bool IsValueInRange(float value)
        {
            value = (float)Math.Round(value, 2);
            if (value >= Min && value <= Max) return true;
            else return false;
        }

        /// <summary>
        /// Sets a new numeric range.
        /// </summary>
        /// <param name="min">Lower range limit.</param>
        /// <param name="max">Upper range limit.</param>
        public void Set(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}