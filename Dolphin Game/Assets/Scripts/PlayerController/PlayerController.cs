using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerControllerBase CurrentPlayerController { get => currentPlayerController; set => currentPlayerController = value; }

    private PlayerControllerBase currentPlayerController;
     
    private DolphinPlayerController dolphinPlayerController;
    private RayPlayerController rayPlayerController;
    private SharkPlayerController sharkPlayerController;
    private OrcaPlayerController orcaPlayerController;

    private List<PlayerControllerBase> allPlayerControllers = new List<PlayerControllerBase>();
 


    void Start()
    {
        dolphinPlayerController = FindObjectOfType<DolphinPlayerController>();
        rayPlayerController = FindObjectOfType<RayPlayerController>();
        sharkPlayerController = FindObjectOfType<SharkPlayerController>();
        orcaPlayerController = FindObjectOfType<OrcaPlayerController>();

        if (dolphinPlayerController != null) {
            allPlayerControllers.Add(dolphinPlayerController);
        }
        if (rayPlayerController != null)
        {
            allPlayerControllers.Add(rayPlayerController);
        }
        if (sharkPlayerController != null)
        {
            allPlayerControllers.Add(sharkPlayerController);
        }
        if (orcaPlayerController != null)
        {
            allPlayerControllers.Add(orcaPlayerController);
        }

        if (allPlayerControllers.Count == 0)
        {
            throw new DolphinGameException("There is no dolphin, ray, orca or shark player controller in this scene. Please add at least one from the presets folder.");
        }
        else {
            currentPlayerController = allPlayerControllers[0];
        }
    }

    void Update()
    {
        UpdateMovement();

        UpdatePlayerSelection();
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
            currentPlayerController = dolphinPlayerController;
        }

        if (selectRay)
        {
            currentPlayerController = rayPlayerController;

        }

        if (selectOrca)
        {
            currentPlayerController = orcaPlayerController;

        }

        if (selectShark)
        {
            currentPlayerController = sharkPlayerController;
        }

        if (selectPrevChar)
        {
            var currentPlayerIndex = allPlayerControllers.IndexOf(currentPlayerController);

 
            currentPlayerIndex = (currentPlayerIndex - 1) % allPlayerControllers.Count;


            if (currentPlayerIndex < 0)
            {
                currentPlayerIndex = allPlayerControllers.Count + currentPlayerIndex;
            } 
            currentPlayerController = allPlayerControllers[currentPlayerIndex];
        }

        if (selectNextChar)
        {
            var currentPlayerIndex = allPlayerControllers.IndexOf(currentPlayerController);

            currentPlayerIndex = (currentPlayerIndex + 1) % allPlayerControllers.Count;
            currentPlayerController = allPlayerControllers[currentPlayerIndex];
        }
    }

    private void UpdateMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        float playerCharacterSpeed = currentPlayerController.Speed;

        var swimFastButtonPressed = Input.GetAxis("A Button");

        if (swimFastButtonPressed > 0)
        {
            playerCharacterSpeed += currentPlayerController.FastSwimMultiplier * swimFastButtonPressed;
        }

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.4)
        {
             currentPlayerController.Move(horizontal * playerCharacterSpeed, vertical * playerCharacterSpeed);
        }
    }
}
