using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVisualisation : MonoBehaviour
{
    [SerializeField] private float rotationChangeSpeed;
    public float RotationChangeSpeed => rotationChangeSpeed;
     
    private PlayerController playerController;
 
    [SerializeField] private AbilityTiles abilityTilesDolphin;
    [SerializeField] private AbilityTiles abilityTilesOrca;
    [SerializeField] private AbilityTiles abilityTilesShark;
    [SerializeField] private AbilityTiles abilityTilesRay;


    private AbilityTiles[] allAbilityTiles;

    private AbilityTiles _activeAbilityTiles;

    public AbilityTiles ActiveAbilityTiles
    {
        get => _activeAbilityTiles;
        set
        {
            _activeAbilityTiles = value;





        }
    }


    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            throw new DolphinGameException("There is no PlayerController Component in this Scene. Please add one PlayerController from the Prefabs folder.");
        }

        allAbilityTiles = new AbilityTiles[] { abilityTilesDolphin, abilityTilesOrca, abilityTilesShark, abilityTilesRay };

        if (abilityTilesDolphin == null)
        {
            throw new DolphinGameException("playerVisualisationImageDolphin is not set");
        }
        if (abilityTilesOrca == null)
        {
            throw new DolphinGameException("playerVisualisationImageOrca is not set");
        }
        if (abilityTilesShark == null)
        {
            throw new DolphinGameException("playerVisualisationImageShark is not set");
        }
        if (abilityTilesRay == null)
        {
            throw new DolphinGameException("playerVisualisationImageRay is not set");
        }



    }


    void Update()
    {
        HandleAbilityTilesVisibility();
    }



    private void HandleAbilityTilesVisibility()
    {

        abilityTilesDolphin.Visible = playerController.DolphinIsActive;
        abilityTilesOrca.Visible = playerController.OrcaIsActive;
        abilityTilesShark.Visible = playerController.SharkIsActive;
        abilityTilesRay.Visible = playerController.RayIsActive;

    }

}
