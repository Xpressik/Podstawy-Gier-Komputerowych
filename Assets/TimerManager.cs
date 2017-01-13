using ProgressBar;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class TimerManager : MonoBehaviour
    {
        private ProgressBarBehaviour timerBar;
        private float timer;
        private Text timerText;
        private string timeLeft;
        private bool isGameOver;

        void Start()
        {
            timer = 300.0f; // 300.0f
            timerBar = GameObject.Find("Timer Bar").GetComponent<ProgressBarBehaviour>();
            timerText = GameObject.Find("Timer Label").GetComponent<Text>();
        }

        void Update()
        {
            if (!isGameOver)
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
                    isGameOver = true;
                    FirstPlayerTargetingManager fptm = GetComponent<FirstPlayerTargetingManager>();
                    fptm.CancelInvoke();
                    fptm.enabled = false; 
                    SecondPlayerTargetingManager sptm = GetComponent<SecondPlayerTargetingManager>();
                    sptm.CancelInvoke();
                    sptm.enabled = false;   
                }
            }
            else
            {
                
            }
        }

            
    }
}
