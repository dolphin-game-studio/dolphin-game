using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkPlayerController : PlayerControllerBase
{


    #region Ranks
    [Range(0, 3)]
    public int rank = 0;

    public GameObject[] armbands;

    public int Rank
    {
        get
        {
            return rank;
        }
        set
        {
            if (rank != 0)
                armbands[rank - 1].SetActive(false);

            rank = value;

            if (rank != 0)
                armbands[rank - 1].SetActive(true);
        }
    }
    #endregion 

    void Awake()
    {
        base.Init();

        anglers = FindObjectsOfType<Angler>();

        Rank = rank;
    }

    void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;

        HandleTakeAngler();

        HandleTakeUniform();

        HandleDistractShark();
    }

    #region Distract Shark

    [SerializeField] private float _maxDinstanceToTalkToShark = 20;

    [SerializeField] private AudioSource distractSharkClip;

    private float distanceToNearestFacingShark;
    private Vector3 fromPlayerToSharkVector;
    private Hai nearestFacingShark;

    public bool SharkToDistractInReach => nearestFacingShark != null
        && nearestFacingShark.IsNotStunned
        && nearestFacingShark.IsNotKnockedOut
        && distanceToNearestFacingShark < _maxDinstanceToTalkToShark
        && Rank >= nearestFacingShark.Rank;

    private void HandleDistractShark()
    {
        nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);

        bool xButtonPressed = Input.GetButtonUp("X Button");

        if (xButtonPressed)
        {
            if (SharkToDistractInReach)
            {
                nearestFacingShark.Distract(this);
                distractSharkClip.Play();
            }
        }
    }
    #endregion

    #region Take Uniform

    public bool KnockedOutSharkInReach => nearestFacingKnockedOutShark != null && distanceToNearestFacingKnockedOutShark < 10;
    public bool KnockedOutSharkWithSuperiorUniformInReach => KnockedOutSharkInReach && nearestFacingKnockedOutShark.Rank > Rank;

    private float distanceToNearestFacingKnockedOutShark;
    private Vector3 fromPlayerToKnockedOutSharkVector;
    private Hai nearestFacingKnockedOutShark;

    [SerializeField] private AudioSource takeUniformClip;

    private void HandleTakeUniform()
    {
        nearestFacingKnockedOutShark = GetNearestFacingShark(out distanceToNearestFacingKnockedOutShark, out fromPlayerToKnockedOutSharkVector, searchForKnockedOutShark: true);

        bool yButtonPressed = Input.GetButtonUp("Y Button");

        if (yButtonPressed)
        {
            if (KnockedOutSharkWithSuperiorUniformInReach)
            {
                Rank = nearestFacingKnockedOutShark.Rank;
                takeUniformClip.Play();
                nearestFacingKnockedOutShark.Rank = 0;
            }
        }
    }
    #endregion

    #region Take Angler

    private Angler _nearestAngler;
    private float _distanceToNearestFacingAngler;
    private Vector3 _fromPlayerToNearestFacingAnglerVector;

    public bool AnglerInReach => _nearestAngler != null && _distanceToNearestFacingAngler < _distanceToTakeAngler;

    [SerializeField] private AudioSource _takeAnglerClip;

    [SerializeField] private float _distanceToTakeAngler;
    protected Angler[] anglers;
    private Angler _followingAngler;

    private void HandleTakeAngler()
    {
        _nearestAngler = GetNearestFacingAngler(out _distanceToNearestFacingAngler, out _fromPlayerToNearestFacingAnglerVector);

        bool bButtonPressed = Input.GetButtonUp("B Button");

        if (bButtonPressed)
        {
            if (_followingAngler == null)
            {
                if (AnglerInReach)
                {
                    _nearestAngler.SharkToFollow = this.gameObject;
                    if (_nearestAngler.SharkToFollow == this.gameObject)
                    {
                        _followingAngler = _nearestAngler;
                        _takeAnglerClip.Play();
                    }
                }
            }
            else if (_followingAngler != null)
            {
                _followingAngler.SharkToFollow = null;
                _followingAngler = null;
            }
        }
    }

    protected Angler GetNearestFacingAngler(out float distanceToNearestFacingAngler, out Vector3 fromPlayerToNearestFacingAnglerVector)
    {
        Angler nearestAngler = null;
        float nearestAnglerDistance = float.MaxValue;
        fromPlayerToNearestFacingAnglerVector = Vector3.zero;

        foreach (var angler in anglers)
        {
            if (angler.SharkToFollow != null)
                continue;

            var fromPlayerToAnglerVector = angler.transform.position - transform.position;

            var dotProdToAngler = Vector3.Dot(fromPlayerToAnglerVector.normalized, transform.forward);

            bool facingTheAngler = dotProdToAngler > 0;

            if (facingTheAngler)
            {
                var distanceToAngler = Vector3.Distance(angler.transform.position, transform.position);
                if (distanceToAngler < nearestAnglerDistance)
                {
                    nearestAnglerDistance = distanceToAngler;
                    nearestAngler = angler;
                    fromPlayerToNearestFacingAnglerVector = fromPlayerToAnglerVector;
                }
            }
        }

        distanceToNearestFacingAngler = nearestAnglerDistance;
        return nearestAngler;
    }

    #endregion

}
