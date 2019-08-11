using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualisation : MonoBehaviour
{
    [SerializeField] private float sizeChangeSpeed = 1;
    [SerializeField] private float positionChangeSpeed = 1;

    [SerializeField] private CharacterVisualisationPanel playerVisualisationImageDolphin;
    [SerializeField] private CharacterVisualisationPanel playerVisualisationImageOrca;
    [SerializeField] private CharacterVisualisationPanel playerVisualisationImageShark;
    [SerializeField] private CharacterVisualisationPanel playerVisualisationImageRay;

    public CharacterVisualisationPanel PlayerVisualisationImageDolphin => playerVisualisationImageDolphin;
    public CharacterVisualisationPanel PlayerVisualisationImageOrca => playerVisualisationImageOrca;
    public CharacterVisualisationPanel PlayerVisualisationImageShark => playerVisualisationImageShark;
    public CharacterVisualisationPanel PlayerVisualisationImageRay => playerVisualisationImageRay;

    private CharacterVisualisationPanel playerSelectionImageNorth;
    private CharacterVisualisationPanel playerSelectionImageWest;
    private CharacterVisualisationPanel playerSelectionImageEast;
    private CharacterVisualisationPanel playerSelectionImageSouth;

    private CharacterVisualisationPanel[] allPlayerVisualisationImages;

    private CharacterVisualisationPanel _activePlayerSelectionImage;

    public CharacterVisualisationPanel ActivePlayerSelectionImage
    {
        get => _activePlayerSelectionImage;
        set
        {
            _activePlayerSelectionImage = value;



            if (ActivePlayerSelectionImage == playerVisualisationImageDolphin)
            {
                playerSelectionImageNorth = playerVisualisationImageDolphin;
                playerSelectionImageWest = playerVisualisationImageOrca;
                playerSelectionImageEast = playerVisualisationImageShark;
                playerSelectionImageSouth = playerVisualisationImageRay;
            }
            else if (ActivePlayerSelectionImage == playerVisualisationImageOrca)
            {
                playerSelectionImageNorth = playerVisualisationImageOrca;
                playerSelectionImageWest = playerVisualisationImageRay;
                playerSelectionImageEast = playerVisualisationImageDolphin;
                playerSelectionImageSouth = playerVisualisationImageShark;
            }
            else if (ActivePlayerSelectionImage == playerVisualisationImageShark)
            {
                playerSelectionImageNorth = playerVisualisationImageShark;
                playerSelectionImageWest = playerVisualisationImageDolphin;
                playerSelectionImageEast = playerVisualisationImageRay;
                playerSelectionImageSouth = playerVisualisationImageOrca;
            }
            else if (ActivePlayerSelectionImage == playerVisualisationImageRay)
            {
                playerSelectionImageNorth = playerVisualisationImageRay;
                playerSelectionImageWest = playerVisualisationImageShark;
                playerSelectionImageEast = playerVisualisationImageOrca;
                playerSelectionImageSouth = playerVisualisationImageDolphin;
            }

        }
    }



    void Start()
    {
        allPlayerVisualisationImages = new CharacterVisualisationPanel[] { playerVisualisationImageDolphin, playerVisualisationImageOrca, playerVisualisationImageShark, playerVisualisationImageRay };

        if (playerVisualisationImageDolphin == null)
        {
            throw new DolphinGameException("playerVisualisationImageDolphin is not set");
        }
        if (playerVisualisationImageOrca == null)
        {
            throw new DolphinGameException("playerVisualisationImageOrca is not set");
        }
        if (playerVisualisationImageShark == null)
        {
            throw new DolphinGameException("playerVisualisationImageShark is not set");
        }
        if (playerVisualisationImageRay == null)
        {
            throw new DolphinGameException("playerVisualisationImageRay is not set");
        }
    }


    void Update()
    {
        HandlePlayerVisualisationImagesPosition();
        HandlePlayerVisualisationImagesScale();
    }



    private void HandlePlayerVisualisationImagesScale()
    {
        GraduallyChangeImageSize(playerSelectionImageNorth.Image, new Vector2(100, 100));
        GraduallyChangeImageSize(playerSelectionImageWest.Image, new Vector2(50, 50));
        GraduallyChangeImageSize(playerSelectionImageEast.Image, new Vector2(50, 50));
        GraduallyChangeImageSize(playerSelectionImageSouth.Image, new Vector2(0, 0));
    }

    private void GraduallyChangeImageSize(RectTransform image, Vector2 size)
    {
        image.sizeDelta = Vector2.MoveTowards(image.sizeDelta, size, sizeChangeSpeed);
    }

    private void HandlePlayerVisualisationImagesPosition()
    {
        GraduallyChangePanelPosition(playerSelectionImageNorth.Panel, new Vector2(0.5f, -0.3f));
        GraduallyChangePanelPosition(playerSelectionImageWest.Panel, new Vector2(1.25f, 0.1f));
        GraduallyChangePanelPosition(playerSelectionImageEast.Panel, new Vector2(-0.25f, 0.1f));
        GraduallyChangePanelPosition(playerSelectionImageSouth.Panel, new Vector2(0.5f, 0.5f));
    }

    private void GraduallyChangePanelPosition(RectTransform panel, Vector2 pivot)
    {
        panel.pivot = Vector2.MoveTowards(panel.pivot, pivot, positionChangeSpeed);
    }
}
