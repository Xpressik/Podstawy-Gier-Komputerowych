using UnityEngine;
using System.Collections;
using Assets;

public class Hex : MonoBehaviour {

    public Vector2 HexPosition;
    public HexModel HexModel { get; set; }

    public Field field;

    public Castle camp;
    public int supply;
    public int contraband; // waluta
    public int garrison;
    public Player owner;

    public void InitializeModel()
    {
        var hex = new GameObject();
        hex.AddComponent<HexModel>();
        HexModel = hex.GetComponent<HexModel>();
        hex.transform.parent = transform;
        hex.transform.localPosition = new Vector3(0, 0, 0);
        hex.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
        hex.GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/hex") as Texture2D;

        camp = new Castle();
        supply = 2;
        contraband = 1;
        garrison = 1;
        owner = new Player("", "");

        field = new Field(camp, supply, contraband, garrison, owner);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
