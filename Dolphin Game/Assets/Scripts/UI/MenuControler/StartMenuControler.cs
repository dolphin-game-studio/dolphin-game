using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartMenuControler : MenuControler
{
    [SerializeField] private AudioMixerSnapshot silenceSnapshot;
    [SerializeField] private AudioMixerSnapshot menuSnapshot;


    private StartScreenControler screenControler;

    public override void Awake()
    {
        base.Awake();

        screenControler = FindObjectOfType<StartScreenControler>();
        if (screenControler == null)
        {
            throw new DolphinGameException("There is no screenControler Component in this Scene.");
        }
    }

    public override void Start()
    {
        base.Start();

        menuSnapshot.TransitionTo(1);
    }

    public override void Update()
    {
        base.Update();
    }

    public void StartGame()
    {
        silenceSnapshot.TransitionTo(2);
        screenControler.ActivateScreen(screenControler.LoadingScreen);
    }

    public void PlayIntro()
    {
        silenceSnapshot.TransitionTo(1);
        screenControler.DeactivateCurrentScreen();

        SceneManager.LoadSceneAsync("Intro Scene", LoadSceneMode.Additive);
    }

    public void OpenControlsScreen()
    {
        screenControler.ActivateScreen(screenControler.ControlsScreen);
    }

    public void OpenCreditsScreen()
    {
        screenControler.ActivateScreen(screenControler.CreditsScreen);
    }

    public void QuitGame()
    {
        silenceSnapshot.TransitionTo(2);
        Application.Quit();
    }
}
