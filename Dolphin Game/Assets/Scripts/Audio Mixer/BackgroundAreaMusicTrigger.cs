using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundAreaMusicTrigger : MonoBehaviour
{
    [SerializeField] private bool _deactivateOnTrigger;

    [SerializeField] private AudioMixerSnapshot triggerSnapshot;
 
    void OnTriggerEnter(Collider other)
    {
        triggerSnapshot.TransitionTo(5f);

        if (_deactivateOnTrigger)
        {
            gameObject.SetActive(false);
        }
    }
}
