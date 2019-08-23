using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer), typeof(Animator))]
public class FadeInVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private Animator animator;

    private bool videoLoaded = false;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (!videoLoaded && videoPlayer.isPrepared)
        {
            animator.SetTrigger("loaded");
            videoLoaded = true;
        }
        else if (videoLoaded)
        {
            Destroy(this);
        }


    }
}
