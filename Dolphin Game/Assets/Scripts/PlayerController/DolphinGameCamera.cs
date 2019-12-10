using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DolphinGameCamera : MonoBehaviour
{
    [SerializeField]
    private float _characterBounds = 30;

    private CharacterSelection _characterSelection;
    private PauseMenuScreen _pauseMenuScreen;
    private Game _game;


    [SerializeField] private float clipNearOffset;

    private PlayerController playerController;

    PostProcessVolume volume;
    DepthOfField depthOfFieldLayer = null;
    ColorGrading colorGrading = null;


    private Vector3 desiredTargetPosition;

    public float zoom = 0f;
    public float Zoom
    {
        get => zoom; set
        {
            zoom = value;
        }
    }

    public Vector3 offset = new Vector3(0, 0, 0);

    public float damping = 1.0f;
    public Rigidbody Rigidbody { get; set; }
    private Camera Camera { get; set; }
    public Vector3 OffsetToTarget
    {
        get => offset; set
        {
            if (value.z < -5)
            {
                offset = value;
            }
        }
    }

    public float CharacterBounds
    {
        get { return _characterBounds; }
        set
        {
            if (value >= 0)
                _characterBounds = value;
        }
    }

    public float followSpeed;
    public float panSpeed;
    public float zoomSpeed;

    private bool manualCamera = false;

    private Vector3 desiredCameraPosition;


    void Awake()
    {

        _game = FindObjectOfType<Game>();

        if (_game == null)
        {
            throw new DolphinGameException("There is no Game Component in this Scene.");
        }

        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            throw new DolphinGameException("There is no PlayerController Component in this Scene. Please add one PlayerController from the Prefabs folder.");
        }

        _characterSelection = FindObjectOfType<CharacterSelection>();
        if (_characterSelection == null)
        {
            throw new DolphinGameException("There is no CharacterSelection Component in this Scene. Please add one CharacterSelection from the Prefabs folder.");
        }

        _pauseMenuScreen = FindObjectOfType<PauseMenuScreen>();
        if (_pauseMenuScreen == null)
        {
            throw new DolphinGameException("There is no PauseMenuScreen Component in this Scene.");
        }


        Rigidbody = GetComponent<Rigidbody>();
        Camera = GetComponent<Camera>();

        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out depthOfFieldLayer);
        volume.profile.TryGetSettings(out colorGrading);



    }

    Vector3 targetPosition;
    Vector3 cameraPositionSmoothDampVelocity;
    Vector3 targetPositionSmoothDampVelocity;
    void LateUpdate()
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
                desiredTargetPosition += cameraMovement * panSpeed;
            }
            Bounds bounds = new Bounds(desiredTargetPosition, new Vector3(_characterBounds, _characterBounds));

            desiredTargetPosition = bounds.center;

            float radius = bounds.size.magnitude / 2f;

            float horizontalFOV = 2f * Mathf.Atan(Mathf.Tan(Camera.fieldOfView * Mathf.Deg2Rad / 2f) * Camera.aspect) * Mathf.Rad2Deg;
            float fov = Mathf.Min(Camera.fieldOfView, horizontalFOV);
            float dist = radius / (Mathf.Sin(fov * Mathf.Deg2Rad / 2f));
            // OffsetToTarget = new Vector3(0, 0, -bounds.size.magnitude +  Zoom);
            OffsetToTarget = new Vector3(0, 0, -dist);
            desiredCameraPosition = desiredTargetPosition + OffsetToTarget;
        }
        else if (playerController.CurrentPlayerController == playerController.DolphinPlayerController)
        {
            Bounds bounds = new Bounds(playerController.DolphinPlayerController.transform.position, new Vector3(_characterBounds, _characterBounds));
            bounds.Encapsulate(new Bounds(playerController.RayPlayerController.transform.position, new Vector3(_characterBounds, _characterBounds)));




            desiredTargetPosition = bounds.center;



            float radius = bounds.size.magnitude / 2f;

            float horizontalFOV = 2f * Mathf.Atan(Mathf.Tan(Camera.fieldOfView * Mathf.Deg2Rad / 2f) * Camera.aspect) * Mathf.Rad2Deg;
            float fov = Mathf.Min(Camera.fieldOfView, horizontalFOV);
            float dist = radius / (Mathf.Sin(fov * Mathf.Deg2Rad / 2f));
            // OffsetToTarget = new Vector3(0, 0, -bounds.size.magnitude +  Zoom);
            OffsetToTarget = new Vector3(0, 0, -dist);
            desiredCameraPosition = desiredTargetPosition + OffsetToTarget;


        }
        else
        {
            Bounds bounds = new Bounds(playerController.CurrentPlayerController.transform.position, new Vector3(_characterBounds, _characterBounds));
            desiredTargetPosition = bounds.center;
            float radius = bounds.size.magnitude / 2f;

            float horizontalFOV = 2f * Mathf.Atan(Mathf.Tan(Camera.fieldOfView * Mathf.Deg2Rad / 2f) * Camera.aspect) * Mathf.Rad2Deg;
            float fov = Mathf.Min(Camera.fieldOfView, horizontalFOV);
            float dist = radius / (Mathf.Sin(fov * Mathf.Deg2Rad / 2f));
            // OffsetToTarget = new Vector3(0, 0, -bounds.size.magnitude +  Zoom);
            OffsetToTarget = new Vector3(0, 0, -dist);
            desiredCameraPosition = desiredTargetPosition + OffsetToTarget;
        }




        Vector3 position = Vector3.SmoothDamp(transform.position, desiredCameraPosition, ref cameraPositionSmoothDampVelocity, 1f);
        transform.position = new Vector3(position.x, position.y, position.z);
        //Camera.nearClipPlane = Mathf.Abs(transform.position.z) - clipNearOffset;

        HandleDepthOfFIeld();

        targetPosition = Vector3.SmoothDamp(targetPosition, desiredTargetPosition, ref targetPositionSmoothDampVelocity, 0.5f);
        transform.LookAt(targetPosition);

        var leftTrigger = Input.GetAxis("Left Trigger");
        var rightTrigger = Input.GetAxis("Right Trigger");

        if (leftTrigger < -0.1)
        {
            CharacterBounds -= leftTrigger * zoomSpeed;
        }
        if (rightTrigger < -0.1)
        {
            CharacterBounds += rightTrigger * zoomSpeed;
        }
    }

    private void HandleDepthOfFIeld()
    {
        if (_characterSelection.Visible || _pauseMenuScreen.InHistory || _game.Spotted)
        {
            depthOfFieldLayer.focusDistance.value = 0;
            depthOfFieldLayer.focalLength.value = 300;

            colorGrading.saturation.value = -100;

        }
        else
        {
            depthOfFieldLayer.focusDistance.value = Mathf.Abs(transform.position.z);
            depthOfFieldLayer.focalLength.value = Mathf.Abs(transform.position.z / 3);
            colorGrading.saturation.value = 0;
        }



    }
}