using UnityEngine;

namespace Assets.Scripts
{
    public class GameOverButtonsManager : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetButtonDown("SecondBButton") || Input.GetButtonDown("BButton"))
            {
                Application.Quit();
            }

            if (Input.GetButtonDown("SecondAButton") || Input.GetButtonDown("AButton"))
            {
                // załaduj menu główne! 
            }
        }
    }
}
