using System;
using UnityEngine;
using System.Collections;
using Assets;
using UnityEditor;

public class FirstPlayerTargetingManager : MonoBehaviour
{
    private float speed = 18;

    private HexGrid hexGrid;

    private Figure selectedFigure;

    private Color selectedCellColor;

    private float movementTime = 10f;

    private HexCell currentCell;

    public GameObject figure;

    private HexCell selectedCell;

    private PlayerOwnershipManager playerOwnershipManager;

    // Use this for initialization
    void Start()
    {
        //        this.transform.position = new Vector3(155.8846f, 30f, 30.0f);
        //	    rg = GetComponent<Rigidbody>();
        //          selectedFigure = GameObject.Find("MECH(Clone)").GetComponent<Figure>();
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
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("LeftJoystickHorizontal");
        float vAxis = Input.GetAxis("LeftJoystickVertical");

        if (vAxis > 0.15 && vAxis < 0.88 && hAxis > 0.15 && hAxis < 0.88) //prawa góra
        {
            Debug.Log("Prawa góra");
            selectedCellColor = currentCell.neighbors[0].color;
            selectedCell = currentCell.neighbors[0];
            currentCell.neighbors[0].color = Color.red;
        }

        else if (vAxis > 0.15 && vAxis < 0.85 && hAxis > -0.85 && hAxis  < -0.15) //lewa góra
        {
            Debug.Log("Lewa góra");
            selectedCellColor = currentCell.neighbors[5].color;
            selectedCell = currentCell.neighbors[5];
            currentCell.neighbors[5].color = Color.red;
        }

        else if (vAxis <= 0.15 && vAxis >= -0.15 && hAxis >= 0.85) //prawa
        {
            Debug.Log("Prawa");
            selectedCellColor = currentCell.neighbors[1].color;
            selectedCell = currentCell.neighbors[1];
            currentCell.neighbors[1].color = Color.red;
        }

        else if (vAxis <= 0.25 && vAxis >= -0.25 && hAxis <= -0.85) //lewa
        {
            Debug.Log("Lewa");
            selectedCellColor = currentCell.neighbors[4].color;
            selectedCell = currentCell.neighbors[4];
            currentCell.neighbors[4].color = Color.red;
        }

        else if (vAxis < -0.15 && vAxis > -0.85 && hAxis > 0.15  &&  hAxis < 0.85) //prawy dół
        {
            Debug.Log("Prawy dół");
            selectedCellColor = currentCell.neighbors[2].color;
            selectedCell = currentCell.neighbors[2];
            currentCell.neighbors[2].color = Color.red;
        }

        else if (vAxis < -0.15 && vAxis > -0.85 && hAxis < -0.15 && hAxis > -0.92) //lewy dół
        {
            Debug.Log("Lewy dół");
            selectedCellColor = currentCell.neighbors[3].color;
            selectedCell = currentCell.neighbors[3];
            currentCell.neighbors[3].color = Color.red;
        }

        if (Input.GetButtonDown("AButton"))
        {
            if (selectedCell != null)
            {
                selectedCell.Color = selectedCellColor;
                MoveFigure(selectedCell);
            }
        }
    }

    public void Wait(float seconds, Action action)
    {
        StartCoroutine(_wait(seconds, action));
    }

    IEnumerator _wait(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
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
        else if (selectedCell.isWallNonCapsule)
        {
            return;
        }
        else if (selectedCell.isWallCapsule)
        {
            selectedFigure.transform.position = selectedCell.transform.position;
            currentCell = selectedCell;
            playerOwnershipManager.UpdateStatus();
        }
        else
        {
            selectedFigure.transform.position = selectedCell.transform.position;
            hexGrid.GetCell(selectedFigure.transform.position).Walled = true;
            hexGrid.GetCell(selectedFigure.transform.position).isWallCapsule = true;
            currentCell = selectedCell;
            playerOwnershipManager.UpdateStatus();
        }
    }
}
