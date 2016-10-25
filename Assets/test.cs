using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log("Mouse is over" + hitInfo.collider.name);
            Destroy(gameObject);
            GameObject hitObject = hitInfo.transform.root.gameObject;

            //    SelectObject(hitObject);
        }
        else
        {
            //ClearSelection();
        }
    }
}
