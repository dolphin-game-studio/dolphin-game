using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class SpottedScreen : MonoBehaviour
{
    private Game _game;

    private VideoPlayer _videoPlayer;
    [SerializeField] private AudioSource _spottedBackgroundMusic;
    [SerializeField] private AudioMixerSnapshot _audioMixerSnapshot;


    public void Awake()
    {
        _game = FindObjectOfType<Game>();

        if (_game == null)
        {
            throw new DolphinGameException("Game Object couldn't be found.");
        }

        _videoPlayer = GetComponent<VideoPlayer>();
    }

    public void Update()
    {
        if (_game.Spotted)
        {
            bool startButtonPressed = Input.GetButtonDown("Start Button");

            if (startButtonPressed)
            {

                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

            }
        }
    }

    internal void PlaySpottedVideo()
    {
        _videoPlayer.Play();
        _spottedBackgroundMusic.Play();
        _audioMixerSnapshot.TransitionTo(0.1f);
    }
}
