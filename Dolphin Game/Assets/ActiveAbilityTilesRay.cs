using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 public class ActiveAbilityTilesRay : AbilityTiles
{
    private RayPlayerController _rayPlayerController;




    public override void Awake()
    {
        base.Awake();

        _rayPlayerController = FindObjectOfType<RayPlayerController>();

        if (_rayPlayerController == null)
        {
            throw new DolphinGameException("RayPlayerController Object couldn't be found.");
        }

        
    }

    public override void Update()
    {
        base.Update();
        
        HandleActiveAbilityVisualisation();
    }
     
    private void HandleActiveAbilityVisualisation()
    {
        westIcon.Usable = _rayPlayerController.SharkToStunInReach;
        northIcon.Usable = _rayPlayerController.NarrowCorridorInReach;

        eastIcon.Usable = _rayPlayerController.InvisibleToShark;
        eastIcon.Active = _rayPlayerController.InvisibleToShark;


    }
}
