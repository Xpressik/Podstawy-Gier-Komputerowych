using UnityEngine;
using System.Collections;

public class HexCellHighlight : MonoBehaviour {


    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.black);
    }
    void OnMouseExit()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
    }
}
