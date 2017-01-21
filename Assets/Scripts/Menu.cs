using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class Menu : MonoBehaviour
    {//stworzyc chunk i dodac do niego cell, bo pewnie przez to przy odswiezaniu (triangulate albo cos takiego) nic sie nie rysuje
        HexCell[] cells;

        public HexCell cellPrefab;

        public Text cellLabelPrefab;

        public Texture2D noiseSource;

        public HexGridChunk chunkPrefab;

        HexGridChunk chunk;

        void Start()
        {
            HexMetrics.noiseSource = noiseSource;
            cells = new HexCell[1];
            CreateCell(0, 0, 0);
        }

        void CreateChunk()
        {
            chunk = Instantiate(chunkPrefab);
            chunk.transform.SetParent(transform);
        }

        void CreateCell(int x, int z, int i)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
            position.y = 0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);

            HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
            cell.Color = Color.yellow;
            Text label = Instantiate(cellLabelPrefab);
            label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
            cell.uiRect = label.rectTransform;
            cell.Elevation = 2;

            AddCellToChunk(x, z, cell);
        }

        void AddCellToChunk(int x, int z, HexCell cell)
        {
            int chunkX = x / HexMetrics.chunkSizeX;
            int chunkZ = z / HexMetrics.chunkSizeZ;

            int localX = x - chunkX * HexMetrics.chunkSizeX;
            int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
            chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
        }
    }
}
