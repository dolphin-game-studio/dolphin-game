using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DolphinGameCamera : MonoBehaviour
{
    private CharacterSelection characterSelection;

    [SerializeField] private float clipNearOffset;

    private PlayerController playerController;

    PostProcessVolume volume;
    DepthOfField depthOfFieldLayer = null;
    ColorGrading colorGrading = null;


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
    
    public float followSpeed;
    public float panSpeed;
    public float zoomSpeed;

    private bool manualCamera = false;

    private Vector3 desiredPosition;


    void Start()
    {
 

        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            throw new DolphinGameException("There is no PlayerController Component in this Scene. Please add one PlayerController from the Prefabs folder.");
        }

        characterSelection = FindObjectOfType<CharacterSelection>();
        if (characterSelection == null)
        {
            throw new DolphinGameException("There is no CharacterSelection Component in this Scene. Please add one CharacterSelection from the Prefabs folder.");
        }

        Rigidbody = GetComponent<Rigidbody>();
        Camera = GetComponent<Camera>();

        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out depthOfFieldLayer);
        volume.profile.TryGetSettings(out colorGrading);



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
            targetPosition = Vector3.MoveTowards(targetPosition , playerController.CurrentPlayerController.transform.position, Time.deltaTime * followSpeed);
        }

        desiredPosition = targetPosition + Offset;


        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        transform.position = new Vector3(position.x, position.y, position.z);
        Camera.nearClipPlane = Mathf.Abs(transform.position.z) - clipNearOffset;


        HandleDepthOfFIeld();



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

    private void HandleDepthOfFIeld()
    {
        if (characterSelection.Visible)
        {
            depthOfFieldLayer.focusDistance.value = 0;
            depthOfFieldLayer.focalLength.value = 300;

            colorGrading.saturation.value = -100;

        }
        else if (characterSelection.NotVisible)
        {
            depthOfFieldLayer.focusDistance.value = Mathf.Abs(transform.position.z);
            depthOfFieldLayer.focalLength.value = Mathf.Abs(transform.position.z * 3);
            colorGrading.saturation.value = 0;
        }



    }
}