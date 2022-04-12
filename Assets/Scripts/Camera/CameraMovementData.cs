using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapGenerator.Interactive
{
    [CreateAssetMenu(menuName = "Hex/Camera Data", fileName = "New Camera Data", order = 0)]
    public class CameraMovementData : ScriptableObject
    {
        [Header("Movement")]
        public float MoveSpeed;
        public float MoveSmoothness;
        public float ShiftMultiplier;

        [Header("Rotation")]
        public float RotationSpeed;
        public float RotationSmoothness;

        [Header("Zoom")]
        public float ZoomSpeed;
        public float ZoomSmoothnees;
        public float ZoomZDivider;

        [Header("Starting Position")]
        public Vector3 StartPosition;
        public Vector3 StartRotation;
        public Vector3 StartZoom;

        [Header("Borders")]
        public Vector3 StartBorder;
        public Vector3 EndBorder;
    }
}