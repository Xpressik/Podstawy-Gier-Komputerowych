using UnityEngine;
using System.Collections;

public class Field {

    //public Castle camp;
    public int supply;
    public int contraband; // waluta
    public int garrison;
    //public Castle castle;
    //public Army army;
    public Player owner;
    public HexCell hex;
    public int ownerInt;
    public bool camp;
    public int playerInt;

    public Field(HexCell hex)
    {
        this.hex = hex;
    }
}
