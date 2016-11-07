using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

	public int width = 6;
	public int height = 6;

	public Color defaultColor = Color.white;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	public Texture2D noiseSource;

	HexCell[] cells;

	Canvas gridCanvas;
	HexMesh hexMesh;

	void Awake () {
        /*if(File.Exists(Application.dataPath + "/hexGrid.dat") && new FileInfo(Application.dataPath + "/hexGrid.dat").Length != 0 
            && File.Exists(Application.dataPath + "/hexCells.dat") && new FileInfo(Application.dataPath + "/hexCells.dat").Length != 0)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/hexGrid.dat", FileMode.Open);
            SerializeGridData data = (SerializeGridData)bf.Deserialize(file);
            file.Close();

            width = data.width;
            height = data.height;

            cellPrefab.coordinates = data.cellPrefabData.coordinates;
            cellPrefab.Elevation = data.cellPrefabData.elevation;

            cells = new HexCell[width * height];

            FileStream file2 = File.Open(Application.dataPath + "/hexCells.dat", FileMode.Open);

            List<HexCellSerializeData> cellsData = (List<HexCellSerializeData>)bf.Deserialize(file2);
            file2.Close();

            HexMetrics.noiseSource = noiseSource;

            gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();

            

            for (int z = 0, i = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    CreateCell(cellsData[i].coordinates.X, cellsData[i].coordinates.Z, i++);
                }
            }
            for (int i = 0; i < width * height; i++)
            {
                cells[i].coordinates = cellsData[i].coordinates;
                cells[i].Elevation = cellsData[i].elevation;
            }
        }
        else
        { */
            HexMetrics.noiseSource = noiseSource;

            gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();

            cells = new HexCell[height * width];

            for (int z = 0, i = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
       // }
		
	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/hexGrid.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        SerializeGridData data = new SerializeGridData();

        data.width = width;
        data.height = height;

        data.cellPrefabData = new HexCellSerializeData();

        //data.cellPrefabData.color = cellPrefab.color;
        data.cellPrefabData.coordinates = cellPrefab.coordinates;
        data.cellPrefabData.elevation = cellPrefab.Elevation;
       // data.cellPrefabData.uiRect = cellPrefab.uiRect;
        
        //data.gridCanvas = gridCanvas;
        //data.hexMesh = hexMesh;

        bf.Serialize(file, data);
        file.Close();

        List<HexCellSerializeData> list = new List<HexCellSerializeData>();
        HexCellSerializeData cellData = new HexCellSerializeData();
        //cellData.color = cells[0].color;
        cellData.coordinates = cells[0].coordinates;
        //cellData.uiRect = cells[0].uiRect;
        cellData.elevation = cells[0].Elevation;

        list.Add(cellData);

        for (int i = 1; i < height * width; i++)
        {
            HexCellSerializeData cellData2 = new HexCellSerializeData();
            //cellData2.color = cells[i].color;
            cellData2.coordinates = cells[i].coordinates;
            //cellData2.uiRect = cells[i].uiRect;
            cellData2.elevation = cells[i].Elevation;
            list.Add(cellData2);
        }
        
        FileStream file2 = File.Open(Application.dataPath + "/hexCells.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        bf.Serialize(file2, list);
        file2.Close();
    }

    void Start () {
		hexMesh.Triangulate(cells);
	}

	void OnEnable () {
		HexMetrics.noiseSource = noiseSource;
	}

	public HexCell GetCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		return cells[index];
	}

	public void Refresh () {
		hexMesh.Triangulate(cells);
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - width]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - width]);
				if (x < width - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
				}
			}
		}

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
		cell.uiRect = label.rectTransform;
	}
}
[Serializable]
public class SerializeGridData
{
    public int width;
    public int height;

   // public Text cellLabelPrefab;

   // public Texture2D noiseSource;

    //public Canvas gridCanvas;
    //public HexMesh hexMesh;

    public HexCellSerializeData cellPrefabData;
}
