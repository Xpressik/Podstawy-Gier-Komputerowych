using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour{

    // zmienne do zainicjowania

    public List<Player> listOfPlayers;
    //public List<Army> listOfArmies;
    public GameBox gameBox;
    public int numberOfTours = 10;
    public bool ready = false;
    public string faza;
    public int runda;
    public Player activePlayer;
    
    void Start()
    {
 
    }

    void Update()
    {
        //gameBox.stage.text = "Action Stage";
    }
    public void Initialize(string name1, string name2)
    {
        InitializePlayers(name1, name2);
        gameBox = gameBox.GetComponent<GameBox>();
        gameBox.enabled = true;
        this.gameBox.setGame(this);
        this.gameBox.Initialize();
        Gameplay(10);
    }

    public void InitializePlayers(string name1, string name2)
    {
        listOfPlayers = new List<Player>();
        Player player1 = new Player(name1);
        Player player2 = new Player(name2);
        listOfPlayers.Add(player1);
        listOfPlayers.Add(player2);
    }

    public void Gameplay(int numberOfTours)
    {
        for (int i = 0; i < numberOfTours; i++)
        {
            runda += runda;
            Stage1();
            Stage2();
            Stage3();
        }
    }

    public bool Stage1()
    {
        faza = "Planning Stage";
        for (int i = 0; i < listOfPlayers.Count; i++)
        {
            activePlayer = listOfPlayers[i];
            Planning(listOfPlayers[i]);
            if (ready == true)
            {
                return true;
            }
        }
        return false;
    }

    public void Stage2()
    {
        faza = "Action Stage";
    }

    public void Stage3()
    {
        faza = "Random Stage";
    }

    public void Planning(Player player)
    {

    }
}
