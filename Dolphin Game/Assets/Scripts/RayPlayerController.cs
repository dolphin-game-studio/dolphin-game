
using System.Linq;

using UnityEngine;

public class RayPlayerController : DolphinPlayerController
{
    Hai[] haie;

    int stingHash = Animator.StringToHash("Sting");


    void Start()
    {
        base.Init();

        haie = FindObjectsOfType<Hai>();


    }

    public Hai GetNearestFacingShark(out float distanceToNearestFacingShark)
    {
        Hai nearestShark = null;
        float nearestSharkDistance = float.MaxValue;
         
        foreach (var hai in haie)
        {
            var fromPlayerToSharkVector = hai.transform.position - transform.position;
             
            var dotProdToShark = Vector3.Dot(fromPlayerToSharkVector.normalized, transform.forward);

            bool facingTheShark = dotProdToShark > 0.5;

            if (facingTheShark)
            {
                var distanceToShark = Vector3.Distance(hai.transform.position, transform.position);
                if (distanceToShark < nearestSharkDistance)
                {
                    nearestSharkDistance = distanceToShark;
                    nearestShark = hai;
                }
            }
        }

        distanceToNearestFacingShark = nearestSharkDistance;
        return nearestShark;
    }

    void Update()
    {



        bool rPressed = Input.GetKeyUp(KeyCode.R);

        if (rPressed)
        {
            animator.SetTrigger(stingHash);

            float distanceToNearestFacingShark;
            Hai nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark);
            if (nearestFacingShark != null && distanceToNearestFacingShark < 15)
            {
                nearestFacingShark.Unconscious = true;
            }
        }



    }



 
 
}
