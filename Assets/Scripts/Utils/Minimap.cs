using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Generation;

namespace HexMapGenerator.Utils
{
    public class Minimap : MonoBehaviour
    {
        public static Minimap Instance { get; private set; }

        private MeshRenderer _renderer;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            _renderer = this.transform.GetComponent<MeshRenderer>();
        }

        public void UpdateMinimap(Block[,] world, bool showBiomes = false)
        {
            int width = world.GetLength(0);
            int height = world.GetLength(1);
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, 0, true);
            texture.filterMode = FilterMode.Point;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (showBiomes) texture.SetPixel(x, y, world[x, y].BlockColor);
                    else texture.SetPixel(x, y, Color.Lerp(Color.black, Color.white, world[x, y].LandValue));

                    if (showBiomes && world[x, y].ResourceType != ResourceData.Type.None) texture.SetPixel(x, y, Color.blue);
                }
            }

            texture.Apply();
            _renderer.material.mainTexture = texture;
        }
    }
}