using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinPlayerController : PlayerControllerBase
{
    [SerializeField] private Transform eccoOrigin;

    public Transform EccoOrigin { get => eccoOrigin; }

    void Start()
    {
        base.Init();
    }

    void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;
    }


}
