using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class MovementManager : MonoBehaviour
    {

        public HexGrid hexGrid;

        public GameObject[] figuresPrefabs;

        private Quaternion orientation = Quaternion.Euler(0, 180, 0);

        public Figure[,] Figures;

        private Figure selectedFigure;

        private int selectionX = -1;
        private int selectionY = -1;

        private bool firstMove = true;
        private bool isCapsule = true;

        private float positionX;
        private float positionY;
        private float positionZ;

        public Text currentPlayer;

        private void Start()
        {
            Figures = new Figure[6, 6];
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                UpdateSelection();
                if (selectionX >= 0 && selectionY >= 0)
                {
                    Debug.Log(selectionX  + " " + selectionY);
                    if (firstMove == true )
                    {
                        if (!isCapsule)
                        {
                            firstMove = false;
                        }
                        SpawnFigure(0, selectionX, selectionY, positionX, positionY, positionZ);
                       
                    }
                    else
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
            }
        }       
        private void UpdateSelection()
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
                positionY = Mathf.Abs(tmp.transform.position.y + 5);
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
            if (isCapsule)
            {
                GameObject go = (GameObject)Instantiate(figuresPrefabs[0], new Vector3(positionX, positionY, positionZ), orientation);
                go.transform.SetParent(transform);
                Figures[x, y] = go.GetComponent<Figure>();
                SetColor(x, y, Color.cyan);
                Figures[x, y].SetPosition(x, y);
                isCapsule = false;
             //   currentPlayer.text = "Player : Cylinder";
            }
            else
            {
                GameObject go = (GameObject)Instantiate(figuresPrefabs[1], new Vector3(positionX, positionY, positionZ), orientation);
                go.transform.SetParent(transform);
                Figures[x, y] = go.GetComponent<Figure>();
                SetColor(x, y, Color.yellow);
                Figures[x, y].SetPosition(x, y);
                isCapsule = true;
             //   currentPlayer.text = "Player : Capsule";
            }
        }

        private void SelectFigure(int x, int y)
        {
            if (Figures [x, y] == null)
            {
                return;
            }
            if (isCapsule)
            {
                if (Figures[x, y].name.Equals("Cylinder(Clone)"))
                {
                    return;
                }
            }
            else
            {
               if (Figures[x, y].name.Equals("Capsule(Clone)"))
                {
                    return;
                }
            }
            selectedFigure = Figures[x, y];
            SetColor(x, y, Color.red);
        }

        private void MoveFigure(int x, int y, float positionX, float positionY, float positionZ)
        {
            if(Figures[x, y] != null)
            {
                return;
            }
            Figures[selectedFigure.CurrentX, selectedFigure.CurrentY] = null;
            selectedFigure.transform.position = new Vector3(positionX, positionY, positionZ);
            Figures[x, y] = selectedFigure;
            selectedFigure = null;

            if (Figures[x, y].name.Equals("Capsule(Clone)"))
            {
                SetColor(x, y, Color.cyan);
            //    currentPlayer.text = "Player : Cylinder";
                isCapsule = false;
            }
            else
            {
                SetColor(x, y, Color.yellow);
             //   currentPlayer.text = "Player : Capsule";
                isCapsule = true;
            }
        }

        private void SetColor(int x, int y, Color color)
        {
            Figures[x, y].GetComponent<Renderer>().material.color = color;
            
        }
    }
}
