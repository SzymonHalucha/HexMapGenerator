using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HexMapGenerator.Utils
{
    public struct MathfExtend
    {
        public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            float inverseLerp = (value - fromMin) / (fromMax - fromMin);
            float lerp = (1f - inverseLerp) * toMin + toMax * inverseLerp;
            return lerp;
        }

        public static Vector3 GetMouseWorldPosition()
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mouseScreen);

            if (Physics.Raycast(ray, out RaycastHit hit, 9999f)) return hit.point;
            else return Vector3.zero;
        }

        public static Vector3 GetMouseWorldPosition(int selectedMask)
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mouseScreen);

            if (Physics.Raycast(ray, out RaycastHit hit, 9999f, selectedMask)) return hit.point;
            else return Vector3.zero;
        }

        public static Vector3 SegmentToPosition(int x, int y, float hexagonSize = 1f)
        {
            float inner = hexagonSize * 0.866025404f;
            return new Vector3((x + y * 0.5f - (int)(y / 2f)) * inner * 2f, 0, y * hexagonSize * 1.5f);
        }

        public static Vector2Int PositionToSegment(Vector3 position, float hexagonSize = 1f)
        {
            float inner = hexagonSize * 0.866025404f;
            int y = (int)(position.z / (hexagonSize * 1.5f));
            int x = (int)((int)(position.x / (inner * 2f)) + (y * 0.5f - (int)(y / 2f)));
            return new Vector2Int(x, y);
        }
    }
}