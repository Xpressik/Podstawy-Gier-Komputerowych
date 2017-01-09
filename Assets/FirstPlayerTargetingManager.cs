using UnityEngine;
using Assets;
using ProgressBar;

public class FirstPlayerTargetingManager : MonoBehaviour
{
    private HexGrid hexGrid;

    private Figure selectedFigure;

    private Color selectedCellColor;

    private HexCell currentCell;

    public GameObject figure;

    private HexCell selectedCell;

    private PlayerOwnershipManager playerOwnershipManager;

    private Player player;

    private ProgressBarBehaviour firstPlayerTimerbar;

    // Use this for initialization
    void Start()
    {
        hexGrid = GetComponent<HexGrid>();
        currentCell = hexGrid.GetCell(new Vector3(155.8846f, 30f, 30.0f));
        currentCell.isWallCapsule = true;
        currentCell.Walled = true;
        GameObject go = (GameObject)Instantiate(figure, new Vector3(155.8846f, -0.4045f, 30.0f), Quaternion.Euler(0, 0, 0));
        go.transform.SetParent(transform);
        go.AddComponent<MeshRenderer>();
        selectedFigure = go.GetComponent<Figure>();
        selectedCell = currentCell;
        playerOwnershipManager = GetComponent<PlayerOwnershipManager>();
        playerOwnershipManager.UpdateStatus();
        player = new Player();
        firstPlayerTimerbar = GameObject.Find("First Player Timer Bar").GetComponent<ProgressBarBehaviour>();
        InvokeRepeating("HandleSupplies", 2.0f, 5.0f);
        UpdateBar();
        selectedCellColor = currentCell.color;
    }

    void HandleCellSelection(HexCell cell)
    {
        selectedCell.color = selectedCellColor;
        selectedCellColor = cell.color;
        selectedCell = cell;
        cell.color = Color.red;
        cell.Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("LeftJoystickHorizontal");
        float vAxis = Input.GetAxis("LeftJoystickVertical");

        if (vAxis > 0.15 && vAxis < 0.88 && hAxis > 0.15 && hAxis < 0.88) //prawa góra
        {
            if (currentCell.coordinates.Z == 14)
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[0]);
        }

        else if (vAxis > 0.15 && vAxis < 0.85 && hAxis > -0.85 && hAxis  < -0.15) //lewa góra
        {
            if (currentCell.coordinates.Z == 14 || (currentCell.coordinates.X == 0 && currentCell.coordinates.Z % 2 == 0))
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[5]);
        }

        else if (vAxis <= 0.15 && vAxis >= -0.15 && hAxis >= 0.85) //prawa
        {
            HandleCellSelection(currentCell.neighbors[1]);
        }

        else if (vAxis <= 0.25 && vAxis >= -0.25 && hAxis <= -0.85) //lewa
        {
            if (currentCell.coordinates.X == 0 && currentCell.coordinates.Z % 2 == 0)
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[4]);
        }

        else if (vAxis < -0.15 && vAxis > -0.85 && hAxis > 0.15  &&  hAxis < 0.85) //prawy dół
        {
            if (currentCell.coordinates.Z == 0)
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[2]);
        }

        else if (vAxis < -0.15 && vAxis > -0.85 && hAxis < -0.15 && hAxis > -0.92) //lewy dół
        {
            if (currentCell.coordinates.Z == Mathf.Abs(currentCell.coordinates.X * 2)) // to powinno mieć sens!
            {
                return;  
            }
            if (currentCell.coordinates.Z == 0)
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[3]);
        }
        if (Input.GetButtonDown("BButton"))
        {
            Debug.Log(currentCell.coordinates.X + " " + currentCell.coordinates.Y + " " + currentCell.coordinates.Z);
        }
        if (Input.GetButtonDown("AButton"))
        {
            if (selectedCell != null && player.Supplies > 0)
            {
                selectedCell.Color = selectedCellColor;
                MoveFigure(selectedCell);
                UpdateBar();
            }
        }
    }        

    void UpdateBar()
    {
        firstPlayerTimerbar.Value = player.Supplies;
    }

    void HandleSupplies()
    {
        player.Supplies += 2;
        UpdateBar();
    }

    private void MoveFigure(HexCell selectedCell)
    {
        if (Mathf.Abs(hexGrid.GetCell(selectedFigure.transform.position).coordinates.X - selectedCell.coordinates.X) > 1
                || Mathf.Abs(hexGrid.GetCell(selectedFigure.transform.position).coordinates.Y - selectedCell.coordinates.Y) > 1
                || Mathf.Abs(hexGrid.GetCell(selectedFigure.transform.position).coordinates.Z - selectedCell.coordinates.Z) > 1
                || Mathf.Abs(hexGrid.GetCell(selectedFigure.transform.position).Elevation - selectedCell.Elevation) > 1)
        {
            return;
        }

        if (hexGrid.GetCell(selectedFigure.transform.position).coordinates.X == selectedCell.coordinates.X
            && hexGrid.GetCell(selectedFigure.transform.position).coordinates.Y == selectedCell.coordinates.Y)
        {
            return;
        }
        if (hexGrid.GetCell(this.transform.position).isWallNonCapsule)
        {
            return;
        }
        if (selectedCell.HasRiver)
        {
            return;
        }
        if (selectedCell.IsUnderwater)
        {
            return;
        }
        else if (selectedCell.isWallNonCapsule)
        {
            return;
        }
        else if (selectedCell.isWallCapsule)
        {
            selectedFigure.transform.position = selectedCell.transform.position;
            currentCell = selectedCell;
            currentCell.color = selectedCellColor;
            playerOwnershipManager.UpdateStatus();
            player.Supplies--;
            
        }
        else
        {
            selectedFigure.transform.position = selectedCell.transform.position;
            hexGrid.GetCell(selectedFigure.transform.position).Walled = true;
            hexGrid.GetCell(selectedFigure.transform.position).isWallCapsule = true;
            currentCell = selectedCell;
            currentCell.color = selectedCellColor;
            playerOwnershipManager.UpdateStatus();
            player.Supplies--;
            player.Supplies += selectedCell.FarmLevel;

        }
    }
}
