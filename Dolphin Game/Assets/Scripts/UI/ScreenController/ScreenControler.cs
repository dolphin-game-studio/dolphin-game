using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ScreenControler : MonoBehaviour
{

    protected List<MenuScreen> screenHistory = new List<MenuScreen>();

    public List<MenuScreen> ScreenHistory { get => screenHistory; }

    private bool _active;

    public bool Active
    {
        get => _active;
        set
        {
            if (_active != value)
            {
                _active = value;
            }
        }
    }

    public void ActivateScreen(MenuScreen screen)
    {
        if (screenHistory.Count > 0)
        {
            screenHistory[screenHistory.Count - 1].Active = false;
        }

        screenHistory.Add(screen);

        screenHistory[screenHistory.Count - 1].Active = true;
    }

    public void DeactivateCurrentScreen()
    {
        screenHistory[screenHistory.Count - 1].Active = false;

        screenHistory.RemoveAt(screenHistory.Count - 1);

        if (screenHistory.Count > 0)
        {
            screenHistory[screenHistory.Count - 1].Active = true;
        }
    }

    public void DeactivateScreensUntil(MenuScreen screen)
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