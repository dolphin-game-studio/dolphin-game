using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Game : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot standardSnapshot;
    [SerializeField] private AudioMixerGroup _noticedPlayerGroup;

    [SerializeField] private AudioSource _noticedPlayerSound;

    private SpottedScreen _spottedScreen;

    private bool _spotted;

    public bool Spotted
    {
        get => _spotted;

        set
        {
            if (_spotted != value)
            {
                _spotted = value;

                if (_spotted)
                {
                    _spottedScreen.PlaySpottedVideo();
                }
            }


        }
    }

    bool _noticedPlayer = false;
    public bool NoticedPlayer
    {
        get => _noticedPlayer;
        set
        {
            if (_noticedPlayer != value)
            {
                _noticedPlayer = value;

                if (_noticedPlayer)
                {
                    _noticedPlayerSound.Play();
                    _noticedPlayerSound.volume  = 1f;

                }
                 
            }
        }
    }
 
    public void Awake()
    {
        _spottedScreen = FindObjectOfType<SpottedScreen>();
    }

    public void Start()
    {
        standardSnapshot.TransitionTo(1f);
    }

    public void Update()
    {
        if (!NoticedPlayer) {
            _noticedPlayerSound.volume -= 0.01f;
        }
    }
}
