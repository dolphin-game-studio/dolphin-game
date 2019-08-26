using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundAreaMusicTrigger : MonoBehaviour
{
 
    [SerializeField] private AudioMixerSnapshot triggerSnapshot;
 
    void Awake()
    {
 
    }

    void Update()
    {
     }

    void OnTriggerEnter(Collider other)
    {
 
        triggerSnapshot.TransitionTo(1f);

        
    }
}
