using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenuScreen : MenuScreen
{
     [SerializeField] private AudioMixerSnapshot silenceSnapshot;
    private PauseScreenControler pauseScreenControler;

 
    public override bool InHistory
    {
        get => base.InHistory;

        set
        {
            if (base.InHistory != value)
            {
                base.InHistory = value;

                if (base.InHistory)
                {
                    Time.timeScale = 0f;
                }
                else {
                    Time.timeScale = 1f;
                }
            }
        }
    }


    public override void Awake()
    {
        base.Awake();

        pauseScreenControler = FindObjectOfType<PauseScreenControler>();
        if (pauseScreenControler == null)
        {
            throw new DolphinGameException("There is no pauseScreenControler Component in this Scene.");
        }

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
        OnFadedOut += LoadStartScreen;
    }

    private void LoadStartScreen()
    {
        SceneManager.LoadSceneAsync("Start Menu", LoadSceneMode.Single);
        OnFadedOut -= LoadStartScreen;

    }
}
