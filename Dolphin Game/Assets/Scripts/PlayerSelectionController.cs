using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionController : MonoBehaviour
{
    [SerializeField] private Transform[] playerSelectionImages;
    [SerializeField] private float selectedPlayerImageSizeMultiplier = 1.2f;
    private int _currentPlayerIndex;

    private float selectedPlayerImageSize;

    public int CurrentPlayerIndex
    {
        get => _currentPlayerIndex;
        set
        {
            if (value != _currentPlayerIndex)
            {
                resetPlayerSelection();
                _currentPlayerIndex = value;
                playerSelectionImages[_currentPlayerIndex - 1].localScale = new Vector3(selectedPlayerImageSize, selectedPlayerImageSize, 1f);
            }
        }
    }

    void Start()
    {
        if (playerSelectionImages.Length < 4)
        {
            throw new DolphinGameException("playerSelectionImages should be set with 4 player images but there are " + playerSelectionImages.Length);
        }

        selectedPlayerImageSize = 1f * selectedPlayerImageSizeMultiplier;
    }

    private void resetPlayerSelection()
    {
        foreach (var image in playerSelectionImages)
        {
            image.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        print(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        bool analogStickIsNotDead = Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.5;

        if (analogStickIsNotDead)
        {
            var absHorizontal = Mathf.Abs(horizontal);
            var absVertical = Mathf.Abs(vertical);

            if (absHorizontal > absVertical)
            {
                if (horizontal > 0)
                    CurrentPlayerIndex = 3;
                else
                    CurrentPlayerIndex = 2;
            }
            else
            {
                if (vertical > 0)
                    CurrentPlayerIndex = 1;
                else
                    CurrentPlayerIndex = 4;
            }
        }


    }
}
