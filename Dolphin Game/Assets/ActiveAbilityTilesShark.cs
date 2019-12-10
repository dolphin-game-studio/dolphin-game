using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 public class ActiveAbilityTilesShark : AbilityTiles
{
    private SharkPlayerController _sharkPlayerController;
         
    public override void Awake()
    {
        base.Awake();

        _sharkPlayerController = FindObjectOfType<SharkPlayerController>();

        if (_sharkPlayerController == null)
        {
            throw new DolphinGameException("SharkPlayerController Object couldn't be found.");
        }        
    }

    public override void Update()
    {
        base.Update();

        HandleActiveAbilityVisualisation();
    }

    private bool _triggerNextEchoAbilityCooldownFinished = true;

    private bool _triggerNextRamAbilityCooldownFinished = true;


    private void HandleActiveAbilityVisualisation()
    {
        westIcon.Usable = _sharkPlayerController.SharkToDistractInReach;
        eastIcon.Usable = _sharkPlayerController.AnglerInReach;
        northIcon.Usable = _sharkPlayerController.KnockedOutSharkWithSuperiorUniformInReach;
    }


    public override void UpdateAbilitiTilesActiveHighlight()
    {
        northIcon.Active = Input.GetButton("Y Button");
        westIcon.Active = Input.GetButton("X Button");
        eastIcon.Active = Input.GetButton("B Button");
        southIcon.Active = Input.GetButton("A Button");
    }
}
