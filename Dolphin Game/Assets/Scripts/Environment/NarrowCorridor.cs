using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrowCorridor : MonoBehaviour
{
    public bool RayCanSlipThrough => findable == null ? true : findable.Found;

    private Findable findable;

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

    [SerializeField] private ParticleSystem particleSystem;


    void Start()
    {
        if (OtherNarrowCorridor == null)
        {
            throw new DolphinGameException("OtherNarrowCorridor is not set on NarrowCorridor");
        }

        if (particleSystem == null)
        {
            throw new DolphinGameException("foundParticleSystem is not set on NarrowCorridor");
        }

        OutputPosition = outputTransform.position;

        findable = GetComponent<Findable>();
        if (findable == null) {
            particleSystem.Play();
        }
    }

    void Update()
    {
        if (findable != null)
        {
            if (findable.Found) {
                if (particleSystem.isStopped) {
                    particleSystem.Play();
                }
            } else {
                if (particleSystem.isPlaying)
                {
                    particleSystem.Stop();
                }
            }
        }
    }
}
