using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HexMapGenerator.Utils
{
    /// <summary>
    /// This structure is an extension of the UnityEngine.Mathf structure.
    /// </summary>
    public struct MathfExtend
    {
        /// <summary>
        /// Maps a numeric value from one range to another.
        /// </summary>
        /// <param name="value">Selected value.</param>
        /// <param name="fromMin">Current lower range.</param>
        /// <param name="fromMax">Current upper range.</param>
        /// <param name="toMin">New lower range.</param>
        /// <param name="toMax">New upper range.</param>
        /// <returns>Returns the mapped value.</returns>
        public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            float inverseLerp = (value - fromMin) / (fromMax - fromMin);
            float lerp = (1f - inverseLerp) * toMin + toMax * inverseLerp;
            return lerp;
        }

        /// <summary>
        /// Converts the cursor position on the screen to a 3D position.
        /// </summary>
        /// <returns>Returns the 3D position of the cursor.</returns>
        public static Vector3 GetMouseWorldPosition()
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mouseScreen);

            if (Physics.Raycast(ray, out RaycastHit hit, 9999f)) return hit.point;
            else return Vector3.zero;
        }

        /// <summary>
        /// Converts the cursor position on the screen to a 3D position.
        /// </summary>
        /// <param name="selectedMask">The LayerMask to ignore when determining position.</param>
        /// <returns>Returns the 3D position of the cursor.</returns>
        public static Vector3 GetMouseWorldPosition(int selectedMask)
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mouseScreen);

            if (Physics.Raycast(ray, out RaycastHit hit, 9999f, selectedMask)) return hit.point;
            else return Vector3.zero;
        }

        /// <summary>
        /// Converts a position/segment to a 3D position.
        /// </summary>
        /// <param name="x">Selected position/segment X.</param>
        /// <param name="y">Selected position/segment Y.</param>
        /// <param name="hexagonSize">Size of a single hexagon. Default is 1.</param>
        /// <returns>Returns the 3D position.</returns>
        public static Vector3 SegmentToPosition(int x, int y, float hexagonSize = 1f)
        {
            float inner = hexagonSize * 0.866025404f;
            return new Vector3((x + y * 0.5f - (int)(y / 2f)) * inner * 2f, 0, y * hexagonSize * 1.5f);
        }

        /// <summary>
        /// Converts a 3D position to a position/segment.
        /// </summary>
        /// <param name="position">Selected 3D position.</param>
        /// <param name="hexagonSize">Size of a single hexagon. Default is 1.</param>
        /// <returns>Returns the position/segment.</returns>
        public static Vector2Int PositionToSegment(Vector3 position, float hexagonSize = 1f)
        {
            float inner = hexagonSize * 0.866025404f;
            int y = (int)(position.z / (hexagonSize * 1.5f));
            int x = (int)((int)(position.x / (inner * 2f)) + (y * 0.5f - (int)(y / 2f)));
            return new Vector2Int(x, y);
        }
    }
}