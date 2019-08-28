using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Game _game;

    [SerializeField] private AudioSource playerChangeClip;


    public PlayerControllerBase CurrentPlayerController
    {
        get => currentPlayerController;

        set
        {
            if (currentPlayerController != value)
            {
                currentPlayerController = value;

                playerChangeClip.Play();

                if (currentPlayerController == DolphinPlayerController)
                {
                    characterVisualisation.ActivePlayerSelectionImage = characterVisualisation.PlayerVisualisationImageDolphin;
                }
                else if (currentPlayerController == RayPlayerController)
                {
                    characterVisualisation.ActivePlayerSelectionImage = characterVisualisation.PlayerVisualisationImageRay;
                }
                else if (currentPlayerController == OrcaPlayerController)
                {
                    characterVisualisation.ActivePlayerSelectionImage = characterVisualisation.PlayerVisualisationImageOrca;
                }
                else if (currentPlayerController == SharkPlayerController)
                {
                    characterVisualisation.ActivePlayerSelectionImage = characterVisualisation.PlayerVisualisationImageShark;
                }
            }
        }
    }

    public bool DolphinIsActive => currentPlayerController == DolphinPlayerController;
    public bool RayIsActive => currentPlayerController == RayPlayerController;
    public bool SharkIsActive => currentPlayerController == SharkPlayerController;
    public bool OrcaIsActive => currentPlayerController == OrcaPlayerController;


    public DolphinPlayerController DolphinPlayerController => dolphinPlayerController;
    public RayPlayerController RayPlayerController => rayPlayerController;
    public SharkPlayerController SharkPlayerController => sharkPlayerController;
    public OrcaPlayerController OrcaPlayerController => orcaPlayerController;

    private PlayerControllerBase currentPlayerController;

    private DolphinPlayerController dolphinPlayerController;
    private RayPlayerController rayPlayerController;
    private SharkPlayerController sharkPlayerController;
    private OrcaPlayerController orcaPlayerController;

    private List<PlayerControllerBase> allPlayerControllers = new List<PlayerControllerBase>();
    private CharacterSelection characterSelection;
    private PauseMenuScreen _pauseMenuScreen;

    private CharacterVisualisation characterVisualisation;



    void Awake()
    {
        _game = FindObjectOfType<Game>();

        if (_game == null)
        {
            throw new DolphinGameException("Game Object couldn't be found.");
        }

        characterSelection = FindObjectOfType<CharacterSelection>();

        if (characterSelection == null)
        {
            throw new DolphinGameException("CharacterSelection Object couldn't be found.");
        }

        _pauseMenuScreen = FindObjectOfType<PauseMenuScreen>();

        if (_pauseMenuScreen == null)
        {
            throw new DolphinGameException("PauseMenuScreen Object couldn't be found.");
        }

        characterVisualisation = FindObjectOfType<CharacterVisualisation>();

        if (characterVisualisation == null)
        {
            throw new DolphinGameException("characterVisualisation Object couldn't be found.");
        }


        dolphinPlayerController = FindObjectOfType<DolphinPlayerController>();
        rayPlayerController = FindObjectOfType<RayPlayerController>();
        sharkPlayerController = FindObjectOfType<SharkPlayerController>();
        orcaPlayerController = FindObjectOfType<OrcaPlayerController>();

        if (dolphinPlayerController != null)
        {
            allPlayerControllers.Add(dolphinPlayerController);
        }
        if (rayPlayerController != null)
        {
            allPlayerControllers.Add(sharkPlayerController);
        }
        if (sharkPlayerController != null)
        {
            allPlayerControllers.Add(rayPlayerController);
        }
        if (orcaPlayerController != null)
        {
            allPlayerControllers.Add(orcaPlayerController);
        }

        if (allPlayerControllers.Count == 0)
        {
            throw new DolphinGameException("There is no dolphin, ray, orca or shark player controller in this scene. Please add at least one from the presets folder.");
        }
        else
        {
            CurrentPlayerController = allPlayerControllers[0];
        }
    }

    void Update()
    {
        if (characterSelection.NotVisible && !_pauseMenuScreen.InHistory && !_game.Spotted)
        {
            UpdateMovement();

            UpdatePlayerSelection();
        }
    }

    private void UpdatePlayerSelection()
    {
        bool selectPrevChar = Input.GetButtonDown("Left Bumper");
        bool selectNextChar = Input.GetButtonDown("Right Bumper");

        bool selectDolphin = Input.GetKeyUp(KeyCode.Alpha1);
        bool selectRay = Input.GetKeyUp(KeyCode.Alpha2);
        bool selectOrca = Input.GetKeyUp(KeyCode.Alpha3);
        bool selectShark = Input.GetKeyUp(KeyCode.Alpha4);


        if (selectDolphin)
        {
            CurrentPlayerController = dolphinPlayerController;
        }

        if (selectRay)
        {
            CurrentPlayerController = rayPlayerController;
        }

        if (selectOrca)
        {
            CurrentPlayerController = orcaPlayerController;
        }

        if (selectShark)
        {
            CurrentPlayerController = sharkPlayerController;
        }

        if (selectPrevChar)
        {
            var currentPlayerIndex = allPlayerControllers.IndexOf(CurrentPlayerController);

            do
            {
                currentPlayerIndex = (currentPlayerIndex - 1) % allPlayerControllers.Count;

                if (currentPlayerIndex < 0)
                {
                    currentPlayerIndex = allPlayerControllers.Count + currentPlayerIndex;
                }
            } while (!allPlayerControllers[currentPlayerIndex].IsPlayable);

            CurrentPlayerController = allPlayerControllers[currentPlayerIndex];
        }

        if (selectNextChar)
        {
            var currentPlayerIndex = allPlayerControllers.IndexOf(CurrentPlayerController);

            do
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % allPlayerControllers.Count;
            } while (!allPlayerControllers[currentPlayerIndex].IsPlayable);

            CurrentPlayerController = allPlayerControllers[currentPlayerIndex];
        }
    }

    private void UpdateMovement()
    {
        if (!CurrentPlayerController.IsPlayable)
            return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        float playerCharacterSpeed = CurrentPlayerController.Speed;

        var swimFastButtonPressed = Input.GetAxis("A Button");

        if (swimFastButtonPressed > 0)
        {
            playerCharacterSpeed += CurrentPlayerController.FastSwimMultiplier * swimFastButtonPressed;
        }

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.4)
        {
            CurrentPlayerController.Move(horizontal * playerCharacterSpeed, vertical * playerCharacterSpeed);
        }
    }
}
