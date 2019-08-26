using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameScreen))]
public class CloseableScreen : MonoBehaviour
{
    private GameScreen menuScreen;
    private ScreenControler screenControler;


    void Awake()
    {
        menuScreen = GetComponent<GameScreen>();

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
