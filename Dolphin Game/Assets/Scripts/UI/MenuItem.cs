using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class MenuItem : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private UnityEvent pressedEvent;

    private bool _selected;

    public bool Selected
    {
        get => _selected;
        set
        {
            if (_selected != value) {
                _selected = value;

                animator.SetBool("selected", Selected);
            }
        }
    }


    void Awake()
    {
        animator = GetComponent<Animator>();

        if (pressedEvent == null) {
            throw new DolphinGameException("pressedEvent is not set");
        }
    }

    public void Press() {
        pressedEvent.Invoke();
        animator.SetTrigger("pressed");
    }
}
