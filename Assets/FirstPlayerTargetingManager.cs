using System;
using UnityEngine;
using System.Collections;
using Assets;
using UnityEditor;

public class FirstPlayerTargetingManager : MonoBehaviour
{
    private float speed = 18;
    private Rigidbody rg;

    public HexGrid hexGrid;

    public Figure selectedFigure;

    private Color selectedCellColor;

    private float movementTime = 10f;

    // Use this for initialization
    void Start ()
	{
        this.transform.position = new Vector3(155.8846f, 30f, 30.0f);
	    rg = GetComponent<Rigidbody>();
 
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float hAxis = Input.GetAxis("LeftJoystickHorizontal");
	    float vAxis = Input.GetAxis("LeftJoystickVertical");

	    Vector3 movement = transform.TransformDirection(new Vector3(hAxis, 0, vAxis)*speed*Time.deltaTime);

        rg.MovePosition(transform.position + movement);
        selectedFigure = GameObject.Find("MECH(Clone)").GetComponent<Figure>();

        HexCell selectedCell = hexGrid.GetCell(this.transform.position);

        selectedCellColor = selectedCell.Color;
        selectedCell.Color = Color.red;

        if (Input.GetButtonDown("AButton"))
	    {
            MoveFigure(selectedCell);
            selectedCell.Color = selectedCellColor;
        }
        selectedCell.Color = selectedCellColor;
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
            Wait(1, () => { selectedFigure.transform.position = selectedCell.transform.position; });            
        }
        else
        {
            Wait(1, () => {
                selectedFigure.transform.position = selectedCell.transform.position;
                hexGrid.GetCell(selectedFigure.transform.position).Walled = true;
                hexGrid.GetCell(selectedFigure.transform.position).isWallCapsule = true;
            });            
        }
    }
}
