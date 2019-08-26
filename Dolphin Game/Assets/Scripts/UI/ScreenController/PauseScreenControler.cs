using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenControler : ScreenControler
{
    private CharacterSelection _characterSelection;

    [SerializeField] private GameScreen pauseScreen;
    public GameScreen PauseScreen => pauseScreen;

    [SerializeField] private GameScreen controlsScreen;
    public GameScreen ControlsScreen => controlsScreen;

    public GameScreen[] allScreens => new GameScreen[] { pauseScreen, controlsScreen };

    public override void Awake()
    {
        base.Awake();

        if (pauseScreen == null)
        {
            throw new DolphinGameException("pauseScreen is not set");
        }
        if (controlsScreen == null)
        {
            throw new DolphinGameException("controlsScreen is not set");
        }
        
        _characterSelection = FindObjectOfType<CharacterSelection>();
        if (_characterSelection == null)
        {
            throw new DolphinGameException("CharacterSelection couldn't be found.");
        }
    }

    public override void Start()
    {
        base.Start();
    }

 

    public override void Update()
    {
        base.Update();

        if (_characterSelection.NotVisible) {
            bool startButtonPressed = Input.GetButtonDown("Start Button");

            if (startButtonPressed)
            {
                if (screenHistory.Contains(pauseScreen))
                {
                    DeactivateScreensUntil(pauseScreen);
                }
                else
                {
                    ActivateScreen(pauseScreen);
                }
            }
        } 
    }
}
