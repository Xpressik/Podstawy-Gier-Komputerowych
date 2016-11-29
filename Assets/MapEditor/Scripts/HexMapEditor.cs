using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;

	int activeElevation;

	Color activeColor;

	int brushSize;

	bool applyColor;
	bool applyElevation = true;

    //  Editor Window

    //public Canvas fieldEditor;
    //public Button player1Button;
    //public Button player2Button;
    //public InputField supplyInput;
    //public InputField contrabandInput;
    //public Toggle campToggle;
    //public Button quitButton;

	enum OptionalToggle
    {
		Ignore, Yes, No
	}

	OptionalToggle riverMode, walledMode;

	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;

    int activeUrbanLevel, activeFarmLevel, activePlantLevel;
    bool applyUrbanLevel, applyFarmLevel, applyPlantLevel;

    public void SelectColor (int index)
    {
		applyColor = index >= 0;
		if (applyColor)
        {
			activeColor = colors[index];
		}
	}

	public void SetApplyElevation (bool toggle)
    {
		applyElevation = toggle;
	}

	public void SetElevation (float elevation)
    {
		activeElevation = (int)elevation;
	}

	public void SetBrushSize (float size)
    {
		brushSize = (int)size;
	}

	public void SetRiverMode (int mode)
    {
		riverMode = (OptionalToggle)mode;
	}

	public void ShowUI (bool visible)
    {
		hexGrid.ShowUI(visible);
	}

    public void SetApplyUrbanLevel(bool toggle)
    {
        applyUrbanLevel = toggle;
    }

    public void SetUrbanLevel(float level)
    {
        activeUrbanLevel = (int)level;
    }

    public void SetApplyFarmLevel(bool toggle)
    {
        applyFarmLevel = toggle;
    }

    public void SetFarmLevel(float level)
    {
        activeFarmLevel = (int)level;
    }

    public void SetApplyPlantLevel(bool toggle)
    {
        applyPlantLevel = toggle;
    }

    public void SetPlantLevel(float level)
    {
        activePlantLevel = (int)level;
    }

    public void SetWalledMode(int mode)
    {
        walledMode = (OptionalToggle)mode;
    }

    void Awake ()
    {
		SelectColor(0);
	}

    void Start()
    {
        //fieldEditor = fieldEditor.GetComponent<Canvas>();
        //player1Button = player1Button.GetComponent<Button>();
        //player2Button = player2Button.GetComponent<Button>();
        //supplyInput = supplyInput.GetComponent<InputField>();
        //contrabandInput = contrabandInput.GetComponent<InputField>();
        //campToggle = campToggle.GetComponent<Toggle>();
        //quitButton = quitButton.GetComponent<Button>();
        //fieldEditor.enabled = false;
}

	void Update ()
    {
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
			HandleInput();
		}
		else
        {
			previousCell = null;
		}
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput2();
        }
	}

	void HandleInput ()
    {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
        {
			HexCell currentCell = hexGrid.GetCell(hit.point);
			if (previousCell && previousCell != currentCell)
            {
				ValidateDrag(currentCell);
			}
			else
            {
				isDrag = false;
			}
			EditCells(currentCell);
			previousCell = currentCell;
		}
		else
        {
			previousCell = null;
		}
	}

    void HandleInput2()
    {

    }

	void ValidateDrag (HexCell currentCell)
    {
		for (dragDirection = HexDirection.NE; dragDirection <= HexDirection.NW; dragDirection++)
        {
			if (previousCell.GetNeighbor(dragDirection) == currentCell)
            {
				isDrag = true;
				return;
			}
		}
		isDrag = false;
	}

	void EditCells (HexCell center)
    {
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
        {
			for (int x = centerX - r; x <= centerX + brushSize; x++)
            {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
		for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
        {
			for (int x = centerX - brushSize; x <= centerX + r; x++)
            {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
	}

    void EditCell (HexCell cell)
    {
		if (cell)
        {
			if (applyColor)
            {
				cell.Color = activeColor;
			}
			if (applyElevation)
            {
				cell.Elevation = activeElevation;
			}
            if (applyUrbanLevel)
            {
                cell.UrbanLevel = activeUrbanLevel;
            }
            if (applyFarmLevel)
            {
                cell.FarmLevel = activeFarmLevel;
            }
            if (applyPlantLevel)
            {
                cell.PlantLevel = activePlantLevel;
            }
            if (riverMode == OptionalToggle.No)
            {
				cell.RemoveRiver();
			}
            if (walledMode != OptionalToggle.Ignore)
            {
                cell.Walled = walledMode == OptionalToggle.Yes;
            }
            else if (isDrag && riverMode == OptionalToggle.Yes)
            {
				HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
				if (otherCell)
                {
					otherCell.SetOutgoingRiver(dragDirection);
				}
			}
		}
	}
    //public void QuitFieldEditor()
    //{
    //    fieldEditor.enabled = false;
    //}
}