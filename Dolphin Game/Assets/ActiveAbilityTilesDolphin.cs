using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilityTilesDolphin : AbilityTiles
{
    private DolphinPlayerController _dolphinPlayerController;


    private float _hackProgressFillVisualisation;
    private float _timeSinceLastHackProgressFillVisualisationUpdate;

    public float HackProgressFillVisualisation
    {
        get => _hackProgressFillVisualisation;
        set
        {
            if (_hackProgressFillVisualisation != value)
            {
                _hackProgressFillVisualisation = value;

                _timeSinceLastHackProgressFillVisualisationUpdate = 0;
            }
            else
            {
                _timeSinceLastHackProgressFillVisualisationUpdate += Time.deltaTime;
            }
        }
    }

    public override void Awake()
    {
        base.Awake();

        _dolphinPlayerController = FindObjectOfType<DolphinPlayerController>();

        if (_dolphinPlayerController == null)
        {
            throw new DolphinGameException("DolphinPlayerController Object couldn't be found.");
        }


    }

    public override void Update()
    {
        base.Update();


        HandleActiveAbilityVisualisation();
    }

    private bool _triggerNextEchoAbilityCooldownFinished = true;

    private bool _triggerNextBubbleAbilityCooldownFinished = true;

    private void HandleActiveAbilityVisualisation()
    {
        if (_triggerNextEchoAbilityCooldownFinished && _dolphinPlayerController.EchoAbilityCooldownFinished)
        {
            westIcon.TriggerCooldownFinished();
            _triggerNextEchoAbilityCooldownFinished = false;
        }
        if (!_dolphinPlayerController.EchoAbilityCooldownFinished)
        {
            _triggerNextEchoAbilityCooldownFinished = true;
        }
        westIcon.Filled = _dolphinPlayerController.EchoAbilityCooldown;

        westIcon.Usable = _dolphinPlayerController.NoJammerInReach;


        if (_triggerNextBubbleAbilityCooldownFinished && _dolphinPlayerController.BubbleAbilityCooldownFinished)
        {
            eastIcon.TriggerCooldownFinished();
            _triggerNextBubbleAbilityCooldownFinished = false;
        }

        if (!_dolphinPlayerController.BubbleAbilityCooldownFinished)
        {
            _triggerNextBubbleAbilityCooldownFinished = true;
        }
        eastIcon.Filled = _dolphinPlayerController.BubbleAbilityCooldown;

        northIcon.Usable = _dolphinPlayerController.AtLeastOneJammerInHackDistance;

        if (_dolphinPlayerController.HackingInProgress)
        {
            northIcon.Filled = HackProgressFillVisualisation + _timeSinceLastHackProgressFillVisualisationUpdate * 0.05f;

            HackProgressFillVisualisation = _dolphinPlayerController.HackProgress;
        }
        else
        {
            northIcon.Filled = 1;
        }





    }
}
