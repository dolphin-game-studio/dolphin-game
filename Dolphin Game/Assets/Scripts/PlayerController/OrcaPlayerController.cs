using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaPlayerController : SmallWhaleControllerBase
{
    public override bool CanMove => RamThrusting == false && ramThrustSheduled == false && IsNotSwimmingToSharkToTransport;

    DestructableObstacle[] destructables;

    void Start()
    {
        base.Init();

        destructables = FindObjectsOfType<DestructableObstacle>();

    }

    protected override void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;

        base.Update();

        HandleRamThrust();

        HandleTransportKnockedOutSharks();

    }



    #region Transport Knocked Out Sharks

    [SerializeField] private GameObject mouthSpine;
    [SerializeField] private float rotateSharkInMouthSpeed;
    [SerializeField] private float moveSharkInMouthSpeed;


    private bool _isTransportingShark;
    public bool IsTransportingShark { get => _isTransportingShark; set => _isTransportingShark = value; }
    public bool IsNotTransportingShark { get => !_isTransportingShark; set => _isTransportingShark = !value; }

    private bool _isSwimmingToSharkToTransport;
    public bool IsSwimmingToSharkToTransport { get => _isSwimmingToSharkToTransport; set => _isSwimmingToSharkToTransport = value; }
    public bool IsNotSwimmingToSharkToTransport { get => !_isSwimmingToSharkToTransport; set => _isSwimmingToSharkToTransport = !value; }

    private Hai currentlyTransportedShark;
    private Vector3 currentlyTransportedSharkDesiredPosition = new Vector3(-0.6f,1.1f,0.2f);
    private Quaternion currentlyTransportedSharkDesiredRotation = Quaternion.Euler(-90,-90,0);

    private Transform currentlyTransportedSharkParent;


    private void HandleTransportKnockedOutSharks()
    {
        bool yButtonPressed = Input.GetButtonUp("Y Button");

        if (yButtonPressed)
        {
            if (IsNotTransportingShark)
            {
                nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);

                if (nearestFacingShark != null && nearestFacingShark.IsKnockedOut)
                {
                    IsTransportingShark = true;

                    currentlyTransportedShark = nearestFacingShark;

                    IsSwimmingToSharkToTransport = true;
                }
            }
            else if (IsTransportingShark)
            {
                IsNotTransportingShark = true;

                currentlyTransportedShark.transform.SetParent(currentlyTransportedSharkParent);
                currentlyTransportedShark = null;
                currentlyTransportedSharkParent = null;
            }
        }

        if (IsSwimmingToSharkToTransport) {

            var distanceToShark = Vector3.Distance(mouthSpine.transform.position, currentlyTransportedShark.BackFin.transform.position);
            //var distanceToShark = Vector3.Distance(transform.position, currentlyTransportedShark.BackFin.transform.position);

            var centerToMouth = mouthSpine.transform.position - transform.position;
            var fromMouthToFin = currentlyTransportedShark.BackFin.transform.position - mouthSpine.transform.position;
            //Vector3 pos = Vector3.MoveTowards(transform.position, currentlyTransportedShark.BackFin.transform.position, Speed * Time.deltaTime);
            Vector3 pos = Vector3.MoveTowards(transform.position, currentlyTransportedShark.BackFin.transform.position - centerToMouth, Speed * Time.deltaTime);
             
            Debug.Log(mouthSpine.transform.position + " " + currentlyTransportedShark.BackFin.transform.position + " " + pos + " mouth to fin: " + fromMouthToFin + "distance mouth to fin: " + distanceToShark);
            GetComponent<Rigidbody>().MovePosition(pos);

            if (distanceToShark < 1) {
                IsNotSwimmingToSharkToTransport = true;

                currentlyTransportedSharkParent = currentlyTransportedShark.transform.parent;
                currentlyTransportedShark.transform.SetParent(mouthSpine.transform);
            }
        }

        if (IsTransportingShark && IsNotSwimmingToSharkToTransport) {
            //if (currentlyTransportedShark.transform.rotation != currentlyTransportedSharkDesiredRotation)
            //{
            //  Quaternion rotation = Quaternion.RotateTowards(currentlyTransportedShark.transform.rotation, currentlyTransportedSharkDesiredRotation, currentlyTransportedShark.roationSpeed * Time.deltaTime);
            Quaternion rotation = Quaternion.RotateTowards(currentlyTransportedShark.transform.localRotation, currentlyTransportedSharkDesiredRotation, rotateSharkInMouthSpeed * Time.deltaTime);
            currentlyTransportedShark.transform.localRotation = rotation; //.GetComponent<Rigidbody>().MoveRotation(rotation);
                                                                                                          //}
                                                                                                          //if (currentlyTransportedShark.transform.position != currentlyTransportedSharkDesiredPosition)
                                                                                                          //{
                                                                                                          //Vector3 pos = Vector3.MoveTowards(currentlyTransportedShark.transform.position, currentlyTransportedSharkDesiredPosition, Time.deltaTime);
                                                                                                          //currentlyTransportedShark.GetComponent<Rigidbody>().MovePosition(pos);

            Vector3 pos = Vector3.MoveTowards(currentlyTransportedShark.transform.localPosition, currentlyTransportedSharkDesiredPosition, moveSharkInMouthSpeed  * Time.deltaTime);
            currentlyTransportedShark.transform.localPosition = pos;
            //}
        }
    }
 

    #endregion  Transport Knocked Out Sharks


    #region Handle Ram Thrust

    int ramHash = Animator.StringToHash("Ram");
    public float ramForce = 1f;

    public float ramThrustDelay = 1f;

    private bool ramThrustSheduled = false;
    private bool _ramThrusting;
    private float timeLeftUntilRamThrust = 0;

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

    [SerializeField] private float ramThrustDuration = 1f;
    private float timeSinceRamStarted = 0f;

    Hai nearestFacingShark;
    float distanceToNearestFacingShark;
    Vector3 fromPlayerToSharkVector;
    
    DestructableObstacle nearestFacingDestructable;
    float distanceToNearestFacingDestructable;
    Vector3 fromPlayerToDestructableVector;

    private void SheduleRamThrust()
    {
        animator.SetTrigger(ramHash);

        ramThrustSheduled = true;
        timeLeftUntilRamThrust = ramThrustDelay;
    }

    private void HandleRamThrust()
    {
        bool bButtonPressed = Input.GetButtonUp("B Button");

        if (bButtonPressed)
        {
            nearestFacingDestructable = GetNearestFacingDestructable(out distanceToNearestFacingDestructable, out fromPlayerToDestructableVector);
            
            if (nearestFacingDestructable != null)
            {
                SheduleRamThrust();
            }
            else {
                nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);
                
                if (nearestFacingShark != null)
                {
                    SheduleRamThrust();
                }
            }

            
        }

        if (ramThrustSheduled)
        {
            timeLeftUntilRamThrust -= Time.deltaTime;

            if (timeLeftUntilRamThrust < 0)
            {
                if (nearestFacingDestructable != null)
                {
                    transform.forward = fromPlayerToDestructableVector;

                    ramThrustSheduled = false;
                    RamThrusting = true;
                }
                else
                {
                    if (nearestFacingShark != null)
                    {
                        transform.forward = fromPlayerToSharkVector;

                        ramThrustSheduled = false;
                        RamThrusting = true;
                    }
                }
            }
        }
        if (RamThrusting)
        {
            timeSinceRamStarted += Time.deltaTime;

            if (timeSinceRamStarted > ramThrustDuration)
            {
                timeSinceRamStarted = 0;
                RamThrusting = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (RamThrusting)
        {
            ContactPoint contact = collision.contacts[0];

            Hai rammedShark = collision.gameObject.GetComponent<Hai>();
            if (rammedShark != null && rammedShark.IsNotAlarmed)
            {
                rammedShark.IsKnockedOut = true;
            }

            DestructableObstacle rammedDestructable = collision.gameObject.GetComponent<DestructableObstacle>();
            if (rammedDestructable != null)
            {
                rammedDestructable.Destroy();
            }
        }

    }
    #endregion Handle Ram Thrust


    #region GetNearestFacingDestructable

    protected DestructableObstacle GetNearestFacingDestructable(out float distanceToNearestFacingDestructable, out Vector3 fromPlayerToNearestFacingDestructableVector)
    {
        DestructableObstacle nearestDestructable = null;
        float nearestDestructableDistance = float.MaxValue;
        fromPlayerToNearestFacingDestructableVector = Vector3.zero;

        foreach (var destructable in destructables)
        {
            if (destructable.OrcaCanDestroy) {
                var fromPlayerToDestructableVector = destructable.transform.position - transform.position;

                var dotProdToDestructable = Vector3.Dot(fromPlayerToDestructableVector.normalized, transform.forward);

                bool facingTheDestructable = dotProdToDestructable > 0;

                if (facingTheDestructable)
                {
                    var distanceToDestructable = Vector3.Distance(destructable.transform.position, transform.position);
                    if (distanceToDestructable < nearestDestructableDistance)
                    {
                        nearestDestructableDistance = distanceToDestructable;
                        nearestDestructable = destructable;
                        fromPlayerToNearestFacingDestructableVector = fromPlayerToDestructableVector;
                    }
                }
            }
        }

        distanceToNearestFacingDestructable = nearestDestructableDistance;
        return nearestDestructable;
    }

    #endregion
}