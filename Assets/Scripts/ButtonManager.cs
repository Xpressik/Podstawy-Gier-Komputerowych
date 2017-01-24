using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class ButtonManager : MonoBehaviour {

    //  menu glowne
    public Button startButton;
    public Button mapEditorButton;
    public Button quitButton;

    public void Start()
    {
        startButton = startButton.GetComponent<Button>();
        mapEditorButton = mapEditorButton.GetComponent<Button>();
        quitButton = quitButton.GetComponent<Button>();
    }
    public void NewGamePress()
    {
		SceneManager.LoadScene(1);
    }

    public void MapEditorPress()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitPress()
    {
        Application.Quit();
    }
}
