using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenControler : ScreenControler
{
    [SerializeField] private MenuScreen pauseScreen;
    public MenuScreen PauseScreen => pauseScreen;

    [SerializeField] private MenuScreen controlsScreen;
    public MenuScreen ControlsScreen => controlsScreen;

    public MenuScreen[] allScreens => new MenuScreen[] { pauseScreen, controlsScreen };

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
    }

    public override void Start()
    {
        base.Start();
    }

    #region Visible

    private bool _visible;
    public bool Visible
    {
        get => _visible;
        set
        {
            if (_visible != value)
            {
                _visible = value;

                ActivateScreen(pauseScreen);

                if (_visible)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }

    public bool NotVisible { get => !Visible; set => Visible = !value; }

    #endregion

    public override void Update()
    {
        base.Update();

        bool startButtonPressed = Input.GetButtonDown("Start Button");

        if (startButtonPressed)
        {
            if (screenHistory.Contains(pauseScreen))
            {
                DeactivateScreensUntil(pauseScreen);
            }
            else {
                ActivateScreen(pauseScreen);
            }
        }
    }
}
