using UnityEngine;
using Assets;
using ProgressBar;

public class SecondPlayerTargetingManager : MonoBehaviour
{ 
    private HexGrid hexGrid;

    private GameObject selectedFigure;

    private Color selectedCellColor;

    private HexCell currentCell;

    public GameObject figure;

    private HexCell selectedCell;

    private PlayerOwnershipManager playerOwnershipManager;

    private static Player player;

    public static Player Player { get { return player; } }

    private ProgressBarBehaviour secondPlayerTimerBar;

    private SoundsHandler soundsHandler;

    public ParticleSystem explosion;

    // Use this for initialization
    void Start ()
    {
        hexGrid = GetComponent<HexGrid>();
        currentCell = hexGrid.GetCell(new Vector3(147.2243f, 0.8379046f, 195.0f));
        currentCell.isWallNonCapsule = true;
        currentCell.PlantLevel = 2;
        GameObject go = (GameObject)Instantiate(figure, currentCell.transform.position, Quaternion.Euler(0, 180, 0));
        go.transform.SetParent(transform);
        go.AddComponent<MeshRenderer>();
        selectedFigure = go;//.GetComponent<GameObject>();
        selectedCell = currentCell;
        playerOwnershipManager = GetComponent<PlayerOwnershipManager>();
        player = new Player();
        secondPlayerTimerBar = GameObject.Find("Second Player Timer Bar").GetComponent<ProgressBarBehaviour>();
        InvokeRepeating("HandleSupplies", 2.0f, 5.0f);
        UpdateBar();
        selectedCellColor = currentCell.color;
        soundsHandler = GetComponent<SoundsHandler>();
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
    void Update ()
	{
	    float hAxis = Input.GetAxis("SecondLeftJoystickHorizontal");
	    float vAxis = Input.GetAxis("SecondLeftJoystickVertical");

        if (vAxis < -0.15 && vAxis > -0.85 && hAxis < -0.15 && hAxis > -0.92) //prawa góra
        {
            if (currentCell.coordinates.X == 19 && currentCell.coordinates.Z == 1
                || currentCell.coordinates.X == 18 && currentCell.coordinates.Z == 3
                || currentCell.coordinates.X == 17 && currentCell.coordinates.Z == 5
                || currentCell.coordinates.X == 16 && currentCell.coordinates.Z == 7
                || currentCell.coordinates.X == 15 && currentCell.coordinates.Z == 9
                || currentCell.coordinates.X == 14 && currentCell.coordinates.Z == 11
                || currentCell.coordinates.X == 13 && currentCell.coordinates.Z == 13)
            {
                return;
            }
            if (currentCell.coordinates.Z == 14)
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[0]);
        }

        else if (vAxis < -0.15 && vAxis > -0.85 && hAxis > 0.15 && hAxis < 0.85) //lewa góra
        {
            if (currentCell.coordinates.Z == currentCell.coordinates.X * -2) 
            {
                return;
            }
            if (currentCell.coordinates.Z == 14)
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[5]);
        }

        else if (vAxis <= 0.25 && vAxis >= -0.25 && hAxis <= -0.85) //prawa
        {
            if (currentCell.coordinates.X == 12 && currentCell.coordinates.Z == 14
                || currentCell.coordinates.X == 19 && currentCell.coordinates.Z == 1
                || currentCell.coordinates.X == 18 && currentCell.coordinates.Z == 3
                || currentCell.coordinates.X == 17 && currentCell.coordinates.Z == 5
                || currentCell.coordinates.X == 16 && currentCell.coordinates.Z == 7
                || currentCell.coordinates.X == 15 && currentCell.coordinates.Z == 9
                || currentCell.coordinates.X == 14 && currentCell.coordinates.Z == 11
                || currentCell.coordinates.X == 13 && currentCell.coordinates.Z == 13
                || currentCell.coordinates.X == 12 && currentCell.coordinates.Z == 14


                || currentCell.coordinates.X == 13 && currentCell.coordinates.Z == 12
                || currentCell.coordinates.X == 14 && currentCell.coordinates.Z == 10
                || currentCell.coordinates.X == 15 && currentCell.coordinates.Z == 8
                || currentCell.coordinates.X == 16 && currentCell.coordinates.Z == 6
                || currentCell.coordinates.X == 17 && currentCell.coordinates.Z == 4
                || currentCell.coordinates.X == 18 && currentCell.coordinates.Z == 2
                || currentCell.coordinates.X == 19 && currentCell.coordinates.Z == 0
                )
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[1]);
        }

        else if (vAxis <= 0.15 && vAxis >= -0.15 && hAxis >= 0.85) //lewa
        {
            if (currentCell.coordinates.Z == (currentCell.coordinates.X * (-2) + 1))
            {
                return;
            }
            if (currentCell.coordinates.Z == currentCell.coordinates.X * -2) 
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[4]);
        }

        else if (vAxis > 0.15 && vAxis < 0.85 && hAxis > -0.85 && hAxis < -0.15) //prawy dół
        {
            if (currentCell.coordinates.X == 19 && currentCell.coordinates.Z == 1
                || currentCell.coordinates.X == 18 && currentCell.coordinates.Z == 3
                || currentCell.coordinates.X == 17 && currentCell.coordinates.Z == 5
                || currentCell.coordinates.X == 16 && currentCell.coordinates.Z == 7
                || currentCell.coordinates.X == 15 && currentCell.coordinates.Z == 9
                || currentCell.coordinates.X == 14 && currentCell.coordinates.Z == 11
                || currentCell.coordinates.X == 13 && currentCell.coordinates.Z == 13
                || currentCell.coordinates.X == 19 && currentCell.coordinates.Z == 0)
            {
                return;
            }
            if (currentCell.coordinates.Z == 0)
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[2]);
        }

        else if (vAxis > 0.15 && vAxis < 0.88 && hAxis > 0.15 && hAxis < 0.88)  //lewy dół
        {
            if (currentCell.coordinates.Z == currentCell.coordinates.X * -2)
            {
                return;
            }
            if (currentCell.coordinates.Z == 0)
            {
                return;
            }
            HandleCellSelection(currentCell.neighbors[3]);
        }

        if (Input.GetButtonDown("SecondLeftJoystickClick"))
        {
            HandleCellSelection(currentCell);

        }

        if (Input.GetButtonDown("SecondAButton"))
        {
            if (selectedCell != null && player.Supplies > 0)
            {
                selectedCell.Color = selectedCellColor;
                MoveFigure(selectedCell);
                UpdateBar();
            }
            else
            {
                soundsHandler.PlayNotEnoughSuppliesSound();
            }
        }

        if (Input.GetButtonDown("SecondXButton"))
        {
            if (currentCell.UrbanLevel == 0 && currentCell.FarmLevel == 0 && currentCell.SpecialIndex == 0)
            {
                if (player.Supplies > 2) // koszt budowy "fortecy" 3 
                {
                    currentCell.SpecialIndex = 2;
                    soundsHandler.PlayBuildingPlacement();
                    player.Supplies -= 3;
                    UpdateBar();
                }
                else
                {
                    soundsHandler.PlayNotEnoughSuppliesSound();
                }
            }
            else
            {
                soundsHandler.PlayIncorrectMoveSound();
            }
        }

        if (Input.GetButtonDown("SecondBButton")) // Niszczenie murów przeciwnika // NIE MOŻNA NISZCZYć MURÓW NA POLACH, NA KTÓRYCH STOJĄ "FORTECE".
        {
            if (selectedCell.isWallCapsule && selectedCell.SpecialIndex != 1)
            {
                if (player.Supplies >= 5) // koszt niszczenia murów 5 
                {
                    selectedCell.isWallCapsule = false;
                    selectedCell.Walled = false;
                    explosion.transform.position = selectedCell.transform.position;
                    explosion.Play();
                    soundsHandler.PlayDestructionSound();
                    player.Supplies -= 5;
                    playerOwnershipManager.UpdateStatus();
                    UpdateBar();
                }
                else
                {
                    soundsHandler.PlayNotEnoughSuppliesSound();
                }
            }
            else
            {
                soundsHandler.PlayIncorrectMoveSound();
            }
        }
    }

    void UpdateBar()
    {
        secondPlayerTimerBar.Value = player.Supplies;
    }

    void HandleSupplies()
    {
        soundsHandler.PlaySuppliesSound();
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
            if (player.SuperPower == Power.water && selectedCell.IsUnderwater)
            {
            }
            else if (player.SuperPower != Power.climbing)
            {
                soundsHandler.PlayIncorrectMoveSound();
                return;
            }
            
        }

        if (hexGrid.GetCell(selectedFigure.transform.position).coordinates.X == selectedCell.coordinates.X
            && hexGrid.GetCell(selectedFigure.transform.position).coordinates.Y == selectedCell.coordinates.Y)
        {
            soundsHandler.PlayIncorrectMoveSound();
            return;
        }
        if (hexGrid.GetCell(this.transform.position).isWallCapsule)
        {
            soundsHandler.PlayIncorrectMoveSound();
            return;
        }
        if (selectedCell.HasRiver)
        {
            if (player.SuperPower != Power.rivers)
            {
                soundsHandler.PlayIncorrectMoveSound();
                return;
            }
        }
        if (selectedCell.IsUnderwater)
        {
            if (player.SuperPower != Power.water)
            {
                soundsHandler.PlayIncorrectMoveSound();
                return;
            }
        }
         if (selectedCell.isWallCapsule)//
        {
            soundsHandler.PlayIncorrectMoveSound();
            return;
        }
         if (selectedCell.isWallNonCapsule)//
        {
            selectedFigure.transform.position = selectedCell.transform.position;
            currentCell = selectedCell;
            currentCell.color = selectedCellColor;
            playerOwnershipManager.UpdateStatus();
            player.Supplies--;
            soundsHandler.PlayMoveSound();
        }
        else
        {
            selectedFigure.transform.position = selectedCell.transform.position;
            hexGrid.GetCell(selectedFigure.transform.position).PlantLevel = 2;
            hexGrid.GetCell(selectedFigure.transform.position).isWallNonCapsule = true;
            currentCell = selectedCell;
            currentCell.color = selectedCellColor;
            playerOwnershipManager.UpdateStatus();
            player.Supplies--;
            if (selectedCell.FarmLevel > 0)
            {
                soundsHandler.PlaySuppliesSound();
                player.Supplies += selectedCell.FarmLevel;
            }
            else
            {
                soundsHandler.PlayMoveSound();
            }
        }
    }
}
