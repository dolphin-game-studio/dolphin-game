using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityTiles : MonoBehaviour
{

    public float RotationChangeSpeed => abilityVisualisation.RotationChangeSpeed;

    [SerializeField] private RectTransform abilityTileNorthImage;
    [SerializeField] private RectTransform abilityTileWestImage;
    [SerializeField] private RectTransform abilityTileEastImage;
    [SerializeField] private RectTransform abilityTileSouthImage;

    public RectTransform AbilityTileNorthImage => abilityTileNorthImage;
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



    protected AbilityIcon northIcon;
    protected AbilityIcon westIcon;
    protected AbilityIcon eastIcon;
    protected AbilityIcon southIcon;


    public virtual void Awake()
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



        northIcon = AbilityTileNorthImage.GetComponent<AbilityIcon>();

        if (northIcon == null)
        {
            throw new DolphinGameException("northIcon has no AbilityIcon Component");
        }

        westIcon = AbilityTileWestImage.GetComponent<AbilityIcon>();

        if (westIcon == null)
        {
            throw new DolphinGameException("westIcon has no AbilityIcon Component");
        }

        eastIcon = AbilityTileEastImage.GetComponent<AbilityIcon>();

        if (eastIcon == null)
        {
            throw new DolphinGameException("eastIcon has no AbilityIcon Component");
        }

        southIcon = AbilityTileSouthImage.GetComponent<AbilityIcon>();

        if (southIcon == null)
        {
            throw new DolphinGameException("southIcon has no AbilityIcon Component");
        }
    }

    public virtual void Update()
    {
        HandleAbilityVisualisationImagesRotation();

        UpdateAbilitiTilesActiveHighlight();
    }

    public abstract void UpdateAbilitiTilesActiveHighlight();

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
