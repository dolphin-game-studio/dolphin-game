
using System.Linq;

using UnityEngine;

public class RayPlayerController : DolphinPlayerController
{

    int stingHash = Animator.StringToHash("Sting");


    void Start()
    {
        base.Init();



    }

    

    void Update()
    {
        if (playerController.currentDolphinPlayerController != this)
            return;


        bool rPressed = Input.GetKeyUp(KeyCode.R);

        if (rPressed)
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
