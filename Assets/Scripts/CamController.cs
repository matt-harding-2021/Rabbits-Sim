using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float turnSpeed = 100;

    private Vector3 motion;
    public float moveSpeed = 150;

    public float zoomSpeed = 300;

    void Update()
    {
        //Rotation//
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0)); //transform according to input
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0); //Convert to local rotation
        }

        //Strafe//
        motion.x = transform.worldToLocalMatrix.MultiplyVector(transform.right).x * Input.GetAxisRaw("Horizontal");
        motion.y = transform.worldToLocalMatrix.MultiplyVector(transform.up).y * Input.GetAxisRaw("Vertical");

        //Zoom//
            motion.z = transform.worldToLocalMatrix.MultiplyVector(transform.forward).z * Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;

        transform.Translate(motion * moveSpeed * Time.deltaTime);
    }
}
