
 

using UnityEngine;

public class RayPlayerController : PlayerControllerBase
{

    int stingHash = Animator.StringToHash("Sting");


    void Start()
    {
        base.Init();



    }



    void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;


        bool stingButtonPressed = Input.GetButtonDown("X Button");

        if (stingButtonPressed)
        {
            animator.SetTrigger(stingHash);

            float distanceToNearestFacingShark;
            Vector3 fromPlayerToSharkVector;

            Hai nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);
            if (nearestFacingShark != null && distanceToNearestFacingShark < 15)
            {
                nearestFacingShark.Stunned = true;
            }
        }



    }





}
