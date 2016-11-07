using UnityEngine;


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

        // Use this for initialization
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
                        firstMove = false;
                        SpawnFigure(0, selectionX, selectionY);
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
                            MoveFigure(selectionX, selectionY);
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
                selectionX = hexGrid.GetCell(hit.point).coordinates.X;
                selectionY = hexGrid.GetCell(hit.point).coordinates.Z;
            }
            else
            {
                selectionX = -1;
                selectionY = -1;
            }
        }

        private void SpawnFigure(int index, int x, int y)
        {
            int xMovement  = x* 15;
            int yMovement  = y * 15;
            if(y == 1 || y == 3 || y == 5 )
            {
                xMovement += 10;
            }

            GameObject go = Instantiate(figuresPrefabs[index], new Vector3(xMovement, 0, yMovement) , orientation) as GameObject;
            go.transform.SetParent(transform);
            Figures[x, y] = go.GetComponent<Figure>();
            Figures[x, y].SetPosition(x, y);
        }

        private void SelectFigure(int x, int y)
        {
            if (Figures [x, y] == null)
            {
                Debug.Log("TAK");
                return;
            }
            selectedFigure = Figures[x, y];
        }

        private void MoveFigure(int x, int y)
        {
            Figures[selectedFigure.CurrentX, selectedFigure.CurrentY] = null;
            selectedFigure.transform.position = new Vector3(x, y);
            Figures[x, y] = selectedFigure; 
            selectedFigure = null;
        }
    }
}
