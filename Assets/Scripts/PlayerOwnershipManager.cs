using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ProgressBar;

namespace Assets
{
    public class PlayerOwnershipManager : MonoBehaviour
    {

        public Text textUpper;
        public Text textLower;
        public HexGrid grid;

        private ProgressBarBehaviour progressBarLower;
        private ProgressBarBehaviour progressBarUpper;

        static int walledCounter = 0;
        static int plantCounter = 0;

        void Start()
        {
            progressBarUpper = GameObject.Find("ProgressBarLabelInside").GetComponent<ProgressBarBehaviour>();
            progressBarLower = GameObject.Find("ProgressBarLabelInside (1)").GetComponent<ProgressBarBehaviour>();

            textUpper.text = ("Walls: (0) 50% : 50% (0) :Plants");
            textLower.text = ("Walls: (0) 50% : 50% (0) :Plants");
            progressBarUpper.Value = 50;
            progressBarLower.Value = 50;
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

        static int walledOwnershipPercentage;
        static int plantOwnershipPercentage;
        private void SetText()
        {
            walledOwnershipPercentage = walledCounter * 100 / (walledCounter + plantCounter);
            plantOwnershipPercentage = plantCounter * 100 / (walledCounter + plantCounter);
            if(walledOwnershipPercentage + plantOwnershipPercentage < 100)
            {
                walledOwnershipPercentage++;
            }
            textUpper.text = ("Walls: (" + walledCounter + ") " + walledOwnershipPercentage + "% : " + plantOwnershipPercentage + "% (" + plantCounter + ") :Plants");
            textLower.text = ("Walls: (" + walledCounter + ") " + walledOwnershipPercentage + "% : " + plantOwnershipPercentage + "% (" + plantCounter + ") :Plants");
            progressBarUpper.Value = walledOwnershipPercentage;
            progressBarLower.Value = walledOwnershipPercentage;
        }

        public void UpdateStatus()
        {
            countFieldsPossession();
            SetText();
        }

        public static void GameOverStatus()
        {
            GameObject.Find("GameOverBar").GetComponent<ProgressBarBehaviour>().Value = walledOwnershipPercentage;
            GameObject.Find("Game Over Stats Text").GetComponent<Text>().text = ("Walls: (" + walledCounter + ") " + walledOwnershipPercentage + "% : " + plantOwnershipPercentage + "% (" + plantCounter + ") :Plants");
        }
    }
}
