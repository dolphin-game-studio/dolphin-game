using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject FollowObject;

    private Vector3 targetPosition;


    public Vector3 offset = new Vector3(-3, 2, 0);
    public float damping = 1.0f;
    public Rigidbody Rigidbody { get; set; }

    public float panSpeed;
    public float zoomSpeed;

    private bool manualCamera = false;

    private Vector3 desiredPosition;


    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();

    }


    void Update()
    {
        var leftHorizontal = Input.GetAxis("Horizontal");
        var leftVertical = Input.GetAxis("Vertical");
         
        var rightHorizontal = Input.GetAxis("Right Horizontal");
        var rightVertical = Input.GetAxis("Right Vertical");
         
        if (Mathf.Abs(rightHorizontal) + Mathf.Abs(rightVertical) > 0.4)
        {
            manualCamera = true;
        }
        if (Mathf.Abs(leftHorizontal) + Mathf.Abs(leftVertical) > 0.4)
        {
            manualCamera = false;
        }

        if (manualCamera)
        {
            if (Mathf.Abs(rightHorizontal) + Mathf.Abs(rightVertical) > 0.4)
            {
                var cameraMovement = new Vector3(rightHorizontal, rightVertical);
                targetPosition += cameraMovement * panSpeed;
            }
        }
        else
        {
            targetPosition = FollowObject.transform.position;
        }

        desiredPosition = targetPosition + offset;


        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        transform.position = new Vector3(position.x, position.y, position.z);
        transform.LookAt(targetPosition);

        var leftTrigger = Input.GetAxis("Left Trigger");
        var rightTrigger = Input.GetAxis("Right Trigger");
         
        if (leftTrigger < -0.1)
        {
            offset += new Vector3(0,0, leftTrigger * zoomSpeed);
        }
        if (rightTrigger < -0.1)
        {
            offset += new Vector3(0, 0, -rightTrigger * zoomSpeed);
        }
    }
}