using UnityEngine;
using System.Collections;
using Assets;
using System;

public class SecondPlayerTargetingManager : MonoBehaviour
{

    private Rigidbody rg;
    private float speed = 18;

    public HexGrid hexGrid;

    public Figure selectedFigure;

    private Color selectedCellColor;

    private float movementTime = 10f;

    // Use this for initialization
    void Start ()
    {
        this.transform.position = new Vector3(147.2243f, 30f, 195.0f);
        rg = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update ()
	{
	    float hAxis = Input.GetAxis("SecondLeftJoystickHorizontal");
	    float vAxis = Input.GetAxis("SecondLeftJoystickVertical");

	    Vector3 movement = transform.TransformDirection(new Vector3(hAxis, 0, vAxis)*speed*Time.deltaTime);

        rg.MovePosition(transform.position + movement);
	    selectedFigure = GameObject.Find("Trooper(Clone)").GetComponent<Figure>();

        HexCell selectedCell = hexGrid.GetCell(this.transform.position);

        if (Input.GetButtonDown("SecondAButton"))
        {
            MoveFigure(selectedCell);
        }
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
        if (hexGrid.GetCell(this.transform.position).isWallCapsule)
        {
            return;
        }
        if (selectedCell.HasRiver)
        {
            return;
        }
        else if (selectedCell.isWallCapsule)
        {
            return;
        }
        else if (selectedCell.isWallNonCapsule)
        {
            selectedFigure.transform.position = selectedCell.transform.position;
        }
        else
        {
            selectedFigure.transform.position = selectedCell.transform.position;
            hexGrid.GetCell(selectedFigure.transform.position).PlantLevel = 3;
            hexGrid.GetCell(selectedFigure.transform.position).isWallNonCapsule = true;
        }
    }
}
