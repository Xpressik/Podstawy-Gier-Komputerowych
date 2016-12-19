using UnityEngine;
using System.Collections;

public enum BuildingType
{
    WALLS, PLANTS
}


public class Player : MonoBehaviour {

    private BuildingType type;

    public string Name { get { return name; } set { name = value;  } }

    public Player (string name, BuildingType type)
    {
        this.name = name;
        this.type = type;
    }

    public BuildingType Type { get { return type; } set { type = value; } }

}
