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

        private void SetPrefabOnPosition()
        {
            Vector3 mousePosition = MathfExtend.GetMouseWorldPosition();
            Vector2Int segment = MathfExtend.PositionToSegment(mousePosition);
            Vector3 position = MathfExtend.SegmentToPosition(segment.x, segment.y);
            position.y += 0.05f;
            _prefab.transform.position = position;
        }

        public void OnExit()
        {
            _canvas.enabled = false;
            _prefab.gameObject.SetActive(false);
        }
    }
}