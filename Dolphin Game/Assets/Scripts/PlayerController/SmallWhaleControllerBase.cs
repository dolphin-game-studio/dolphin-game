﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallWhaleControllerBase : PlayerControllerBase
{
    [SerializeField] private Transform eccoOrigin;

    public Transform EccoOrigin { get => eccoOrigin; }

    protected Jammer[] allJammer;
    protected EccoEffect eccoEffect;


    protected override void Init()
    {
        base.Init();

        allJammer = FindObjectsOfType<Jammer>();
        eccoEffect = FindObjectOfType<EccoEffect>();
    }



    public List<Jammer> GetJammerInReach()
    {
        List<Jammer> jammerInReach = new List<Jammer>(); ;

        for (int i = 0; i < allJammer.Length; i++)
        {
            var jammer = allJammer[i];
            if (jammer.IsNotHacked)
            {
                var distanceToJammer = Vector3.Distance(transform.position, jammer.transform.position);

                if (distanceToJammer < jammer.Radius)
                {
                    jammerInReach.Add(jammer);
                }
            }
        }
        return jammerInReach;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("X Button"))
        {
            var jammerInReach = GetJammerInReach();

            eccoEffect.StartEcho(new Echo() { Type = jammerInReach.Count > 0 ? EchoType.JammedEcho :  EchoType.Echo, Origin = eccoOrigin.position });

            if (jammerInReach.Count > 0)
            {
                for (int i = 0; i < jammerInReach.Count; i++)
                {
                    var jammer =  jammerInReach[i];
                    eccoEffect.StartEcho(new Echo() { Type = jammerInReach.Count > 0 ? EchoType.JammedEcho :  EchoType.Echo, Origin = jammer.transform.position });
                }
            }
        }
    }
}