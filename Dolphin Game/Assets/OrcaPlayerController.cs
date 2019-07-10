using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaPlayerController : DolphinPlayerController
{

    int ramHash = Animator.StringToHash("Ram");
    public float ramForce = 1f;

    public float ramThrustDelay = 1f;


    private bool ramThrustSheduled = false;
    private bool _ramThrusting;
    private float timeLeftUntilRamThrust = 0;

    public float distanceToMakeSharkUnconscious = 5;

    public bool RamThrusting
    {
        get { return _ramThrusting; }
        set
        {
            _ramThrusting = value;

            if (_ramThrusting)
            {
                Rigidbody.AddForce(transform.forward * ramForce, ForceMode.Impulse);

            }
            else
            {
                Rigidbody.velocity = Vector3.zero;

            }

        }
    }

    void Start()
    {
        base.Init();



    }

    Hai nearestFacingShark;

    void Update()
    {
        if (playerController.currentDolphinPlayerController != this)
            return;


        bool rPressed = Input.GetKeyUp(KeyCode.R);

        if (rPressed)
        {
            animator.SetTrigger(ramHash);

            SheduleRamThrust();






        }

        if (ramThrustSheduled)
        {
            timeLeftUntilRamThrust -= Time.deltaTime;

            if (timeLeftUntilRamThrust < 0)
            {
                float distanceToNearestFacingShark;
                Vector3 fromPlayerToSharkVector;

                nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);

                transform.forward = fromPlayerToSharkVector;

                ramThrustSheduled = false;

                RamThrusting = true;
            }
        }

        if (RamThrusting && nearestFacingShark != null)
        {
 
        }




    }

    void OnCollisionEnter(Collision collision)
    {
        if (RamThrusting) {
            ContactPoint contact = collision.contacts[0];
 
            Debug.Log(collision.gameObject);

            Hai rammedShark = collision.gameObject.GetComponent<Hai>();
            if (rammedShark != null && rammedShark.IsNotAlarmed)
            {
                rammedShark.KnockedOut = true;

                RamThrusting = false;
            } 
          


        }

    }

    private void SheduleRamThrust()
    {
        ramThrustSheduled = true;
        timeLeftUntilRamThrust = ramThrustDelay;
    }
}