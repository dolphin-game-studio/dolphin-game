using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallWhaleControllerBase : PlayerControllerBase
{
    [SerializeField] private Transform eccoOrigin;

    public Transform EccoOrigin { get => eccoOrigin; }

    protected Jammer[] allJammer;
    protected EccoEffect eccoEffect;
    protected EccoEffectFindable eccoEffectFindable;
    protected EccoEffectJammed eccoEffectJammed;


    protected override void Init()
    {
        base.Init();

        allJammer = FindObjectsOfType<Jammer>();
        eccoEffect = FindObjectOfType<EccoEffect>();
        eccoEffectFindable = FindObjectOfType<EccoEffectFindable>();
        eccoEffectJammed = FindObjectOfType<EccoEffectJammed>();

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
        HandleEcho();
    }

    #region Handle Echo

    [SerializeField] private AudioSource[] echoClips;
    [SerializeField] private AudioSource[] echoJammedClips;


    private void HandleEcho()
    {
        if (Input.GetButtonDown("X Button"))
        {
            var jammerInReach = GetJammerInReach();

            if (jammerInReach.Count > 0)
            {
                eccoEffect.StartEcho(new Echo() { Type = EchoType.JammedEcho, Origin = eccoOrigin.position });
                echoJammedClips[Random.Range(0, echoJammedClips.Length - 1)].Play();

                for (int i = 0; i < jammerInReach.Count; i++)
                {
                    var jammer = jammerInReach[i];
                    eccoEffect.StartEcho(new Echo() { Type = jammerInReach.Count > 0 ? EchoType.JammedEcho : EchoType.Echo, Origin = jammer.transform.position });
                }
            }
            else
            {
                eccoEffect.StartEcho(new Echo() { Type = EchoType.Echo, Origin = eccoOrigin.position });
                eccoEffectFindable.StartEcho(new Echo() { Type = EchoType.Echo, Origin = eccoOrigin.position });
                echoClips[Random.Range(0, echoClips.Length - 1)].Play();
            }
        }
    }

    #endregion
}
