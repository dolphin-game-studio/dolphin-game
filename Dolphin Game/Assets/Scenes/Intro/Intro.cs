using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class Intro : MonoBehaviour
{
    VideoPlayer videoPlayer;


    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += IntroCompleted;
    }

    private void IntroCompleted(VideoPlayer source)
    {
        LoadStartScreen();
    }

    private void LoadStartScreen()
    {
        SceneManager.LoadSceneAsync("Start Menu", LoadSceneMode.Single);

    }

    private void Update()
    {
        bool bButtonPressed = Input.GetButtonUp("B Button");

        if (bButtonPressed)
        {
            LoadStartScreen();
        }
    }
}
