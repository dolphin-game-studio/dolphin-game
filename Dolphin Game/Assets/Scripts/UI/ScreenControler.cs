using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControler : MonoBehaviour
{
    [SerializeField] private MenuScreen startScreen;
    public MenuScreen StartScreen => startScreen;

    [SerializeField] private MenuScreen pauseScreen;
    public MenuScreen PauseScreen => pauseScreen;

    [SerializeField] private MenuScreen controlsScreen;
    public MenuScreen ControlsScreen => controlsScreen;

    [SerializeField] private MenuScreen creditsScreen;
    public MenuScreen CreditsScreen => creditsScreen;

    private MenuScreen currentMenuScreen;
    public MenuScreen CurrentMenuScreen
    {
        get => currentMenuScreen;
        set
        {

            if (currentMenuScreen != value)
            {
                if (currentMenuScreen != null)
                {
                    currentMenuScreen.Active = false;
                }

                currentMenuScreen = value;

                currentMenuScreen.Active = true;
            }
        }
    }



    void Start()
    {
        if (startScreen == null)
        {
            throw new DolphinGameException("startScreen is not set");
        }
        if (pauseScreen == null)
        {
            throw new DolphinGameException("pauseScreen is not set");
        }
        if (controlsScreen == null)
        {
            throw new DolphinGameException("controlsScreen is not set");
        }
        if (creditsScreen == null)
        {
            throw new DolphinGameException("creditsScreen is not set");
        }

        CurrentMenuScreen = startScreen;
    }

}
