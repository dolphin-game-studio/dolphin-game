using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenControler : ScreenControler
{
    [SerializeField] private GameScreen loadingScreen;
    public GameScreen LoadingScreen => loadingScreen;

    [SerializeField] private GameScreen startScreen;
    public GameScreen StartScreen => startScreen;

    [SerializeField] private GameScreen controlsScreen;
    public GameScreen ControlsScreen => controlsScreen;

    [SerializeField] private GameScreen creditsScreen;
    public GameScreen CreditsScreen => creditsScreen;

    public GameScreen[] allScreens => new GameScreen[] { loadingScreen, startScreen, controlsScreen, creditsScreen };
     
    public override void Awake()
    {
        base.Awake();

        if (startScreen == null)
        {
            throw new DolphinGameException("startScreen is not set");
        }
        if (controlsScreen == null)
        {
            throw new DolphinGameException("controlsScreen is not set");
        }
        if (creditsScreen == null)
        {
            throw new DolphinGameException("creditsScreen is not set");
        }
    }

    public override void Start()
    {
        base.Start();
 
        ActivateScreen(startScreen);
    }

    public override void Update()
    {
        base.Update();
    }

}
