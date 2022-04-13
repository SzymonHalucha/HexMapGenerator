using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace HexMapGenerator.Interactive
{
    /// <summary>
    /// This class is responsible for camera movement.
    /// </summary>
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

        /// <summary>
        /// Sets camera rotation, position, and zoom to initial values.
        /// </summary>
        private void Initialize()
        {
            targetPosition = Data.StartPosition;
            pivot.position = targetPosition;

            targetRoation = Quaternion.Euler(Data.StartRotation);
            pivot.rotation = targetRoation;

            targetZoom = Data.StartZoom;
            cameraPivot.localPosition = targetZoom;
        }

        /// <summary>
        /// Responsible for movement the camera parent pivot.
        /// </summary>
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

        /// <summary>
        /// Responsible for rotating the camera parent pivot.
        /// </summary>
        private void RotateHandler()
        {
            if (rotate < 0) targetRoation *= Quaternion.Euler(Vector3.up * Data.RotationSpeed * Time.deltaTime);
            else if (rotate > 0) targetRoation *= Quaternion.Euler(-Vector3.up * Data.RotationSpeed * Time.deltaTime);

            pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRoation, Data.RotationSmoothness * Time.deltaTime);
        }

        /// <summary>
        /// Handles camera position directly, allowing you to zoom in and out (Camera.localPosition).
        /// </summary>
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

        /// <summary>
        /// Reads WSAD values from the New input System.
        /// </summary>
        /// <param name="value">The vector value of the WSAD keys from New Input System.</param>
        private void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        /// <summary>
        /// Reads the acceleration (Shift) button and changes the speed at which the camera moves.
        /// </summary>
        /// <param name="value">Boolean value from New Input System.</param>
        private void OnMultiplier(InputValue value)
        {
            if (value.isPressed) Data.MoveSpeed *= Data.ShiftMultiplier;
            else Data.MoveSpeed /= Data.ShiftMultiplier;
        }

        /// <summary>
        /// Reads the axial value of QE buttons between -1 and 1.
        /// </summary>
        /// <param name="value">Axial value of QE buttons from New Input System.</param>
        private void OnRotate(InputValue value)
        {
            rotate = value.Get<float>();
        }

        /// <summary>
        /// Reads the vector value of the mouse wheel (y property).
        /// </summary>
        /// <param name="value">Mouse wheel vector value (y property) from New Input System</param>
        private void OnZoom(InputValue value)
        {
            zoom = value.Get<Vector2>().y;
        }
    }
}