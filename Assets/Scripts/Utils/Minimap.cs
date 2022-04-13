using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Generation;

namespace HexMapGenerator.Utils
{
    /// <summary>
    /// This class displays a map as a texture on the object mesh (For debugging only).
    /// </summary>
    public class Minimap : MonoBehaviour
    {
        public static Minimap Instance { get; private set; }

        private MeshRenderer _renderer;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            _renderer = this.transform.GetComponent<MeshRenderer>();
        }

        /// <summary>
        /// Generates a texture representing the generated map and adds it to the object mesh.
        /// </summary>
        /// <param name="world">An array with the generated map.</param>
        /// <param name="showBiomes">Use colors to define biomes.</param>
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