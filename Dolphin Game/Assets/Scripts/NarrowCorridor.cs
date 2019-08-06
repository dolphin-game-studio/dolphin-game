using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrowCorridor : MonoBehaviour
{

    [SerializeField] private NarrowCorridor otherNarrowCorridor;

    public NarrowCorridor OtherNarrowCorridor { get => otherNarrowCorridor; set => otherNarrowCorridor = value; }
    public Vector3 OutputPosition
    {
        get => outputPosition;
        set
        {
            outputPosition = new Vector3(value.x, value.y, 0);
        }
    }

    private Vector3 outputPosition;
    [SerializeField] private Transform outputTransform;

    #region Found
    private HashSet<Echo> echosThatFoundTheCorridor = new HashSet<Echo>();

    [SerializeField] private float timeToStopFoundParticles = 10f;
    private float timeSinceFoundParticleSystemIsPlaying = 0;

    public ParticleSystem foundParticleSystem;

    private bool found = false;

    public bool Found
    {
        get => found;
        set
        {
            if (found != value)
            {
                found = value;

                if (found)
                {
                    timeSinceFoundParticleSystemIsPlaying = 0;
                    otherNarrowCorridor.Found = true;

                    foundParticleSystem.Play();
                }
                else
                {
                    foundParticleSystem.Stop();
                    timeSinceFoundParticleSystemIsPlaying = 0;
                }
            }
        }
    }

    public void FoundByEcho(Echo echo)
    {
        bool isNewEcho = !echosThatFoundTheCorridor.Contains(echo);
        if (isNewEcho)
        {
            echosThatFoundTheCorridor.Add(echo);
            Found = true;
        }
    }
    #endregion

    void Start()
    {
        if (OtherNarrowCorridor == null)
        {
            throw new DolphinGameException("OtherNarrowCorridor is not set on NarrowCorridor");
        }

        if (foundParticleSystem == null)
        {
            throw new DolphinGameException("foundParticleSystem is not set on NarrowCorridor");
        }

        OutputPosition = outputTransform.position;
    }

    void Update()
    {
        if (foundParticleSystem.isPlaying)
        {
            timeSinceFoundParticleSystemIsPlaying += Time.deltaTime;

            if (timeSinceFoundParticleSystemIsPlaying > timeToStopFoundParticles)
            {
                Found = false;
            }
        }
    }
}
