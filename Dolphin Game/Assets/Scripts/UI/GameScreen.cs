using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GameScreen : MonoBehaviour
{
    Animator animator;

    public delegate void FadedOut();
    public static event FadedOut OnFadedOut;
 
    private bool _fadeOutComplete;
    public bool FadeOutComplete { get => _fadeOutComplete; set => _fadeOutComplete = value; }


    private bool _fadeComplete;
    public bool FadeComplete { get => _fadeComplete; set => _fadeComplete = value; }



    public virtual bool NotInHistory { get => !InHistory; set => InHistory = !value; }


    private bool _inHistory = false;

    public virtual bool InHistory
    {
        get => _inHistory;

        set
        {
            if (_inHistory != value)
            {
                _inHistory = value;
            }
        }
    }

    private bool _active = false;

    public virtual bool Active
    {
        get => _active;

        set
        {
            if (_active != value)
            {
                _active = value;

                animator.SetBool("visible", _active);

                if (!_active)
                {
                    _fadeComplete = false;
                }
            }
        }
    }

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {

    }

    public void OnFadeInComplete()
    {
        _fadeComplete = true;
    }

    public void OnFadeOutComplete()
    {
        OnFadedOut();
        _fadeComplete = true;
    }
}
