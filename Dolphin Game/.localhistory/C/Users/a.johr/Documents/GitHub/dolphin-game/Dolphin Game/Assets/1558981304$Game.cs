using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Transform spottedScreen;

    private bool _spotted;

    public bool Spotted
    {
        get => _spotted;

        set
        {
            _spotted = value;
            if (_spotted) {

            }
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
