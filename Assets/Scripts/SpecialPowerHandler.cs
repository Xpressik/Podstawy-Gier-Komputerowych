using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SpecialPowerHandler : MonoBehaviour
    {
        private RawImage fp1;
        private RawImage fp2;
        private RawImage fp3;
        private RawImage fp4;

        private RawImage sp1;
        private RawImage sp2;
        private RawImage sp3;
        private RawImage sp4;

        void Start()
        {
            fp1 = GameObject.Find("First Player Power 1").GetComponent<RawImage>();
            fp2 = GameObject.Find("First Player Power 2").GetComponent<RawImage>();
            fp3 = GameObject.Find("First Player Power 3").GetComponent<RawImage>();
            //fp4 = GameObject.Find("First Player Power 4").GetComponent<RawImage>();

            sp1 = GameObject.Find("Second Player Power 1").GetComponent<RawImage>();
            sp2 = GameObject.Find("Second Player Power 2").GetComponent<RawImage>();
            sp3 = GameObject.Find("Second Player Power 3").GetComponent<RawImage>();
            //sp4 = GameObject.Find("Second Player Power 4").GetComponent<RawImage>();

            LoadImage("1not.png", ref fp1);
            LoadImage("2not.png", ref fp2);
            LoadImage("3not.png", ref fp3);

            LoadImage("1not.png", ref sp1);
            LoadImage("2not.png", ref sp2);
            LoadImage("3not.png", ref sp3);
        }
        void Update()
        {
            if ((int)TimerManager.Timer % 90 == 0 || (int)TimerManager.Timer == 300)
            {
                FirstPlayerTargetingManager.Player.SuperPower = (Power)Random.Range(1, 4);
                SecondPlayerTargetingManager.Player.SuperPower = (Power)Random.Range(1, 4);
                switch (FirstPlayerTargetingManager.Player.SuperPower)
                {
                    case Power.rivers:
                        LoadImage("1.png", ref fp1);
                        LoadImage("2not.png", ref fp2);
                        LoadImage("3not.png", ref fp3);
                        break;

                    case Power.water:
                        LoadImage("1not.png", ref fp1);
                        LoadImage("2.png", ref fp2);
                        LoadImage("3not.png", ref fp3);
                        break;

                    case Power.climbing:
                        LoadImage("1not.png", ref fp1);
                        LoadImage("2not.png", ref fp2);
                        LoadImage("3.png", ref fp3);
                        break;
                }

                switch (SecondPlayerTargetingManager.Player.SuperPower)
                {
                    case Power.rivers:
                        LoadImage("1.png", ref sp1);
                        LoadImage("2not.png", ref sp2);
                        LoadImage("3not.png", ref sp3);
                        break;

                    case Power.water:
                        LoadImage("1not.png", ref sp1);
                        LoadImage("2.png", ref sp2);
                        LoadImage("3not.png", ref sp3);
                        break;

                    case Power.climbing:
                        LoadImage("1not.png", ref sp1);
                        LoadImage("2not.png", ref sp2);
                        LoadImage("3.png", ref sp3);
                        break;
                }
            }
        }

        private void LoadImage(string FileName,ref RawImage rawImage)
        {
            byte[] bytes = File.ReadAllBytes("Assets/Resources/SuperPower Icons/" + FileName);
            Texture2D texture = new Texture2D(100, 100);
            texture.LoadImage(bytes);
            rawImage.texture = texture;
        }
    }
}
