using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace HexMapGenerator.Interactive
{
    public class CameraMovement : MonoBehaviour
    {
        public CameraMovementData Data;

        private Vector2 move;
        private float rotate;
        private float zoom;

        private Transform pivot;
        private Transform cameraPivot;
        private Vector3 targetPosition;
        private Quaternion targetRoation;
        private Vector3 targetZoom;

        private void Awake()
        {
            pivot = this.transform;
            cameraPivot = this.transform.GetComponentInChildren<UnityEngine.Camera>().transform;
            Initialize();
        }

        private void Update()
        {
            MoveHandler();
            RotateHandler();
            ZoomHandler();
        }

        private void Initialize()
        {
            targetPosition = Data.StartPosition;
            pivot.position = targetPosition;

            targetRoation = Quaternion.Euler(Data.StartRotation);
            pivot.rotation = targetRoation;

            targetZoom = Data.StartZoom;
            cameraPivot.localPosition = targetZoom;
        }

        private void MoveHandler()
        {
            if (move.x > 0) targetPosition += (pivot.right.normalized * Data.MoveSpeed) * Time.deltaTime;
            else if (move.x < 0) targetPosition += (-pivot.right.normalized * Data.MoveSpeed) * Time.deltaTime;

            if (move.y > 0) targetPosition += (pivot.forward.normalized * Data.MoveSpeed) * Time.deltaTime;
            else if (move.y < 0) targetPosition += (-pivot.forward.normalized * Data.MoveSpeed) * Time.deltaTime;

            targetPosition.x = Mathf.Clamp(targetPosition.x, Data.StartBorder.x, Data.EndBorder.x);
            targetPosition.z = Mathf.Clamp(targetPosition.z, Data.StartBorder.z, Data.EndBorder.z);

            pivot.position = Vector3.Lerp(pivot.position, targetPosition, Data.MoveSmoothness * Time.deltaTime);
        }

        private void RotateHandler()
        {
            if (rotate < 0) targetRoation *= Quaternion.Euler(Vector3.up * Data.RotationSpeed * Time.deltaTime);
            else if (rotate > 0) targetRoation *= Quaternion.Euler(-Vector3.up * Data.RotationSpeed * Time.deltaTime);

            pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRoation, Data.RotationSmoothness * Time.deltaTime);
        }

        private void ZoomHandler()
        {
            if (zoom > 0) targetZoom += (new Vector3(0, -0.5f, 0.5f) * Data.ZoomSpeed) * Time.deltaTime;
            else if (zoom < 0) targetZoom += (new Vector3(0, 0.5f, -0.5f) * Data.ZoomSpeed) * Time.deltaTime;

            targetZoom.y = Mathf.Clamp(targetZoom.y, Data.StartBorder.y, Data.EndBorder.y);
            float value = Mathf.InverseLerp(Data.StartBorder.y, Data.EndBorder.y, targetZoom.y);
            targetZoom.z = -Mathf.Clamp(Data.StartBorder.y, Data.EndBorder.y / Data.ZoomZDivider, value);

            cameraPivot.localPosition = Vector3.Lerp(cameraPivot.localPosition, targetZoom, Data.ZoomSmoothnees * Time.deltaTime);
            cameraPivot.LookAt(pivot.transform);
        }

        private void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        private void OnMultiplier(InputValue value)
        {
            if (value.isPressed) Data.MoveSpeed *= Data.ShiftMultiplier;
            else Data.MoveSpeed /= Data.ShiftMultiplier;
        }

        private void OnRotate(InputValue value)
        {
            rotate = value.Get<float>();
        }

        private void OnZoom(InputValue value)
        {
            zoom = value.Get<Vector2>().y;
        }
    }
}