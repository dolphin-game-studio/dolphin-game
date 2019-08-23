using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinPlayerController : SmallWhaleControllerBase
{
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

    [SerializeField] private AudioSource hackingClip;
 
    public float hackDistance;

    public void SendHackEcho()
    {
        eccoEffect.StartEcho(new Echo() { Type = EchoType.HackEcho, Origin = nearestJammerInHackDistance.transform.position });
        eccoEffect.StartEcho(new Echo() { Type = EchoType.HackEcho, Origin = EccoOrigin.position });
        hackingClip.Play();
    }

        public Jammer GetNearestJammerInHackDistance()
    {
        Jammer nearestJammer = null;
        float distanceToNearestJammer = float.MaxValue;

        for (int i = 0; i < allJammer.Length; i++)
        {
            var jammer = allJammer[i];
            if (jammer.IsNotHacked)
            {
                var distanceToJammer = Vector3.Distance(transform.position, jammer.transform.position);

                if (distanceToJammer < hackDistance)
                {
                    if (nearestJammer == null || distanceToJammer < distanceToNearestJammer)
                    {
                        nearestJammer = jammer;
                    }
                }
            }
        }
        return nearestJammer;
    }

    public float hackEchoDelayMultiplier = 0.8f; // 1 1 2 3 5 8
    public float hackEchoDelayEnd = 0.1f; // 1 1 2 3 5 8

    public float initialHackEchoDelay = 3f; // 1 1 2 3 5 8
    private float hackEchoDelay;
    private float timeSinceLastHackEcho;

    private bool hackingInProgress = false;
    private Jammer nearestJammerInHackDistance = null;



    private void HandleHacking()
    {
        bool yButtonDown = Input.GetButtonDown("Y Button");
        bool yButtonUp = Input.GetButtonUp("Y Button");


        if (yButtonDown)
        {
            nearestJammerInHackDistance = GetNearestJammerInHackDistance();
            if (nearestJammerInHackDistance != null)
            {

                hackingInProgress = true;
                hackEchoDelay = initialHackEchoDelay;
                SendHackEcho();
            }
        }
        if (yButtonUp)
        {

            hackingInProgress = false;
        }


        if (hackingInProgress)
        {

            if (timeSinceLastHackEcho > hackEchoDelay)
            {
                SendHackEcho();

                hackEchoDelay *= hackEchoDelayMultiplier;
                timeSinceLastHackEcho = 0;
                if (hackEchoDelay < hackEchoDelayEnd)
                {
                    hackingInProgress = false;
                    nearestJammerInHackDistance.IsHacked = true;

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

        bool bButtonPressed = Input.GetButtonUp("B Button");

        if (bButtonPressed)
        {
            animator.SetTrigger(bubbleHash);
            SheduleBubble();

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

        base.Update();
    }

    private void SheduleBubble()
    {
        bubbleSheduled = true;
        timeLeftUntilBubble = bubbleDelay;
    }

    private void ShotBubble()
    {
        var bubble = Instantiate(bubblePrefab);
        bubble.transform.position = EccoOrigin.transform.position;
        var bubbleRigidBody = bubble.GetComponent<Rigidbody>();
        bubbleRigidBody.velocity = transform.forward * bubbleThrustPower;
    }

}
