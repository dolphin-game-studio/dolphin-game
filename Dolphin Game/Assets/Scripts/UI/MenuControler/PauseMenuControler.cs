using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MenuScreen))]
public class PauseMenuControler : MenuControler
{
    private MenuScreen _menuScreen;
    [SerializeField] private AudioMixerSnapshot silenceSnapshot;
    private PauseScreenControler pauseScreenControler;

    public override void Awake()
    {
        base.Awake();

        pauseScreenControler = FindObjectOfType<PauseScreenControler>();
        if (pauseScreenControler == null)
        {
            throw new DolphinGameException("There is no pauseScreenControler Component in this Scene.");
        }

        _menuScreen = GetComponent<MenuScreen>();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public void ContinueGame()
    {
        pauseScreenControler.DeactivateScreensUntil(pauseScreenControler.PauseScreen);
    }

    public void OpenControlsScreen()
    {
        pauseScreenControler.ActivateScreen(pauseScreenControler.ControlsScreen);
    }


    public void QuitGame()
    {
        pauseScreenControler.DeactivateCurrentScreen();
        silenceSnapshot.TransitionTo(0.5f);
        _menuScreen.OnFadedOut += LoadStartScreen;
    }

    private void LoadStartScreen()
    {
        SceneManager.LoadSceneAsync("Start Menu", LoadSceneMode.Single);
    }
}
