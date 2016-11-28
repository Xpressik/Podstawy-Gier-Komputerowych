using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameBox : MonoBehaviour
{
    public Canvas canvas;
    public GameObject panel;
    public Button readyButton;

    public Text round;
    public Text player;
    public Game game;
    public Text stage;
    public bool ready;

    // Use this for initialization
    void Start()
    {
        panel = panel.GetComponent<GameObject>();
        canvas.enabled = true;
        //panel.active = true;
        round = round.GetComponent<Text>();
        player = player.GetComponent<Text>();
        stage = stage.GetComponent<Text>();
        stage.enabled = true;
    }

    // Update is called once per frame
    public void Update()
    {
        setRound(game.runda);
        setStage(game.faza);
    }

    public void setRound(int number)
    {
        round.text = number.ToString();
    }

    public void setStage(string stage)
    {
        this.stage.text = stage;
    }

    public void setActivePlayer(Player player)
    {
        this.player.text = player.getName();
    }

    public void SetActivePLayer(int i)
    {
        this.player.text = game.listOfPlayers[i].getName();
    }

    public void setGame(Game game)
    {
        this.game = game;
    }
    public void Initialize()
    {
        round = round.GetComponent<Text>();
        player = player.GetComponent<Text>();
        setRound(0);
        this.player.text = "test";
    }

    public void ReadyButtonPress()
    {
        this.game.ready = true;
    }
}

