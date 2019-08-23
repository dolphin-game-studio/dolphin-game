using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angler : MonoBehaviour
{
    [SerializeField] private GameObject sharkToFollow;

    [SerializeField] private float roationSpeed;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Vector3 offsetToShark;


    void Awake()
    {
        SharkToFollow = sharkToFollow;
    }

    public GameObject SharkToFollow
    {
        get => sharkToFollow;
        set => sharkToFollow = value;
    }

    void Update()
    {
        if (sharkToFollow != null)
        {
            FollowShark();
        }
    }

    private void FollowShark()
    {
        RotateToDesiredRotation();
        MoveToDesiredPosition();

        var hai = sharkToFollow.GetComponent<Hai>();

        if (hai != null)
        {
            if (hai.IsKnockedOut)
            {
                sharkToFollow = null;
            }
        }

    }

    private void RotateToDesiredRotation()
    {
        var desiredRotation = sharkToFollow.transform.rotation;

        if (transform.rotation != desiredRotation)
        {
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, roationSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
    }

    private void MoveToDesiredPosition()
    {
        var desiredPosition = sharkToFollow.transform.position + offsetToShark;

        if (transform.position != desiredPosition)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            transform.position = pos;
        }

    }
}
