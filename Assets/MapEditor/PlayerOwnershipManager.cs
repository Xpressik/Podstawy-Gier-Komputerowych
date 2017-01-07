using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ProgressBar;

namespace Assets
{
    public class PlayerOwnershipManager : MonoBehaviour
    {

        public Text text;
        public HexGrid grid;

        private ProgressBarBehaviour progressBar;

        int walledCounter = 0;
        int plantCounter = 0;

        void Start()
        {
            progressBar = GameObject.Find("ProgressBarLabelInside").GetComponent<ProgressBarBehaviour>();
            text.text = ("Walls: (0) 50% : 50% (0) :Plants");
            progressBar.Value = 50;
        }

        private void countFieldsPossession()
        {
            walledCounter = 0;
            plantCounter = 0;
            for (int i = 0; i < grid.CellCountX * grid.CellCountZ; i++)
            {
                if (grid.Cells[i].isWallCapsule)
                {
                    walledCounter++;
                }
                else if (grid.Cells[i].isWallNonCapsule)
                {
                    plantCounter++;
                }
            }
        }

        private void SetText()
        {
            int walledOwnershipPercentage = walledCounter * 100 / (walledCounter + plantCounter);
            int plantOwnershipPercentage = plantCounter * 100 / (walledCounter + plantCounter);
            if(walledOwnershipPercentage + plantOwnershipPercentage < 100)
            {
                walledOwnershipPercentage++;
            }
            text.text = ("Walls: (" + walledCounter + ") " + walledOwnershipPercentage + "% : " + plantOwnershipPercentage + "% (" + plantCounter + ") :Plants");
            progressBar.Value = walledOwnershipPercentage;
        }

        public void UpdateStatus()
        {
            countFieldsPossession();
            SetText();
        }
    }
}
