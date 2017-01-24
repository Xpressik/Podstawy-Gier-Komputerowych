using UnityEngine;

namespace Assets.Scripts
{
    class GameOverHandler : MonoBehaviour
    {
        public Canvas canvasGameOver;
        public Canvas canvasGUI;

        public void OnGameOver()
        {
            canvasGUI.enabled = false;
            canvasGameOver.enabled = true;
            PlayerOwnershipManager.GameOverStatus();
            canvasGameOver.GetComponent<GameOverButtonsManager>().enabled = true;
        }
    }
}
