using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;

namespace HexMapGenerator.Generation
{
    /// <summary>
    /// This class is responsible for storing information about the generated map and its mesh.
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class Map : MonoBehaviour
    {
        public static Map Instance { get; private set; }

        public Block[,] World;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;
        private MapMesher _mapMesher;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            _meshFilter = this.transform.GetComponent<MeshFilter>();
            _meshRenderer = this.transform.GetComponent<MeshRenderer>();
            _meshCollider = this.transform.GetComponent<MeshCollider>();
            _mapMesher = new MapMesher();
        }

        /// <summary>
        /// Generates a map with the selected settings.
        /// </summary>
        /// <param name="selectedData">Selected map settings.</param>
        public void GenerateMap(GeneratorData selectedData)
        {
            CleanMapArray(selectedData);
            selectedData.Initialize();

            Generator.GenerateLand(selectedData, World);
            Generator.GenerateBiomes(selectedData, World);
            _meshFilter.sharedMesh = _mapMesher.GenerateMesh(World);
            _meshCollider.sharedMesh = _meshFilter.sharedMesh;
        }

        /// <summary>
        /// Creates a new array to store information about the generated map and/or removes the remnants of the previous generation.
        /// </summary>
        /// <param name="selectedData">Selected map settings.</param>
        public void CleanMapArray(GeneratorData selectedData)
        {
            if (World == null)
            {
                World = new Block[selectedData.Size.x, selectedData.Size.y];
                return;
            }

            int width = World.GetLength(0);
            int height = World.GetLength(1);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (World[x, y].Prefab != null)
                        Destroy(World[x, y].Prefab);

            if (width != selectedData.Size.x || height != selectedData.Size.y)
                World = new Block[selectedData.Size.x, selectedData.Size.y];
        }
    }
}