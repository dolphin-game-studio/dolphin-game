using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiPatrouille : MonoBehaviour
{
    public Waypoint[] waypoints;
    private Vector3[] waypointPositions;

    public float speed = 1;
    public float roationSpeed = 1;

    private int current;
    private float pauseStartTime;
    private bool pause;

    private float currentPauseDuration;

    private Hai hai;

    void Start()
    {

        hai = GetComponent<Hai>();
       
        waypointPositions = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypointPositions[i] = waypoints[i].transform.position;
        }
    }

    void Update()
    {


        if (pause)
        {
            var timeWaited =  Time.time - pauseStartTime; 
            if(timeWaited > currentPauseDuration)
            {
                pause = false;
            }
        }
        else {
            if (transform.position != waypointPositions[current])
            {
                Vector3 pos = Vector3.MoveTowards(transform.position, waypointPositions[current], speed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(pos);
                Quaternion rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(waypointPositions[current] - transform.position, Vector3.up), roationSpeed * Time.deltaTime);
                GetComponent<Rigidbody>().MoveRotation(rotation);

            }
            else
            {
                pause = true;

                pauseStartTime = Time.time;
                currentPauseDuration = waypoints[current].pauseDuration;

                current = (current + 1) % waypointPositions.Length;
            }
        }


    }
}
