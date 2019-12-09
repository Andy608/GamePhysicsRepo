using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Camera { get; private set; }
    private Vector3 velocity = Vector3.zero;
    private float speed = 5.0f;

    private Vector3 prevMousePos;
    private Vector3 mouseDiff;
    //private bool rotateCamera;

    private void Awake()
    {
        Camera = GameObject.Find("Main Camera");
        prevMousePos = Input.mousePosition;
        mouseDiff = prevMousePos;

        //rotateCamera = true;
        //Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            velocity += (-Camera.transform.right * speed);

        if (Input.GetKey(KeyCode.S))
            velocity += (-Camera.transform.forward * speed);

        if (Input.GetKey(KeyCode.D))
            velocity += (Camera.transform.right * speed);

        if (Input.GetKey(KeyCode.W))
            velocity += (Camera.transform.forward * speed);

        Camera.transform.Translate(Camera.transform.InverseTransformDirection(velocity) * Time.deltaTime);
        velocity = Vector3.zero;

        if (Input.GetMouseButton(0))
        {
            mouseDiff = Input.mousePosition - prevMousePos;
            Camera.transform.eulerAngles += new Vector3(-mouseDiff.y, mouseDiff.x, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            //if (rotateCamera)
            //{
            //    rotateCamera = false;
            //    Cursor.visible = true;
            //}
            //else
            //{
            //    rotateCamera = true;
            //    Cursor.visible = false;
            //}
        }

        prevMousePos = Input.mousePosition;
    }
}
