using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinPlayerController : SmallWhaleControllerBase
{
    #region Cooldowns

    [SerializeField] private float _bubbleCooldown;
    private float _timeSinceLastBubble;

    public float BubbleAbilityCooldown => Mathf.Clamp01(_timeSinceLastBubble / _bubbleCooldown);

    public bool BubbleAbilityCooldownFinished => BubbleAbilityCooldown == 1;


    #endregion


    public GameObject bubblePrefab;
    int bubbleHash = Animator.StringToHash("Bubble");
    private bool bubbleSheduled = false;
    public float bubbleDelay = 1f;
    private float timeLeftUntilBubble = 0;

    public float bubbleThrustPower = 1f;



    void Awake()
    {
        base.Init();
    }

    #region hacking

    private Jammer _nearestJammer = null;
    private float _distanceToNearestJammer = float.MaxValue;
    private Vector3 _fromPlayerToNearestFacingJammerVector = Vector3.zero;

    [SerializeField] private float _maxHackDistance = 10;

    [SerializeField] private AudioSource hackingClip;


    public void SendHackEcho()
    {
        eccoEffect.StartEcho(new Echo() { Type = EchoType.HackEcho, Origin = _nearestJammerInHackDistance.transform.position });
        eccoEffect.StartEcho(new Echo() { Type = EchoType.HackEcho, Origin = EccoOrigin.position });
    }

    public Jammer GetNearestFacingJammerInHackDistance(out float distanceToNearestFacingJammer, out Vector3 fromPlayerToNearestFacingJammerVector)
    {
        Jammer nearestJammer = null;
        fromPlayerToNearestFacingJammerVector = Vector3.zero;
        distanceToNearestFacingJammer = float.MaxValue;

        for (int i = 0; i < allJammer.Length; i++)
        {
            var jammer = allJammer[i];

            if (jammer.IsNotHacked)
            {
                var fromPlayerToJammerVector = jammer.transform.position - transform.position;
                var dotProdToJammer = Vector3.Dot(fromPlayerToJammerVector.normalized, transform.forward);

                bool facingTheJammer = dotProdToJammer > 0;

                if (facingTheJammer)
                {
                    var distanceToJammer = Vector3.Distance(transform.position, jammer.transform.position);

                    if (distanceToJammer < _maxHackDistance)
                    {
                        if (nearestJammer == null || distanceToJammer < distanceToNearestFacingJammer)
                        {
                            nearestJammer = jammer;
                            distanceToNearestFacingJammer = distanceToJammer;
                            fromPlayerToNearestFacingJammerVector = fromPlayerToJammerVector;
                        }
                    }
                }
            }
        }


        return nearestJammer;
    }

     public float HackProgress => Map(hackEchoDelay, initialHackEchoDelay, hackEchoDelayEnd, 0, 1);


    public static float Map(float x, float x1, float x2, float y1, float y2)
    {
        var m = (y2 - y1) / (x2 - x1);
        var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

        return m * x + c;
    }

    public float hackEchoDelayMultiplier = 0.8f; // 1 1 2 3 5 8
    public float hackEchoDelayEnd = 0.1f; // 1 1 2 3 5 8

    public float initialHackEchoDelay = 3f; // 1 1 2 3 5 8
    private float hackEchoDelay;
    private float timeSinceLastHackEcho;

    private bool _hackingInProgress = false;
    private bool _hackingFinished = false;


    public bool AtLeastOneJammerInHackDistance => _nearestJammerInHackDistance != null;

    public bool HackingInProgress
    {
        get => _hackingInProgress;
        set
        {
            if (_hackingInProgress != value)
            {
                _hackingInProgress = value;

                if (_hackingInProgress) {
                    hackingClip.Play();
                }

                if (!_hackingInProgress && !_hackingFinished) {
                    hackingClip.Stop();
                }
            }
        }
    }

    private Jammer _nearestJammerInHackDistance = null;
    private float _distanceToNearestFacingJammer;
    private Vector3 _fromPlayerToJammerVector;



    private void HandleHacking()
    {
        _nearestJammerInHackDistance = GetNearestFacingJammerInHackDistance(out _distanceToNearestFacingJammer, out _fromPlayerToJammerVector);

        bool yButtonDown = Input.GetButtonDown("Y Button");
        bool yButtonUp = Input.GetButtonUp("Y Button");


        if (yButtonDown)
        {
            if (_nearestJammerInHackDistance != null)
            {
                HackingInProgress = true;
                _hackingFinished = false;
 
                hackEchoDelay = initialHackEchoDelay;
                SendHackEcho();
            }
        }
        if (yButtonUp || _nearestJammerInHackDistance == null)
        {
            HackingInProgress = false;
        }


        if (_hackingInProgress)
        {

            if (timeSinceLastHackEcho > hackEchoDelay)
            {
                SendHackEcho();

                hackEchoDelay *= hackEchoDelayMultiplier;
                timeSinceLastHackEcho = 0;
                if (hackEchoDelay < hackEchoDelayEnd)
                {
                    _hackingFinished = true;

                    HackingInProgress = false;
                    _nearestJammerInHackDistance.IsHacked = true;

                }
            }
            timeSinceLastHackEcho += Time.deltaTime;




        }
    }
    #endregion
    protected override void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;


        HandleHacking();
        HandlBubble();

        base.Update();
    }

    #region Bubble

    [SerializeField] private AudioSource bubbleClip;


    private void HandlBubble()
    {
        bool bButtonPressed = Input.GetButtonUp("B Button");

        if (bButtonPressed)
        {

            if (BubbleAbilityCooldownFinished)
            {
                _animator.SetTrigger(bubbleHash);
                SheduleBubble();
            }



        }

        if (bubbleSheduled)
        {
            timeLeftUntilBubble -= Time.deltaTime;

            if (timeLeftUntilBubble < 0)
            {
                ShotBubble();

                bubbleSheduled = false;
            }
        }

        _timeSinceLastBubble += Time.deltaTime;

    }

    private void SheduleBubble()
    {
        bubbleSheduled = true;
        bubbleClip.Play();

        timeLeftUntilBubble = bubbleDelay;
    }

    private void ShotBubble()
    {
        var bubble = Instantiate(bubblePrefab);
        bubble.transform.position = EccoOrigin.transform.position;
        var bubbleRigidBody = bubble.GetComponent<Rigidbody>();
        bubbleRigidBody.velocity = transform.forward * bubbleThrustPower;

        _timeSinceLastBubble = 0;


    }
    #endregion

}
