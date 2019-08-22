using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MenuScreen))]
public class CloseableScreen : MonoBehaviour
{
    private MenuScreen menuScreen;
    private ScreenControler screenControler;


    void Start()
    {
        menuScreen = GetComponent<MenuScreen>();

        screenControler = FindObjectOfType<ScreenControler>();
        if (screenControler == null)
        {
            throw new DolphinGameException("There is no screenControler Component in this Scene.");
        }
    }

    void Update()
    {
        if (menuScreen.Active)
        {
            bool bButtonPressed = Input.GetButtonUp("B Button");

            if (bButtonPressed)
            {
                screenControler.DeactivateCurrentScreen();
            }
        }
    }
}
