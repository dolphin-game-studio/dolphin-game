using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FollowCamera : MonoBehaviour
{
    private PlayerController playerController;

    PostProcessVolume volume;
    DepthOfField depthOfFieldLayer = null;


    private Vector3 targetPosition;


    public Vector3 offset = new Vector3(0, 0, 0);

    public float damping = 1.0f;
    public Rigidbody Rigidbody { get; set; }
    private Camera Camera { get; set; }
    public Vector3 Offset
    {
        get => offset; set
        {
            if (value.z < -5)
            {
                offset = value;
            }
        }
    }

    public float panSpeed;
    public float zoomSpeed;

    private bool manualCamera = false;

    private Vector3 desiredPosition;


    void Start()
    {
        var cameras = FindObjectsOfType<Camera>();
        if (cameras.Length > 1)
        {
            throw new DolphinGameException("There are two cameras in this scene. Keep the camera inside the Essentials object and delete any other.");
        }

        playerController = FindObjectOfType<PlayerController>();


        if (playerController == null)
        {
            throw new DolphinGameException("There is no PlayerController Component in this Scene. Please add one PlayerController from the Prefabs folder.");
        }

        Rigidbody = GetComponent<Rigidbody>();
        Camera = GetComponent<Camera>();

        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out depthOfFieldLayer);
    }


    void Update()
    {
        var leftHorizontal = Input.GetAxis("Horizontal");
        var leftVertical = Input.GetAxis("Vertical");

        var rightHorizontal = Input.GetAxis("Right Horizontal");
        var rightVertical = Input.GetAxis("Right Vertical");

        if (Mathf.Abs(rightHorizontal) + Mathf.Abs(rightVertical) > 0)
        {
            manualCamera = true;
        }
        if (Mathf.Abs(leftHorizontal) + Mathf.Abs(leftVertical) > 0)
        {
            manualCamera = false;
        }

        if (manualCamera)
        {
            if (Mathf.Abs(rightHorizontal) + Mathf.Abs(rightVertical) > 0)
            {
                var cameraMovement = new Vector3(rightHorizontal, rightVertical);
                targetPosition += cameraMovement * panSpeed;
            }
        }
        else
        {
            targetPosition = playerController.CurrentPlayerController.transform.position;
        }

        desiredPosition = targetPosition + Offset;


        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        transform.position = new Vector3(position.x, position.y, position.z);
        Camera.nearClipPlane = Mathf.Abs(transform.position.z) - 5;
        depthOfFieldLayer.focusDistance.value = Mathf.Abs(transform.position.z);


        transform.LookAt(targetPosition);

        var leftTrigger = Input.GetAxis("Left Trigger");
        var rightTrigger = Input.GetAxis("Right Trigger");

        if (leftTrigger < -0.1)
        {
            Offset += new Vector3(0, 0, leftTrigger * zoomSpeed);
        }
        if (rightTrigger < -0.1)
        {
            Offset += new Vector3(0, 0, -rightTrigger * zoomSpeed);
        }
    }
}