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
                current = (current + 1) % waypointPositions.Length;
            }
            // Hinfahrt 15 - 18:25 ? -30m
            // 1:30 - 3:30
            // if (transform.position != waypointPositions[current])
            // 10:26 - 14:10
            //{

            var observationDir = new Vector3(Mathf.Cos(waypoints[current].observationAngle * Mathf.Deg2Rad), Mathf.Sin(waypoints[current].observationAngle * Mathf.Deg2Rad), 0);

          //  Vector3 observationDir = (Vector3)(Quaternion.Euler(0, 0, waypoints[current].observationAngle) * Vector3.right);

            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(observationDir, Vector3.up), roationSpeed * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(rotation);
           // }
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
            }
        }


    }
}
