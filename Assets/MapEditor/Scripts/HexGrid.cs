﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    public bool isGame;

	public int chunkCountX = 4, chunkCountZ = 3;

	public Color defaultColor = Color.white;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;
	public HexGridChunk chunkPrefab;

	public Texture2D noiseSource;

	HexGridChunk[] chunks;
	HexCell[] cells;

	int cellCountX, cellCountZ;

	void Awake ()
    {
		HexMetrics.noiseSource = noiseSource;

		cellCountX = chunkCountX * HexMetrics.chunkSizeX;
		cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

		CreateChunks();
		CreateCells();

        if(File.Exists(Application.dataPath + "/savedCells.gd"))
        {
            List<HexCellInfo> cellsInfo = new List<HexCellInfo>();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/savedCells.gd", FileMode.Open);
            cellsInfo = (List<HexCellInfo>)bf.Deserialize(file);
            file.Close();

            for(int i = 0; i < cellCountZ * cellCountX; i++)
            {
                cells[i].color.r = cellsInfo[i]._myColor[0];
                cells[i].color.g = cellsInfo[i]._myColor[1];
                cells[i].color.b = cellsInfo[i]._myColor[2];
                cells[i].color.a = cellsInfo[i]._myColor[3];
                cells[i].Elevation = cellsInfo[i].elevation;
                cells[i].hasIncomingRiver = cellsInfo[i].hasIncomingRiver;
                cells[i].hasOutgoingRiver = cellsInfo[i].hasOutgoingRiver;
                cells[i].incomingRiver = cellsInfo[i].incomingRiver;
                cells[i].outgoingRiver = cellsInfo[i].outgoingRiver;
            }
            
        }
        
	}

    public void SaveCells()
    {
        List<HexCellInfo> cellsInfo = new List<HexCellInfo>();
        for(int i = 0; i < cellCountZ * cellCountX; i++)
        {
            cells[i].SaveInfo();
            cellsInfo.Add(cells[i].info);
        }
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(Application.dataPath + "/savedCells.gd"))
        {
            File.Delete(Application.dataPath + "/savedCells.gd");
        }
        FileStream file = File.Create(Application.dataPath + "/savedCells.gd");
        bf.Serialize(file, cellsInfo);
        file.Close();
    }

	void CreateChunks ()
    {
		chunks = new HexGridChunk[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
			for (int x = 0; x < chunkCountX; x++)
            {
				HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
			}
		}
	}

	void CreateCells ()
    {
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++)
        {
			for (int x = 0; x < cellCountX; x++)
            {
				CreateCell(x, z, i++);
			}
		}
	}

	void OnEnable ()
    {
		HexMetrics.noiseSource = noiseSource;
	}

	public HexCell GetCell (Vector3 position)
    {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
		return cells[index];
	}

	public HexCell GetCell (HexCoordinates coordinates)
    {
		int z = coordinates.Z;
		if (z < 0 || z >= cellCountZ)
        {
			return null;
		}
		int x = coordinates.X + z / 2;
		if (x < 0 || x >= cellCountX)
        {
			return null;
		}
		return cells[x + z * cellCountX];
	}

	public void ShowUI (bool visible)
    {
		for (int i = 0; i < chunks.Length; i++)
        {
			chunks[i].ShowUI(visible);
		}
	}

	void CreateCell (int x, int z, int i)
    {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.Color = defaultColor;

		if (x > 0)
        {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0)
        {
			if ((z & 1) == 0)
            {
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if (x > 0)
                {
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}
			}
			else
            {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1)
                {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}
			}
		}

        Text label = Instantiate(cellLabelPrefab);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        if (!isGame)
        {
            label.text = cell.coordinates.ToStringOnSeparateLines();
        }
        cell.uiRect = label.rectTransform;
        cell.Elevation = 0;

		AddCellToChunk(x, z, cell);
	}

	void AddCellToChunk (int x, int z, HexCell cell)
    {
		int chunkX = x / HexMetrics.chunkSizeX;
		int chunkZ = z / HexMetrics.chunkSizeZ;
		HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * HexMetrics.chunkSizeX;
		int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
		chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
	}
}