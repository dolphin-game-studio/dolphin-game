using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuControler : MonoBehaviour
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

    public void ContinueGame()
    {
        Debug.Log("ContinueGame");
    }

    public void OpenControlsScreen()
    {
         screenControler.ActivateScreen(screenControler.ControlsScreen);
    }


    public void QuitGame()
    {
        Debug.Log("QuitGame");
        screenControler.DeactivateCurrentScreen();
    }
}
