using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkPlayerController : PlayerControllerBase
{
    [SerializeField] private float distanceToTakeAngler;

    protected Angler[] anglers;

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

    void Start()
    {
        base.Init();

        anglers = FindObjectsOfType<Angler>();
        
        Rank = rank;
    }

    #region Take Angler
    private Angler followingAngler;
    private void HandleTakeAngler()
    {
        bool bButtonPressed = Input.GetButtonUp("B Button");

        if (bButtonPressed)
        {
            if (followingAngler == null)
            {
                foreach (var angler in anglers)
                {
                    var fromPlayerToAnglerVector = angler.transform.position - transform.position;

                    var dotProdToAngler = Vector3.Dot(fromPlayerToAnglerVector.normalized, transform.forward);

                    bool facingTheAngler = dotProdToAngler > 0.5;

                    bool anglerInReach = fromPlayerToAnglerVector.magnitude < distanceToTakeAngler;
                    if (anglerInReach)
                    {
                        angler.SharkToFollow = this.gameObject;
                        if (angler.SharkToFollow == this.gameObject)
                        {
                            followingAngler = angler;
                        }
                    }
                }
            }
            else if (followingAngler != null)
            {
                followingAngler.SharkToFollow = null;
            }
        }


    }
    #endregion

    void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;

        HandleTakeAngler();



        bool rPressed = Input.GetKeyUp(KeyCode.R);

        if (rPressed)
        {
            float distanceToNearestFacingShark;
            Vector3 fromPlayerToSharkVector;

            Hai nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);
            if (nearestFacingShark != null && nearestFacingShark.IsKnockedOut && distanceToNearestFacingShark < 10)
            {
                if (Rank < nearestFacingShark.Rank)
                {
                    Rank = nearestFacingShark.Rank;
                }

                nearestFacingShark.Rank = 0;

            }
            else if (nearestFacingShark != null && !nearestFacingShark.Stunned && !nearestFacingShark.IsKnockedOut && distanceToNearestFacingShark < 20)
            {
                nearestFacingShark.Distract(this);
            }
        }



    }





}
