using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using HexMapGenerator.Generation;
using HexMapGenerator.Utils;

namespace HexMapGenerator.UI
{
    public class HighlightOnPointer : MonoBehaviour
    {
        public string MapMaskName = "Map";

        private LayerMask _mask;
        private Map _map;
        private TileInfo _tileInfo;
        private bool _isPointerOnUI;

        private void Awake()
        {
            _mask = LayerMask.NameToLayer(MapMaskName);
            _map = FindObjectOfType<Map>();
            _tileInfo = FindObjectOfType<TileInfo>();
        }

        private void Update()
        {
            _isPointerOnUI = EventSystem.current.IsPointerOverGameObject();
            SetHighlightOnCurrentSegment();
        }

        private void SetHighlightOnCurrentSegment()
        {
            Vector3 mousePosition = MathfExtend.GetMouseWorldPosition(~_mask);
            Vector2Int segment = MathfExtend.PositionToSegment(mousePosition);
            Vector3 position = MathfExtend.SegmentToPosition(segment.x, segment.y);
            position.y += 0.05f;
            this.transform.position = position;
        }

        private void OnClicked()
        {
            if (_isPointerOnUI) return;

            Vector3 mousePosition = MathfExtend.GetMouseWorldPosition(~_mask);
            Vector2Int segment = MathfExtend.PositionToSegment(mousePosition);

            if (_map.World == null || !_map.World[segment.x, segment.y].IsSolid) return;

            string biome = _map.World[segment.x, segment.y].BiomeName;
            string resource = _map.World[segment.x, segment.y].ResourceType.ToString();
            _tileInfo.UpdateTileInfo(segment.x, segment.y, biome, resource);
        }
    }
}