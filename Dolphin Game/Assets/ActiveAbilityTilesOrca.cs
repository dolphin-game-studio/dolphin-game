using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AbilityTiles))]
public class ActiveAbilityTilesOrca : MonoBehaviour
{
    private OrcaPlayerController _orcaPlayerController;

    private AbilityTiles _abilityTiles;
       

    void Awake()
    {
        _abilityTiles = GetComponent<AbilityTiles>();

        _orcaPlayerController = FindObjectOfType<OrcaPlayerController>();

        if (_orcaPlayerController == null)
        {
            throw new DolphinGameException("OrcaPlayerController Object couldn't be found.");
        }
    }

    void Update()
    {
        HandleActiveAbilityVisualisation();
    }

    private void HandleActiveAbilityVisualisation()
    {
        _abilityTiles.AbilityTileWestImage.GetComponent<Image>().fillAmount = _orcaPlayerController.EchoAbilityCooldown;
    }
}
