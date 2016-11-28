using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FieldInfoScript : MonoBehaviour {

    public Canvas fieldInfo;
    public Text supplyOutput;
	// Use this for initialization
	void Start () {
        fieldInfo = fieldInfo.GetComponent<Canvas>();
        supplyOutput = supplyOutput.GetComponent<Text>();
        supplyOutput.text = "test";
        fieldInfo.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        fieldInfo.enabled = true;
    }
}
