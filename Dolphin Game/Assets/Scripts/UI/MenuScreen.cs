using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuScreen : MonoBehaviour
{
    Animator animator;

    private bool _active = false;

    public bool Active
    {
        get => _active;

        set
        {
            if (_active != value)
            {
                _active = value;

                animator.SetBool("visible", _active);
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }
}
