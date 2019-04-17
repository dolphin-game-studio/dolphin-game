using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinPlayerController : MonoBehaviour
{
    public Rigidbody Rigidbody { get; set; }
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxRotationDegreesDelta = 0.1f;


    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        if (Rigidbody == null)
        {
            Debug.LogError("Rigidbody Component is not set on DolphinPlayerController");
        }
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * speed;
        var vertical = Input.GetAxis("Vertical") * speed;

        var movement = new Vector3(horizontal, vertical);
        Rigidbody.velocity = movement;

        var lookRotation = Quaternion.LookRotation(movement);
        var eulerLookRotation =  lookRotation.eulerAngles;
        print(eulerLookRotation);

        if (eulerLookRotation.y < 0) {
            eulerLookRotation.y = 360 + eulerLookRotation.y;
        }
        lookRotation = Quaternion.Euler(eulerLookRotation);
        //transform.Rotate(movement); funny

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, maxRotationDegreesDelta);


    }
}
