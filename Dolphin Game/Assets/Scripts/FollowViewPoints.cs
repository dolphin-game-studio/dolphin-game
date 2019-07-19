using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowViewPoints : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Transform[] targets;
    public float speed;
    public float roationSpeed;
    Hai hai;

    private int current;

    void Start()
    {
        hai = GetComponent<Hai>();
        if (hai == null)
        {
            throw new DolphinGameException("FollowViewPoints must have a Hai component.");
        }

        if (targets.Length < 1)
        {
            throw new DolphinGameException("FollowViewPoints targets should not be empty");
        }

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            throw new DolphinGameException("FollowViewPoints rigidbody should not be null");
        }
    }

    void Update()
    {
        if (hai.visibleTargets.Count > 0)
        {
            var visiblePlayer = hai.visibleTargets[0];

            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(visiblePlayer.transform.position - transform.position, Vector3.up), roationSpeed * 1.5f * Time.deltaTime);
            if (transform.rotation.eulerAngles != rotation.eulerAngles)
            {
                _rigidbody.MoveRotation(rotation);
            }
        }
        else {
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targets[current].position - transform.position, Vector3.up), roationSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles != rotation.eulerAngles)
            {
                _rigidbody.MoveRotation(rotation);
            }
            else
            {
                current = (current + 1) % targets.Length;
            }
        }

   
    }
}
