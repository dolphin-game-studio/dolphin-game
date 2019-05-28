﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    DolphinPlayerController currentDolphinPlayerController;

    [SerializeField]
    private DolphinPlayerController[] dolphinPlayerControllers;
    private int currentPlayerIndex = 0;

    [SerializeField]
    private FollowCamera camera;

    void Start()
    {
        if (currentDolphinPlayerController == null)
        {
            Debug.LogError("CurrentDolphinPlayerController is not set on PlayerController");
        }
         
        if (camera == null)
        {
            Debug.LogError("Camera is not set on PlayerController");
        }
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");


        float dolphinSpeed = currentDolphinPlayerController.Speed;
        currentDolphinPlayerController.Move(horizontal * dolphinSpeed, vertical * dolphinSpeed);

        bool xPressed = Input.GetKeyUp(KeyCode.X);

        if (xPressed)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % dolphinPlayerControllers.Length;
            currentDolphinPlayerController = dolphinPlayerControllers[currentPlayerIndex];

            camera.Target = currentDolphinPlayerController.gameObject;
        }
    }
}
