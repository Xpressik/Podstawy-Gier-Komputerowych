using UnityEngine; 

namespace Assets
{
    public class StartButtonOnClick : MonoBehaviour
    {

        public HexGrid grid;

        TimerManager timerManager;

        void Start()
        {
            timerManager = grid.GetComponent<TimerManager>();
        }

        void Update()
        {
            if (Input.GetButtonDown("SecondAButton") || Input.GetButtonDown("AButton"))
            {
                timerManager.StartGame();
                Destroy(this);
            }
        }
    }
}
