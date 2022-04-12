using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexMapGenerator.Utils;

namespace HexMapGenerator.Generation
{
    public class MapMesher
    {
        private enum Direction { NorthWest, NorthEast, East, SouthEast, SouthWest, West }

        private const float _outer = 1f;
        private const float _inner = _outer * 0.866025404f;

        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _triangles = new List<int>();
        private List<Color> _colors = new List<Color>();

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

        private void AddBorderColors(Color color)
        {
            _colors.Add(color);
            _colors.Add(color);
            _colors.Add(color);
            _colors.Add(color);
        }

        private void AddHexagonTop(int x, int y, Block[,] world)
        {
            Vector3 position = new Vector3((x + y * 0.5f - (int)(y / 2f)) * _inner * 2f, 0, y * _outer * 1.5f);
            AddHexagonVertices(position);
            AddHexagonTriangles();
            AddHexagonColors(world[x, y].BlockColor);
        }

        private void AddHexagonVertices(Vector3 position)
        {
            _vertices.Add(position + new Vector3(0, 0, -_outer));
            _vertices.Add(position + new Vector3(-_inner, 0, -_outer * 0.5f));
            _vertices.Add(position + new Vector3(-_inner, 0, _outer * 0.5f));
            _vertices.Add(position + new Vector3(0, 0, _outer));
            _vertices.Add(position + new Vector3(_inner, 0, _outer * 0.5f));
            _vertices.Add(position + new Vector3(_inner, 0, -_outer * 0.5f));
        }

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