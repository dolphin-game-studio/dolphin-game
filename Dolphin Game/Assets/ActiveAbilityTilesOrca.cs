using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 public class ActiveAbilityTilesOrca : AbilityTiles
{
    private OrcaPlayerController _orcaPlayerController;




    public override void Awake()
    {
        base.Awake();

        _orcaPlayerController = FindObjectOfType<OrcaPlayerController>();

        if (_orcaPlayerController == null)
        {
            throw new DolphinGameException("OrcaPlayerController Object couldn't be found.");
        }

        
    }

    void Update()
    {
        base.Update();


        HandleActiveAbilityVisualisation();
    }

    public override void UpdateAbilitiTilesActiveHighlight()
    {
        northIcon.Active = Input.GetButton("Y Button");
        westIcon.Active = Input.GetButton("X Button");
        eastIcon.Active = Input.GetButton("B Button");
        southIcon.Active = Input.GetButton("A Button");
    }

    private bool _triggerNextEchoAbilityCooldownFinished = true;

    private bool _triggerNextRamAbilityCooldownFinished = true;


    private void HandleActiveAbilityVisualisation()
    {
        if (_triggerNextEchoAbilityCooldownFinished && _orcaPlayerController.EchoAbilityCooldownFinished)
        {
            westIcon.TriggerCooldownFinished();
            _triggerNextEchoAbilityCooldownFinished = false;
        }
        if (!_orcaPlayerController.EchoAbilityCooldownFinished)
        {
            _triggerNextEchoAbilityCooldownFinished = true;
        }
        westIcon.Filled = _orcaPlayerController.EchoAbilityCooldown;

        westIcon.Usable = _orcaPlayerController.NoJammerInReach;


        if (_triggerNextRamAbilityCooldownFinished && _orcaPlayerController.RamAbilityCooldownFinished)
        {
            eastIcon.TriggerCooldownFinished();
            _triggerNextRamAbilityCooldownFinished = false;
        }

        if (!_orcaPlayerController.RamAbilityCooldownFinished)
        { 
            _triggerNextRamAbilityCooldownFinished = true;
        }
        eastIcon.Filled = _orcaPlayerController.RamAbilityCooldown;

        eastIcon.Usable = _orcaPlayerController.EitherDestructableOrSharkFound;

        northIcon.Usable = _orcaPlayerController.SharkToTransportInReach;

        //southIcon.Filled = _orcaPlayerController.EchoAbilityCooldown;


        //_abilityTiles.AbilityTileWestImage.GetComponent<Image>().material.SetFloat("_PintPow", echoCooldown);
        // _abilityTiles.AbilityTileWestImage.GetComponent<Image>().material.SetFloat("_DistMultiplier", echoCooldown);

    }
}
