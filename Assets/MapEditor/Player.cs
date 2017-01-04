using UnityEngine;
using System.Collections;

public enum BuildingType
{
    WALLS, PLANTS, NONE
}


public class Player {

    private BuildingType type;
    private string playerName;

    public string Name { get { return playerName; } set { playerName = value;  } }

    public Player (string name, BuildingType type)
    {
        this.playerName = name;
        this.type = type;
    }

    public BuildingType Type { get { return type; } set { type = value; } }

}
