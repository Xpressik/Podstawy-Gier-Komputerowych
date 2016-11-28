using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour{

    // zmienne do zainicjowania

    public List<Player> listOfPlayers;
    //public List<Army> listOfArmies;
    public GameBox gameBox;
    void Start()
    {

    }
    public void Initialize(string name1, string name2)
    {
        InitializePlayers(name1, name2);
        gameBox = gameBox.GetComponent<GameBox>();
        gameBox.enabled = true;
        this.gameBox.setGame(this);
        this.gameBox.Initialize();
    }

    public void InitializePlayers(string name1, string name2)
    {
        listOfPlayers = new List<Player>();
        Player player1 = new Player(name1);
        Player player2 = new Player(name2);
        listOfPlayers.Add(player1);
        listOfPlayers.Add(player2);
    }
}
