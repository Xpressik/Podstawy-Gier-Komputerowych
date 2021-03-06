﻿using ProgressBar;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class MovementManager : MonoBehaviour
    {

        public HexGrid hexGrid;

        public GameObject[] figuresPrefabs;

        public Figure[,] Figures;

        private Figure selectedFigure;

        private int selectionX = -1;
        private int selectionY = -1;

        private bool firstMove = true;
        public bool isCapsule = true;

        private float positionX;
        private float positionY;
        private float positionZ;

        private Player[] players;

        private PlayerOwnershipManager playerOwnershipManager;

        private new HexMapCamera camera;  // kazał mi dodać new  nie sprawdzałem czy to coś ZMIENIA <-----------------------        

        public float firstPlayerCounter;
        public float secondPlayerCounter;

        public bool firstTimerEnabled = true;
        public bool secondTimerEnabled = true;

        public bool firstPlayerEnableMove = true;
        public bool secondPlayerEnableMove = true;

        private ProgressBarBehaviour firstPlayerTimerbar;
        private ProgressBarBehaviour secondPlayerTimerbar;

        private HexCell selectedCell;
        private Color selectedCellColor;

        public float speed = 18;

        private void Start()
        {
            Figures = new Figure[20, 15];

            var cellPosition = hexGrid.GetCell(new Vector3(155.8846f, -0.4045f, 30.0f)).transform.position;
            SpawnFigure(2, 8, 2, 155.8846f, -0.4045f, 30.0f);
            hexGrid.GetCell(new Vector3(155.8846f, -0.4045f, 30.0f)).Walled = true;
            hexGrid.GetCell(new Vector3(155.8846f, -0.4045f, 30.0f)).isWallCapsule = true;
            SpawnFigure(3, 2, 13, 147.2243f, 0.8379046f, 195.0f);
            hexGrid.GetCell(new Vector3(147.2243f, 0.8379046f, 195.0f)).PlantLevel = 3;
            hexGrid.GetCell(new Vector3(147.2243f, 0.8379046f, 195.0f)).isWallNonCapsule = true;
            camera = GameObject.Find("Hex Map Camera").GetComponent<HexMapCamera>();

            firstPlayerCounter = 60.0f;
            secondPlayerCounter = 60.0f;

            playerOwnershipManager = hexGrid.GetComponent<PlayerOwnershipManager>();

            firstPlayerTimerbar = GameObject.Find("First Player Timer Bar").GetComponent<ProgressBarBehaviour>();
            secondPlayerTimerbar = GameObject.Find("Second Player Timer Bar").GetComponent<ProgressBarBehaviour>();
            firstPlayerTimerbar.Value = 100;
            secondPlayerTimerbar.Value = 100;

            players = new Player[2] { new Player("first", BuildingType.WALLS), new Player("second", BuildingType.PLANTS) };
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleInput();
                if (selectionX >= 0 && selectionY >= 0)
                {
                    if (selectedFigure == null)
                    {
                        // select the figure
                        SelectFigure(selectionX, selectionY);
                    }
                    else
                    {
                        //move figure
                        MoveFigure(selectionX, selectionY, positionX, positionY, positionZ);
                    }
                }
            }
            if (selectedFigure != null)
            {
                HandleInput();
            }
            if (isCapsule && firstTimerEnabled)
            {
                firstPlayerCounter -= Time.deltaTime;
               // Debug.Log("First player: " + firstPlayerCounter);
            }
            if (!isCapsule && secondTimerEnabled)
            {
                secondPlayerCounter -= Time.deltaTime;
               // Debug.Log("Second player: " + secondPlayerCounter);
            }
            if (firstPlayerCounter <= 0.0f)
            {
                firstPlayerEnableMove = false;
                firstTimerEnabled = false;
            }
            if (secondPlayerCounter <= 0.0f)
            {
                secondPlayerEnableMove = false;
                secondTimerEnabled = false;
            }
            if (!firstPlayerEnableMove && !secondPlayerEnableMove)
            {
                
            }
            UpdateTimerBars();
        }

        private void UpdateTimerBars()
        {
            firstPlayerTimerbar.Value = (int)(firstPlayerCounter * 100 / 60);
            secondPlayerTimerbar.Value = (int)(secondPlayerCounter * 100 / 60);
        }
        private void HandleInput()
        {
            if (!Camera.main)
            {
                return;
            }
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(inputRay, out hit))
            {
                var tmp = hexGrid.GetCell(hit.point);
                positionX = tmp.transform.position.x;
                positionY = Mathf.Abs(tmp.transform.position.y);
                positionZ = tmp.transform.position.z;
                selectionX = tmp.coordinates.X;
                selectionY = tmp.coordinates.Z;
            }
            else
            {
                selectionX = -1;
                selectionY = -1;
            }
        }

        private void SpawnFigure(int index, int x, int y, float positionX, float positionY, float positionZ)
        {
            Quaternion northOrietation = Quaternion.Euler(0, 0, 0);
            Quaternion southOrientation = Quaternion.Euler(0, 180, 0);
            if (isCapsule)
            {
                GameObject go = (GameObject)Instantiate(figuresPrefabs[index], new Vector3(positionX, positionY, positionZ), northOrietation);
                go.transform.SetParent(transform);
                go.AddComponent<MeshRenderer>();
                Figures[x, y] = go.GetComponent<Figure>();
                Figures[x, y].SetPosition(x, y);
                isCapsule = false;
            }
            else
            {
                GameObject go = (GameObject)Instantiate(figuresPrefabs[index], new Vector3(positionX, positionY, positionZ), southOrientation);
                go.transform.SetParent(transform);
                Figures[x, y] = go.GetComponent<Figure>();
                Figures[x, y].SetPosition(x, y);
                isCapsule = true;
            }
        }

        private void SelectFigure(int x, int y)
        {
            if (Figures[x, y] == null)
            {
                return;
            }
            if (isCapsule)
            {
                if (Figures[x, y].name.Equals("Trooper(Clone)"))
                {
                    return;
                }
            }
            else
            {
                if (Figures[x, y].name.Equals("MECH(Clone)"))
                {
                    return;
                }
            }
            selectedFigure = Figures[x, y];

            selectedCell = hexGrid.GetCell(selectedFigure.transform.position);
            selectedCellColor  =  selectedCell.Color;
            selectedCell.Color = Color.red;   
        }

        private void MoveFigure(int x, int y, float positionX, float positionY, float positionZ)
        {
            Figures[selectedFigure.CurrentX, selectedFigure.CurrentY] = null;

            HexCell currentCell = hexGrid.GetCell(new Vector3(positionX, positionY, positionZ));
            
            if (Mathf.Abs(hexGrid.GetCell(selectedFigure.transform.position).coordinates.X - currentCell.coordinates.X) > 1 
                || Mathf.Abs(hexGrid.GetCell(selectedFigure.transform.position).coordinates.Y - currentCell.coordinates.Y) > 1 
                || Mathf.Abs(hexGrid.GetCell(selectedFigure.transform.position).coordinates.Z - currentCell.coordinates.Z) > 1
                || Mathf.Abs(hexGrid.GetCell(selectedFigure.transform.position).Elevation - currentCell.Elevation) > 1)
            {
                return;
            }

            if (hexGrid.GetCell(selectedFigure.transform.position).coordinates.X == currentCell.coordinates.X
                && hexGrid.GetCell(selectedFigure.transform.position).coordinates.Y == currentCell.coordinates.Y)
            {
                return;
            }

            if (selectedFigure.name.Equals("MECH(Clone)") && firstPlayerEnableMove)
            {
                if (hexGrid.GetCell(new Vector3(positionX, positionY, positionZ)).isWallNonCapsule)
                {
                    return;
                }
                if (currentCell.HasRiver)
                {
                    return;
                }
                else if (currentCell.isWallNonCapsule)
                {
                    return;
                }
                else if (currentCell.isWallCapsule)
                {
                    selectedFigure.transform.position = new Vector3(positionX, positionY, positionZ);
                }
                else
                {
                    selectedFigure.transform.position = new Vector3(positionX, positionY, positionZ);
                    hexGrid.GetCell(selectedFigure.transform.position).Walled = true;
                    hexGrid.GetCell(selectedFigure.transform.position).isWallCapsule = true;
                }                
            }
            else if(selectedFigure.name.Equals("Trooper(Clone)") && secondPlayerEnableMove)
            {
                if (hexGrid.GetCell(new Vector3(positionX, positionY, positionZ)).isWallCapsule)
                {
                    return;
                }
                if (currentCell.HasRiver)
                {
                    return;
                }
                else if (currentCell.isWallCapsule)
                {
                    return;
                }
                else if (currentCell.isWallNonCapsule)
                {
                    selectedFigure.transform.position = new Vector3(positionX, positionY, positionZ);
                }
                else
                {
                    selectedFigure.transform.position = new Vector3(positionX, positionY, positionZ);
                    hexGrid.GetCell(selectedFigure.transform.position).PlantLevel = 3;
                    hexGrid.GetCell(selectedFigure.transform.position).isWallNonCapsule = true;
                }
                
            }


            Figures[x, y] = selectedFigure;
            selectedCell.Color = selectedCellColor;
            selectedFigure = null;

            if (Figures[x, y].name.Equals("MECH(Clone)"))
            {
                //    currentPlayer.text = "Player : Cylinder";
                isCapsule = false;

            }
            else
            {
                //   currentPlayer.text = "Player : Capsule";
                isCapsule = true;
           
            }

            if (!firstPlayerEnableMove)
            {
                isCapsule = false;
            }
            if (!secondPlayerEnableMove)
            {
                isCapsule = true;
            }

            playerOwnershipManager.UpdateStatus();
        }

        private void SetColor(int x, int y, Color color)
        {
            Figures[x, y].GetComponent<Renderer>().material.color = color;
        }
    }
}
