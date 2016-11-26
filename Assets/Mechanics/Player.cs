using UnityEngine;
using System.Collections;
using System;

public class Player {
    private int turn;
    private string name;

    public Player(string name)
    {
        this.name = name;
    }

    public string getName()
    {
        return this.name;
    }

}
