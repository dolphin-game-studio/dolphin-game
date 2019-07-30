using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaPlayerController : SmallWhaleControllerBase
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

    public override bool CanMove
    {
        get
        {
            return RamThrusting == false && ramThrustSheduled == false;
        }
    }

    void Start()
    {
        base.Init();



    }


    #region Handle Ram Thrust
    [SerializeField] private float timeToBeAbleToMoveAgainAfterRam = 1f;
    private float timeSinceRamStarted = 0f;



    Hai nearestFacingShark;
    float distanceToNearestFacingShark;
    Vector3 fromPlayerToSharkVector;

    private void HandleRamThrust()
    {
        bool bButtonPressed = Input.GetButtonUp("B Button");

        if (bButtonPressed)
        {
            nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);

            if (nearestFacingShark != null)
            {
                SheduleRamThrust();
            }
        }

        if (ramThrustSheduled)
        {
            timeLeftUntilRamThrust -= Time.deltaTime;

            if (timeLeftUntilRamThrust < 0)
            {
                nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);

                transform.forward = fromPlayerToSharkVector;

                ramThrustSheduled = false;

                RamThrusting = true;
            }
        }
        if (RamThrusting)
        {
            timeSinceRamStarted += Time.deltaTime;

            if (timeSinceRamStarted > timeToBeAbleToMoveAgainAfterRam)
            {
                timeSinceRamStarted = 0;
                RamThrusting = false;
            }
        }
    }
    #endregion Handle Ram Thrust


    protected override void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;

        base.Update();

        HandleRamThrust();
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (RamThrusting)
        {
            ContactPoint contact = collision.contacts[0];

            Debug.Log(collision.gameObject);

            Hai rammedShark = collision.gameObject.GetComponent<Hai>();
            if (rammedShark != null && rammedShark.IsNotAlarmed)
            {
                rammedShark.KnockedOut = true;
            }
        }

    }

    private void SheduleRamThrust()
    {
        animator.SetTrigger(ramHash);

        ramThrustSheduled = true;
        timeLeftUntilRamThrust = ramThrustDelay;
    }
}