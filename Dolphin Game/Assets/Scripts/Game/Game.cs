using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Game : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot standardSnapshot;


    public GameObject spottedScreen;

    private bool _spotted;

    public bool Spotted
    {
        get => _spotted;

        set
        {
            _spotted = value;
            if (_spotted) {
                spottedScreen.SetActive(true);
            }
        }
    }

    public void Awake()
    {
    }

    public void Start()
    {
        standardSnapshot.TransitionTo(1f);
    }

    public void Update()
    {

    }
}
