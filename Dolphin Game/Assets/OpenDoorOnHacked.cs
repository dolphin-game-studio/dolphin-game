using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Jammer))]
public class OpenDoorOnHacked : MonoBehaviour
{
    private Jammer _jammer;
    [SerializeField] private Animator _doorAnimator;

    void Awake()
    {
        _jammer = GetComponent<Jammer>();
    }

    void Update()
    {
        if (_jammer.IsHacked)
        {
            _doorAnimator.SetTrigger("open");
            enabled = false;
        }
    }
}
