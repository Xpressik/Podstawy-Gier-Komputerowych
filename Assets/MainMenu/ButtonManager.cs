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

    //sub menu
    public Canvas subMenu;
    public Button map1;
    public Button map2;
    public int mapChoser;
    public Button playButton;
    public InputField inputField1;
    public InputField inputField2;

    public Game game;

    // pomocnicze

    string name1;
    string name2;
    public void Start()
    {
        subMenu = subMenu.GetComponent<Canvas>();
        startButton = startButton.GetComponent<Button>();
        mapEditorButton = mapEditorButton.GetComponent<Button>();
        quitButton = quitButton.GetComponent<Button>();
        inputField1 = inputField1.GetComponent<InputField>();
        inputField2 = inputField2.GetComponent<InputField>();

        subMenu.enabled = false;
    }
    public void NewGamePress()
    {
        subMenu.enabled = true;
        map1 = map1.GetComponent<Button>();
        map2 = map2.GetComponent<Button>();
        playButton = playButton.GetComponent<Button>();
    }

    public void MapEditorPress(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void ExitPress()
    {
        Application.Quit();
    }

    public void Map1Press()
    {
        mapChoser = 1;
    }

    public void Map2Press()
    {
        mapChoser = 2;
    }

    public void PlayPress()
    {
        name1 = inputField1.text.ToString();
        name2 = inputField2.text.ToString();
        Console.Out.WriteLine(name1);
        this.game = new Game();
        //this.game.Initialize(name1, name2);
        SceneManager.LoadScene(mapChoser);
    }

    public Game getGame()
    {
        return this.game;
    }
}
