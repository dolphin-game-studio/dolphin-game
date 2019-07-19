using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkPlayerController : PlayerControllerBase
{

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

        Rank = rank;
    }



    void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;


        bool rPressed = Input.GetKeyUp(KeyCode.R);

        if (rPressed)
        {
            float distanceToNearestFacingShark;
            Vector3 fromPlayerToSharkVector;

            Hai nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);
            if (nearestFacingShark != null && nearestFacingShark.KnockedOut && distanceToNearestFacingShark < 10)
            {
                if (Rank < nearestFacingShark.Rank)
                {
                    Rank = nearestFacingShark.Rank;
                }

                nearestFacingShark.Rank = 0;

            }
            else if (nearestFacingShark != null && !nearestFacingShark.Stunned && !nearestFacingShark.KnockedOut && distanceToNearestFacingShark < 20) {
                nearestFacingShark.Distract(this);
            }
        }



    }





}
