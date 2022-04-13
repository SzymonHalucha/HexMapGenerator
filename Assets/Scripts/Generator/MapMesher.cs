using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;

namespace HexMapGenerator.Generation
{
    /// <summary>
    /// This class is responsible for generating the map mesh.
    /// </summary>
    public class MapMesher
    {
        private enum Direction { NorthWest, NorthEast, East, SouthEast, SouthWest, West }

        private const float _outer = 1f;
        private const float _inner = _outer * 0.866025404f;

        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _triangles = new List<int>();
        private List<Color> _colors = new List<Color>();

        /// <summary>
        /// Generates a mesh from the information contained in the array.
        /// </summary>
        /// <param name="world">An array with information about the generated map.</param>
        /// <returns>Returns the mesh of the generated map.</returns>
        public Mesh GenerateMesh(Block[,] world)
        {
            int width = world.GetLength(0);
            int height = world.GetLength(1);
            _vertices.Clear();
            _triangles.Clear();
            _colors.Clear();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!world[x, y].IsSolid) continue;

                    AddHexagonTop(x, y, world);
                    AddHexagonBorders(x, y, world);
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.colors = _colors.ToArray();
            mesh.RecalculateNormals();
            mesh.Optimize();
            return mesh;
        }

        /// <summary>
        /// Checks and adds (if necessary) the side walls of the hexagon.
        /// </summary>
        /// <param name="x">Position/segment X of hexagon.</param>
        /// <param name="y">Position/segment Y of hexagon.</param>
        /// <param name="world">An array with information about the generated map.</param>
        private void AddHexagonBorders(int x, int y, Block[,] world)
        {
            if (!world[x, y].IsSolid) return;
            float height = 0.5f;

            if (!CheckNeighbor(x, y, world, Direction.SouthWest)) AddBorderVertices(x, y, height, world, Direction.SouthWest);
            if (!CheckNeighbor(x, y, world, Direction.West)) AddBorderVertices(x, y, height, world, Direction.West);
            if (!CheckNeighbor(x, y, world, Direction.NorthWest)) AddBorderVertices(x, y, height, world, Direction.NorthWest);
            if (!CheckNeighbor(x, y, world, Direction.NorthEast)) AddBorderVertices(x, y, height, world, Direction.NorthEast);
            if (!CheckNeighbor(x, y, world, Direction.East)) AddBorderVertices(x, y, height, world, Direction.East);
            if (!CheckNeighbor(x, y, world, Direction.SouthEast)) AddBorderVertices(x, y, height, world, Direction.SouthEast);
        }

        /// <summary>
        /// Checks if there is a neighbor of the selected hexagon.
        /// </summary>
        /// <param name="x">The position/segment X of the selected hexagon.</param>
        /// <param name="y">The position/segment Y of the selected hexagon.</param>
        /// <param name="world">An array with information about the generated map.</param>
        /// <param name="direction">Check direction.</param>
        /// <returns>If the neighbor exists it returns true, if not it returns false.</returns>
        private bool CheckNeighbor(int x, int y, Block[,] world, Direction direction)
        {
            int width = world.GetLength(0);
            int height = world.GetLength(1);
            int currentX = x;
            int currentY = y;

            switch (direction)
            {
                case Direction.SouthWest:
                    currentY = y - 1;
                    if (currentY % 2 == 0) currentX = x;
                    else currentX = x - 1;
                    break;

                case Direction.West:
                    currentX = x - 1;
                    currentY = y;
                    break;

                case Direction.NorthWest:
                    currentY = y + 1;
                    if (currentY % 2 == 0) currentX = x;
                    else currentX = x - 1;
                    break;

                case Direction.NorthEast:
                    currentY = y + 1;
                    if (currentY % 2 == 0) currentX = x + 1;
                    else currentX = x;
                    break;

                case Direction.East:
                    currentX = x + 1;
                    currentY = y;
                    break;

                case Direction.SouthEast:
                    currentY = y - 1;
                    if (currentY % 2 == 0) currentX = x + 1;
                    else currentX = x;
                    break;
            }

            if (currentX < 0 || currentY < 0 || currentX >= width || currentY >= height) return false;
            else return world[currentX, currentY].IsSolid;
        }

        /// <summary>
        /// Adds the vertices of the sidewall of the selected hexagon.
        /// </summary>
        /// <param name="x">The position/segment X of the selected hexagon.</param>
        /// <param name="y">The position/segment Y of the selected hexagon.</param>
        /// <param name="height">Sidewall height.</param>
        /// <param name="world">An array with information about the generated map.</param>
        /// <param name="direction">Check direction.</param>
        private void AddBorderVertices(int x, int y, float height, Block[,] world, Direction direction)
        {
            Vector3 position = MathfExtend.SegmentToPosition(x, y, _outer);

            switch (direction)
            {
                case Direction.SouthWest:
                    _vertices.Add(position + new Vector3(0, 0, -_outer));
                    _vertices.Add(position + new Vector3(-_inner, 0, -_outer * 0.5f));
                    _vertices.Add(position + new Vector3(0, -height, -_outer));
                    _vertices.Add(position + new Vector3(-_inner, -height, -_outer * 0.5f));
                    break;

                case Direction.West:
                    _vertices.Add(position + new Vector3(-_inner, 0, -_outer * 0.5f));
                    _vertices.Add(position + new Vector3(-_inner, 0, _outer * 0.5f));
                    _vertices.Add(position + new Vector3(-_inner, -height, -_outer * 0.5f));
                    _vertices.Add(position + new Vector3(-_inner, -height, _outer * 0.5f));
                    break;

                case Direction.NorthWest:
                    _vertices.Add(position + new Vector3(-_inner, 0, _outer * 0.5f));
                    _vertices.Add(position + new Vector3(0, 0, _outer));
                    _vertices.Add(position + new Vector3(-_inner, -height, _outer * 0.5f));
                    _vertices.Add(position + new Vector3(0, -height, _outer));
                    break;

                case Direction.NorthEast:
                    _vertices.Add(position + new Vector3(0, 0, _outer));
                    _vertices.Add(position + new Vector3(_inner, 0, _outer * 0.5f));
                    _vertices.Add(position + new Vector3(0, -height, _outer));
                    _vertices.Add(position + new Vector3(_inner, -height, _outer * 0.5f));
                    break;

                case Direction.East:
                    _vertices.Add(position + new Vector3(_inner, 0, _outer * 0.5f));
                    _vertices.Add(position + new Vector3(_inner, 0, -_outer * 0.5f));
                    _vertices.Add(position + new Vector3(_inner, -height, _outer * 0.5f));
                    _vertices.Add(position + new Vector3(_inner, -height, -_outer * 0.5f));
                    break;

                case Direction.SouthEast:
                    _vertices.Add(position + new Vector3(_inner, 0, -_outer * 0.5f));
                    _vertices.Add(position + new Vector3(0, 0, -_outer));
                    _vertices.Add(position + new Vector3(_inner, -height, -_outer * 0.5f));
                    _vertices.Add(position + new Vector3(0, -height, -_outer));
                    break;
            }

            AddBorderTriangles();
            AddBorderColors(world[x, y].BlockColor);
        }

        /// <summary>
        /// Sets the correct vertices order of the sidewall.
        /// </summary>
        private void AddBorderTriangles()
        {
            int current = _vertices.Count - 4;

            _triangles.Add(current + 2);
            _triangles.Add(current + 1);
            _triangles.Add(current);

            _triangles.Add(current + 3);
            _triangles.Add(current + 1);
            _triangles.Add(current + 2);
        }

        /// <summary>
        /// Adds color to the sidewall.
        /// </summary>
        /// <param name="color">Selected color.</param>
        private void AddBorderColors(Color color)
        {
            _colors.Add(color);
            _colors.Add(color);
            _colors.Add(color);
            _colors.Add(color);
        }

        /// <summary>
        /// Responsible for creating the mesh of the upper part of the hexagon.
        /// </summary>
        /// <param name="x">The position/segment X of the selected hexagon.</param>
        /// <param name="y">The position/segment Y of the selected hexagon.</param>
        /// <param name="world">An array with information about the generated map.</param>
        private void AddHexagonTop(int x, int y, Block[,] world)
        {
            Vector3 position = new Vector3((x + y * 0.5f - (int)(y / 2f)) * _inner * 2f, 0, y * _outer * 1.5f);
            AddHexagonVertices(position);
            AddHexagonTriangles();
            AddHexagonColors(world[x, y].BlockColor);
        }

        /// <summary>
        /// Responsible for adding the top vertices of the hexagon at the corresponding position.
        /// </summary>
        /// <param name="position">Selected 3D position.</param>
        private void AddHexagonVertices(Vector3 position)
        {
            _vertices.Add(position + new Vector3(0, 0, -_outer));
            _vertices.Add(position + new Vector3(-_inner, 0, -_outer * 0.5f));
            _vertices.Add(position + new Vector3(-_inner, 0, _outer * 0.5f));
            _vertices.Add(position + new Vector3(0, 0, _outer));
            _vertices.Add(position + new Vector3(_inner, 0, _outer * 0.5f));
            _vertices.Add(position + new Vector3(_inner, 0, -_outer * 0.5f));
        }

        /// <summary>
        /// Sets the correct order of the top vertices of a hexagon.
        /// </summary>
        private void AddHexagonTriangles()
        {
            int current = _vertices.Count - 6;

            for (int i = 2; i < 6; i++)
            {
                _triangles.Add(current);
                if (i <= 5) _triangles.Add(current + i - 1);
                _triangles.Add(current + i);
                if (i == 6) _triangles.Add(current + i + 1);
            }
        }

        /// <summary>
        /// Adds color to the top of the hexagon.
        /// </summary>
        /// <param name="color">Selected color.</param>
        private void AddHexagonColors(Color color)
        {
            _colors.Add(color);
            _colors.Add(color);
            _colors.Add(color);

            _colors.Add(color);
            _colors.Add(color);
            _colors.Add(color);
        }
    }
}