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


    private List<MenuScreen> screenHistory = new List<MenuScreen>();

    public List<MenuScreen> ScreenHistory { get => screenHistory; }

    public void ActivateScreen(MenuScreen screen)
    {
        if(screenHistory.Count > 0)
        {
            screenHistory[screenHistory.Count - 1].Active = false; ;
        }

        screenHistory.Add(screen);

        screenHistory[screenHistory.Count - 1].Active = true;
    }

    public void DeactivateCurrentScreen() {
        screenHistory[screenHistory.Count - 1].Active = false;

        screenHistory.RemoveAt(screenHistory.Count - 1);

        screenHistory[screenHistory.Count - 1].Active = true;
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

        ActivateScreen(startScreen);
    }

}
