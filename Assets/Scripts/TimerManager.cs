using Assets.Scripts;
using ProgressBar;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class TimerManager : MonoBehaviour
    {
        private ProgressBarBehaviour timerBar;
        private static float timer;
        private Text timerText;
        private string timeLeft;
        private bool isGameOver;

        public Canvas canvasGameOver;
        public Canvas canvasGUI;
        public Canvas canvasIntro;

        bool isIntro = true;

        public static float Timer { get { return timer; } }

        FirstPlayerTargetingManager fptm;
        SecondPlayerTargetingManager sptm;

        void Start()
        {
            canvasGUI.enabled = false;
            canvasGameOver.enabled = false;
            canvasGameOver.GetComponent<GameOverButtonsManager>().enabled = false;
            
            fptm = GetComponent<FirstPlayerTargetingManager>();
            sptm = GetComponent<SecondPlayerTargetingManager>();
            fptm.CancelInvoke();
            fptm.enabled = false;
            sptm.CancelInvoke();
            sptm.enabled = false;

            timer = 301.0f; // 300.0f
            timerBar = GameObject.Find("Timer Bar").GetComponent<ProgressBarBehaviour>();
            timerText = GameObject.Find("Timer Label").GetComponent<Text>();
        }

        public void StartGame()
        {
            canvasGUI.enabled = true;
            canvasIntro.enabled = false;
            isIntro = false;

            fptm = GetComponent<FirstPlayerTargetingManager>();
            fptm.enabled = true;
            fptm.InvokeRepeating("HandleSupplies", 2.0f, 5.0f);
            sptm = GetComponent<SecondPlayerTargetingManager>();
            sptm.enabled = true;
            sptm.InvokeRepeating("HandleSupplies", 2.0f, 5.0f);

            timer = 300.0f;
        }

        void Update()
        {
            if (isIntro)
            {

            }
            else if (!isGameOver)
            {
                timer -= Time.deltaTime;
                timerBar.Value = (int)(timer * 100 / 300);

                int minutes = (int)(timer / 60);
                int seconds = (int)(timer - minutes * 60);
                timeLeft = minutes.ToString();
                if (seconds < 10)
                {
                    timeLeft += ":0" + seconds.ToString();
                }
                else
                {
                    timeLeft += ":" + seconds.ToString();
                }
                timerText.text = timeLeft;

                if (timer <= 0)
                {
                    GetComponent<SpecialPowerHandler>().enabled = false;

                    isGameOver = true;
                    FirstPlayerTargetingManager fptm = GetComponent<FirstPlayerTargetingManager>();
                    fptm.CancelInvoke();
                    fptm.enabled = false; 
                    SecondPlayerTargetingManager sptm = GetComponent<SecondPlayerTargetingManager>();
                    sptm.CancelInvoke();
                    sptm.enabled = false;
                    
                    GameOverHandler gameOverHandler = GetComponent<GameOverHandler>();
                    gameOverHandler.OnGameOver();
                }
            }
            else
            {
                
            }
        }

            
    }
}
