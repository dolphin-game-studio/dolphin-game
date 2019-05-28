using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayerCharacter : MonoBehaviour
{
    Hai hai;
    bool noticedPlayer;
    float timePlayerNoticed;
    public float secondsToSpotPlayer = 5f;
   
    void Start()
    {
        hai = GetComponent<Hai>();
        if (hai == null)
        {
            Debug.LogError("SpotPlayerCharacter must have a Hai component.");
        }

    }

    void Update()
    {
        if (secondsToSpotPlayer > timePlayerNoticed)
        {
            print("Spotted!");
        }
        else {
            print(timePlayerNoticed);
        }

        if (noticedPlayer) {
            timePlayerNoticed += Time.deltaTime;
        }

        if (hai.visibleTargets.Count > 0)
        {
            noticedPlayer = true;
        }
        else
        {
            noticedPlayer = false;
            timePlayerNoticed = 0;
        }
    }
}
