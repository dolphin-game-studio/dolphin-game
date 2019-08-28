using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    private Game _game;

    private PlayerController playerController;
    private DolphinGameCamera dolphinGameCamera;
    private PauseMenuScreen _pauseMenuScreen;

    
    [SerializeField] private GameObject characterSelectionPanels;

    [SerializeField] private bool roation;



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

                characterSelectionPanels.SetActive(_visible);

                if (_visible)
                {
                    Time.timeScale = 0.1f;
                }
                else
                {
                    Time.timeScale = 1f;

                    playerController.DolphinPlayerController.Achieved = false;
                    playerController.SharkPlayerController.Achieved = false;
                    playerController.OrcaPlayerController.Achieved = false;
                    playerController.RayPlayerController.Achieved = false;


                }
            }
        }
    }

    public bool NotVisible { get => !Visible; set => Visible = !value; }

    #endregion

    [SerializeField] private float rotationMultiplier = 0.3f;


    [SerializeField] private Transform playerSelectionImageNorth;
    [SerializeField] private Transform playerSelectionImageWest;
    [SerializeField] private Transform playerSelectionImageEast;
    [SerializeField] private Transform playerSelectionImageSouth;

    private Transform[] allPlayerSelectionImages;

    private Transform _selectedPlayerSelectionImage;

    public Transform SelectedPlayerSelectionImage
    {
        get => _selectedPlayerSelectionImage;
        set
        {
            _selectedPlayerSelectionImage = value;
            resetPlayerSelection();

            _selectedPlayerSelectionImage.localScale = new Vector3(selectedPlayerImageSize, selectedPlayerImageSize, 1f);
        }
    }


    [SerializeField] private float selectedPlayerImageSizeMultiplier = 1.2f;
    private int _currentPlayerIndex;

    private float selectedPlayerImageSize;

    void Awake()
    {
        _game = FindObjectOfType<Game>();

        if (_game == null)
        {
            throw new DolphinGameException("Game Object couldn't be found.");
        }

        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            throw new DolphinGameException("Player Controller Object couldn't be found.");
        }

        dolphinGameCamera = FindObjectOfType<DolphinGameCamera>();
        if (dolphinGameCamera == null)
        {
            throw new DolphinGameException("Dolphin Game Camera Object couldn't be found.");
        }

        _pauseMenuScreen = FindObjectOfType<PauseMenuScreen>();
        if (_pauseMenuScreen == null)
        {
            throw new DolphinGameException("PauseMenuScreen couldn't be found.");
        }
        

        allPlayerSelectionImages = new Transform[] { playerSelectionImageNorth, playerSelectionImageWest, playerSelectionImageEast, playerSelectionImageSouth };

        if (playerSelectionImageNorth == null)
        {
            throw new DolphinGameException("playerSelectionImageNorth is not set");
        }
        if (playerSelectionImageWest == null)
        {
            throw new DolphinGameException("playerSelectionImageWest is not set");
        }
        if (playerSelectionImageEast == null)
        {
            throw new DolphinGameException("playerSelectionImageEast is not set");
        }
        if (playerSelectionImageSouth == null)
        {
            throw new DolphinGameException("playerSelectionImageSouth is not set");
        }

        selectedPlayerImageSize = 1f * selectedPlayerImageSizeMultiplier;
    }

    public void Start()
    {
        Visible = true;
    }

    private void resetPlayerSelection()
    {
        foreach (var image in allPlayerSelectionImages)
        {
            image.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void Update()
    {
        if (_pauseMenuScreen.NotInHistory && !_game.Spotted)
        {
            bool backButtonPressed = Input.GetButtonDown("Back Button");

            if (backButtonPressed)
            {
                Visible = !Visible;
            }


            if (Visible)
            {
                HandleSelectCharacterByAnalogStick();

                HandleChoseCharacterByAButton();
            }
        }
    }

    private void HandleChoseCharacterByAButton()
    {
        bool aButtonPressed = Input.GetButtonDown("A Button");

        if (aButtonPressed)
        {
            if (SelectedPlayerSelectionImage == playerSelectionImageNorth)
            {
                playerController.CurrentPlayerController = playerController.DolphinPlayerController;
            }
            else if (SelectedPlayerSelectionImage == playerSelectionImageWest)
            {
                playerController.CurrentPlayerController = playerController.OrcaPlayerController;
            }
            else if (SelectedPlayerSelectionImage == playerSelectionImageEast)
            {
                playerController.CurrentPlayerController = playerController.SharkPlayerController;
            }
            else if (SelectedPlayerSelectionImage == playerSelectionImageSouth)
            {
                playerController.CurrentPlayerController = playerController.RayPlayerController;
            }

            NotVisible = true;
        }
    }

    private void HandleSelectCharacterByAnalogStick()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        bool analogStickIsNotDead = Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.5;

        if (analogStickIsNotDead)
        {
            var absHorizontal = Mathf.Abs(horizontal);
            var absVertical = Mathf.Abs(vertical);

            if (absHorizontal > absVertical)
            {
                if (horizontal > 0)
                    SelectedPlayerSelectionImage = playerSelectionImageEast;
                else
                    SelectedPlayerSelectionImage = playerSelectionImageWest;
            }
            else
            {
                if (vertical > 0)
                    SelectedPlayerSelectionImage = playerSelectionImageNorth;
                else
                    SelectedPlayerSelectionImage = playerSelectionImageSouth;
            }
        }

        if (roation)
        {
            var oppositeDirectionOfAnalogStick = new Vector3(-horizontal, -vertical, 0f);
            var newCharacterScreenNormal = oppositeDirectionOfAnalogStick * rotationMultiplier + Vector3.forward;
            this.transform.forward = newCharacterScreenNormal;
        }
    }
}
