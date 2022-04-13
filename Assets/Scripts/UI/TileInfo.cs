using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using HexMapGenerator.Generation;
using HexMapGenerator.Utils;

namespace HexMapGenerator.UI
{
    /// <summary>
    /// This class is responsible for displaying information about the hexagon using the UI.
    /// </summary>
    public class TileInfo : MonoBehaviour
    {
        public GameObject HighlightPrefab;
        public TMP_Text PositionText;
        public TMP_Text BiomeText;
        public TMP_Text ResourceText;

        private Canvas _canvas;
        private GameObject _prefab;

        private void Awake()
        {
            _canvas = this.transform.GetComponent<Canvas>();
            _prefab = Instantiate(HighlightPrefab, Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            this.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }

        /// <summary>
        /// Displays and sets the UI position with the specified values.
        /// </summary>
        /// <param name="x">The position/segment X of the selected hexagon.</param>
        /// <param name="y">The position/segment Y of the selected hexagon.</param>
        /// <param name="biome">Name of the biome to display.</param>
        /// <param name="resource">The name of the resource to display.</param>
        public void UpdateTileInfo(int x, int y, string biome, string resource)
        {
            Vector3 position = MathfExtend.SegmentToPosition(x, y);
            position.y += 3f;

            PositionText.text = $"Segment: {x}, {y}";
            BiomeText.text = $"Biome: {biome}";
            ResourceText.text = $"Resource: {resource}";

            _canvas.transform.position = position;
            _canvas.enabled = true;

            SetPrefabOnPosition();
            _prefab.gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets the hexagon highlight to the current cursor position.
        /// </summary>
        private void SetPrefabOnPosition()
        {
            Vector3 mousePosition = MathfExtend.GetMouseWorldPosition();
            Vector2Int segment = MathfExtend.PositionToSegment(mousePosition);
            Vector3 position = MathfExtend.SegmentToPosition(segment.x, segment.y);
            position.y += 0.05f;
            _prefab.transform.position = position;
        }

        /// <summary>
        /// Hides the UI and highlights the hexagon.
        /// </summary>
        public void OnExit()
        {
            _canvas.enabled = false;
            _prefab.gameObject.SetActive(false);
        }
    }
}