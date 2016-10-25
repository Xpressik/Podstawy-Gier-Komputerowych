using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    float speed = 50.0f;
    int boundary = 1;
    int width;
    int height;

    float ROTSpeed = 10f;
    float min = (float) -10.0;
    float max = (float) 10.0;


    void Start () {
        width = Screen.width ;
        height = Screen.height + 10;

        min = Camera.main.fov + min;
        max = Camera.main.fov + max;
    }
	
	void Update () {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        this.transform.Rotate(-45, 0, 0);
        this.transform.Translate(movement);
        this.transform.Rotate(45, 0, 0);


        if (Input.mousePosition.x > width - boundary)
        {
            transform.position -= new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * speed,
                                       0.0f, 0.0f);
        }

        if (Input.mousePosition.x < 0 + boundary)
        {
            transform.position -= new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * speed,
                                       0.0f, 0.0f);
        }

        if (Input.mousePosition.y > height - boundary)
        {
            transform.position -= new Vector3(0.0f, 0.0f,
                                       Input.GetAxis("Mouse Y") * Time.deltaTime * speed);
        }

        if (Input.mousePosition.y < 0 + boundary)
        {
            transform.position -= new Vector3(0.0f, 0.0f,
                                       Input.GetAxis("Mouse Y") * Time.deltaTime * speed);
        }

        if (Camera.main.fov <= min)
            Camera.main.fov = min;
        if (Camera.main.fov >= max)
            Camera.main.fov = max;
        Camera.main.fov += Input.GetAxis("Mouse ScrollWheel") * ROTSpeed;
    }
}







