using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinPlayerController : SmallWhaleControllerBase
{
  
    void Start()
    {
        base.Init();
    }

    protected override void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;

        base.Update();
    }


}
