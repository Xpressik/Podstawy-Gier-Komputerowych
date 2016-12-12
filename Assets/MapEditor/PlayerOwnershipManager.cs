using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Assets
{
    public class PlayerOwnershipManager : MonoBehaviour
    {

        public Text text;
        public HexGrid grid;

        //public int WalledCounter
        //{
        //    get
        //    {
        //        return walledCounter;
        //    }
        //}

        //public int PlantCounter
        //{
        //    get
        //    {
        //        return plantCounter;
        //    }
        //}

        int walledCounter = 0;
        int plantCounter = 0;

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
            text.text = ("Walls: (" + walledCounter + ") " + walledOwnershipPercentage + " : " +
                         plantOwnershipPercentage + " (" + plantCounter + ") :Plant");
        }

        public void UpdateStatus()
        {
            countFieldsPossession();
            SetText();
        }
    }
}
