using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTiles : MonoBehaviour
{

    public float RotationChangeSpeed => abilityVisualisation.RotationChangeSpeed;

    [SerializeField] private RectTransform abilityTileNorthImage;
    [SerializeField] private RectTransform abilityTileWestImage;
    [SerializeField] private RectTransform abilityTileEastImage;
    [SerializeField] private RectTransform abilityTileSouthImage;

    public RectTransform AbilityTileNorthImage  => abilityTileNorthImage;
    public RectTransform AbilityTileWestImage => abilityTileWestImage;
    public RectTransform AbilityTileEastImage => abilityTileEastImage;
    public RectTransform AbilityTileSouthImage => abilityTileSouthImage;


    [SerializeField] private AbilityVisualisation abilityVisualisation;


    #region Visible

    private bool _visible;
    public bool Visible
    {
        get => _visible;
        set
        {
            if (_visible != value)
            {
                _visible = value;
            }
        }
    }

    public bool NotVisible { get => !Visible; set => Visible = !value; }

    #endregion

    public Quaternion DesiredRotation => Visible ? Quaternion.LookRotation(Vector3.forward, new Vector3(1, 1, 0)) : Quaternion.LookRotation(new Vector3(1, 1, 0), Vector3.back);


    void Awake()
    {
        if (abilityVisualisation == null)
        {
            throw new DolphinGameException("abilityVisualisation is not set");
        }
        if (abilityTileNorthImage == null)
        {
            throw new DolphinGameException("abilityTileNorthImage is not set");
        }
        if (abilityTileWestImage == null)
        {
            throw new DolphinGameException("abilityTileWestImage is not set");
        }
        if (abilityTileEastImage == null)
        {
            throw new DolphinGameException("abilityTileEastImage is not set");
        }
        if (abilityTileSouthImage == null)
        {
            throw new DolphinGameException("abilityTileSouthImage is not set");
        }
    }

    void Update()
    {
        HandleAbilityVisualisationImagesRotation();
    }


    private void HandleAbilityVisualisationImagesRotation()
    {

        GraduallyChangeImageRotation(abilityTileNorthImage, DesiredRotation);
        GraduallyChangeImageRotation(abilityTileWestImage, DesiredRotation);
        GraduallyChangeImageRotation(abilityTileEastImage, DesiredRotation);
        GraduallyChangeImageRotation(abilityTileSouthImage, DesiredRotation);
    }

    private void GraduallyChangeImageRotation(RectTransform image, Quaternion rotation)
    {
        image.localRotation = Quaternion.RotateTowards(image.localRotation, rotation, RotationChangeSpeed);
    }
}
