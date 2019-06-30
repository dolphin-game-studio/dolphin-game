using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinPlayerController : MonoBehaviour
{
    public Rigidbody Rigidbody { get; set; }
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float maxRotationDegreesDelta = 0.1f;

    [SerializeField] private bool funnyMoveMent = false;


    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        if (Rigidbody == null)
        {
            Debug.LogError("Rigidbody Component is not set on DolphinPlayerController");
        }

        if (playerController == null)
        {
            Debug.LogError("PlayerController is not set on DolphinPlayerController");
        }
    }

    void Update()
    {


    }

    public void Move(float horizontal, float vertical)
    {
        var movement = new Vector3(horizontal, vertical);
        Rigidbody.velocity = movement;

        if (funnyMoveMent)
        {
            transform.Rotate(movement);
        }
        if (movement == Vector3.zero)
        {
            movement = Vector3.forward;
        }
        var lookRotation = Quaternion.LookRotation(movement);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, maxRotationDegreesDelta);

    }
}
