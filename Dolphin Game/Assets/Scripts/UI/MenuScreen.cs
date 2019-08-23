using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuScreen : MonoBehaviour
{
    Animator animator;

    public delegate void FadedOut();
    public event FadedOut OnFadedOut;

    private bool _fadeOutComplete;
    public bool FadeOutComplete { get => _fadeOutComplete; set => _fadeOutComplete = value; }


    private bool _fadeComplete;
    public bool FadeComplete { get => _fadeComplete; set => _fadeComplete = value; }

    private bool _active = false;

    public bool Active
    {
        get => _active && FadeComplete;

        set
        {
            if (_active != value)
            {
                _active = value;

                animator.SetBool("visible", _active);

                if (!_active) {
                    _fadeComplete = false;
                }
            }
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>(); 
    }
     

    void Update()
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
