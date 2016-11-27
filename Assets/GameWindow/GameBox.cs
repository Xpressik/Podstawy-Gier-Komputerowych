using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameBox : MonoBehaviour
{

    public Text round;
    public Text player;
    public Game game;

    // Use this for initialization
    void Start()
    {
        round = round.GetComponent<Text>();
        player = player.GetComponent<Text>();
        setRound(0);
        this.player.text = "test";
        //this.game = GameObject.Find("MainMenu").GetComponent<Canvas>().game;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setRound(int number)
    {
        round.text = number.ToString();
    }

    public void SetActivePLayer(int i)
    {
        this.player.text = game.listOfPlayers[i].getName();
    }
}

