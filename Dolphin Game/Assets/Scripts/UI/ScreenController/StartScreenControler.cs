using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenControler : ScreenControler
{
    [SerializeField] private MenuScreen loadingScreen;
    public MenuScreen LoadingScreen => loadingScreen;

    [SerializeField] private MenuScreen startScreen;
    public MenuScreen StartScreen => startScreen;

    [SerializeField] private MenuScreen controlsScreen;
    public MenuScreen ControlsScreen => controlsScreen;

    [SerializeField] private MenuScreen creditsScreen;
    public MenuScreen CreditsScreen => creditsScreen;

    public MenuScreen[] allScreens => new MenuScreen[] { loadingScreen, startScreen, controlsScreen, creditsScreen };
     
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
