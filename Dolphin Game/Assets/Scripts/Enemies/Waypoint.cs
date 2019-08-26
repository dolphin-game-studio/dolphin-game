using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Range(0, 60)]
    public float pauseDuration = 0;

    [Range(0, 360)]
    public float observationAngle = 0;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
