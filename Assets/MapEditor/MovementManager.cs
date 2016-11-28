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

        public Text currentPlayer;

        private HexMapCamera camera;

        private void Start()
        {
            Figures = new Figure[20, 15];

            var cellPosition = hexGrid.GetCell(new Vector3(155.8846f, -0.4045f, 30.0f)).transform.position;
            SpawnFigure(2, 8, 2, 155.8846f, -0.4045f, 30.0f);
            SpawnFigure(3, 2, 13, 147.2243f, 0.8379046f, 195.0f);
            camera = GameObject.Find("Hex Map Camera").GetComponent<HexMapCamera>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleInput(false);
                if (selectionX >= 0 && selectionY >= 0)
                {
                    //if (firstMove == true )
                    //{
                    //    if (!isCapsule)
                    //    {
                    //        firstMove = false;
                    //    }
                    //    if (Figures[selectionX, selectionY] != null)
                    //    {
                    //        firstMove = true;
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        SpawnFigure(0, selectionX, selectionY, positionX, positionY, positionZ);
                    //    }
                    //}
                    //else
                    //{
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
                    //}
                }
            }
            if (selectedFigure != null)
            {
                HandleInput(true);
            }
        }       
        private void HandleInput(bool stickToMouse)
        {
            if (!Camera.main)
            {
                return;
            }
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(inputRay, out hit))
            {

                if (stickToMouse)
                {
                    selectedFigure.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }
                else
                {
                    var tmp = hexGrid.GetCell(hit.point);
                    positionX = tmp.transform.position.x;
                    positionY = Mathf.Abs(tmp.transform.position.y);
                    positionZ = tmp.transform.position.z;
                    selectionX = tmp.coordinates.X;
                    selectionY = tmp.coordinates.Z;
                }
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
               // SetColor(x, y, Color.cyan);
                Figures[x, y].SetPosition(x, y);
                isCapsule = false;
            //   currentPlayer.text = "Player : Cylinder";
             }
             else
             {
                GameObject go = (GameObject)Instantiate(figuresPrefabs[index], new Vector3(positionX, positionY, positionZ), southOrientation);
                go.transform.SetParent(transform);
                Figures[x, y] = go.GetComponent<Figure>();
                //SetColor(x, y, Color.yellow);
                Figures[x, y].SetPosition(x, y);
                isCapsule = true;
             //   currentPlayer.text = "Player : Capsule";
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
          //  SetColor(x, y, Color.red);
        }

        private void MoveFigure(int x, int y, float positionX, float positionY, float positionZ)
        {
            if (Figures[x, y] != null)
            {
                return;
            }
            Figures[selectedFigure.CurrentX, selectedFigure.CurrentY] = null;
            selectedFigure.transform.position = new Vector3(positionX, positionY, positionZ);            
            Figures[x, y] = selectedFigure;
            selectedFigure = null;

            if (Figures[x, y].name.Equals("MECH(Clone)"))
            {
                //SetColor(x, y, Color.cyan);
            //    currentPlayer.text = "Player : Cylinder";
                isCapsule = false;
                camera.SecondPlayerMovePositon();
            }
            else
            {
                //SetColor(x, y, Color.yellow);
             //   currentPlayer.text = "Player : Capsule";
                isCapsule = true;
                camera.FirstPlayerMovePosition();
            }
            // camera.AdjustZoom(-200f); oddalenie kamery do fazy ruchu
        }

        private void SetColor(int x, int y, Color color)
        {
            Figures[x, y].GetComponent<Renderer>().material.color = color;   
        }
    }
}
