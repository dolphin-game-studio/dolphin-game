using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ScreenControler : MonoBehaviour
{

    protected List<GameScreen> screenHistory = new List<GameScreen>();

    public List<GameScreen> ScreenHistory { get => screenHistory; }
 

    public void ActivateScreen(GameScreen screen)
    {
        if (screenHistory.Count > 0)
        {
            screenHistory[screenHistory.Count - 1].Active = false;
        }

        screenHistory.Add(screen);

        screenHistory[screenHistory.Count - 1].Active = true;
        screenHistory[screenHistory.Count - 1].InHistory = true; 
    }

    public void DeactivateCurrentScreen()
    {
        screenHistory[screenHistory.Count - 1].Active = false;
        screenHistory[screenHistory.Count - 1].InHistory = false;

        screenHistory.RemoveAt(screenHistory.Count - 1);

        if (screenHistory.Count > 0)
        {
            screenHistory[screenHistory.Count - 1].Active = true;
        }
    }

    public void DeactivateScreensUntil(GameScreen screen)
    {
        while (screenHistory.Count > 0 && screenHistory[screenHistory.Count - 1] != screen)
        {
            DeactivateCurrentScreen();
        }
        DeactivateCurrentScreen();
    }

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

}