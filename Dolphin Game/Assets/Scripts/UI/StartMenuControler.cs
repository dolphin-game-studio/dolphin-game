using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuControler : MonoBehaviour
{
    private ScreenControler screenControler;

    void Start()
    {
        screenControler = FindObjectOfType<ScreenControler>();
        if (screenControler == null)
        {
            throw new DolphinGameException("There is no screenControler Component in this Scene.");
        }
     }

    public void StartGame()
    {
        Debug.Log("StartGame");
    }

    public void PlayIntro()
    {
        Debug.Log("PlayIntro");
    }

    public void OpenControlsScreen()
    {
         screenControler.CurrentMenuScreen = screenControler.ControlsScreen;
    }

    public void OpenCreditsScreen()
    {
        screenControler.CurrentMenuScreen = screenControler.CreditsScreen;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
