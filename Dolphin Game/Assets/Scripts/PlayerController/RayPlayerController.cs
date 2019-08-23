using UnityEngine;

public class RayPlayerController : PlayerControllerBase
{
    void Awake()
    {
        base.Init();

        narrowCorridors = FindObjectsOfType<NarrowCorridor>();
    }

    void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;

        HandleSting();
        HandleNarrowCorridors();
    }

    #region HandleNarrowCorridors

    [SerializeField] private AudioSource narrowCorridorClip;

    NarrowCorridor[] narrowCorridors;

    private void HandleNarrowCorridors()
    {
        bool narrowCorridorButtonPressed = Input.GetButtonDown("Y Button");

        if (narrowCorridorButtonPressed)
        {
            float distanceToNearestNarrowCorridor;
            Vector3 fromPlayerToNarrowCorridorVector;

            NarrowCorridor nearestFacingNarrowCorridor = GetNearestFacingNarrowCorridor(out distanceToNearestNarrowCorridor, out fromPlayerToNarrowCorridorVector);
            if (nearestFacingNarrowCorridor != null && distanceToNearestNarrowCorridor < 15)
            {
                if (nearestFacingNarrowCorridor.RayCanSlipThrough) {
                    this.transform.position = nearestFacingNarrowCorridor.OtherNarrowCorridor.OutputPosition;
                    narrowCorridorClip.Play();
                }
            }
        }
    }
    #endregion

    #region HandleSting

    [SerializeField] private AudioSource rayStingClip;

    int stingHash = Animator.StringToHash("Sting");

    private void HandleSting()
    {
        bool stingButtonPressed = Input.GetButtonDown("X Button");

        if (stingButtonPressed)
        {
            animator.SetTrigger(stingHash);
            rayStingClip.Play();

            float distanceToNearestFacingShark;
            Vector3 fromPlayerToSharkVector;

            Hai nearestFacingShark = GetNearestFacingShark(out distanceToNearestFacingShark, out fromPlayerToSharkVector);
            if (nearestFacingShark != null && distanceToNearestFacingShark < 15)
            {
                nearestFacingShark.IsStunned = true;
            }
        }
    }
    #endregion

    #region GetNearestFacingNarrowCorridor
    protected NarrowCorridor GetNearestFacingNarrowCorridor(out float distanceToNearestFacingNarrowCorridor, out Vector3 fromPlayerToNearestFacingNarrowCorridorVector)
    {
        NarrowCorridor nearestNarrowCorridor = null;
        float nearestNarrowCorridorDistance = float.MaxValue;
        fromPlayerToNearestFacingNarrowCorridorVector = Vector3.zero;

        foreach (var narrowCorridor in narrowCorridors)
        {
            if (narrowCorridor.RayCanSlipThrough) {
                var fromPlayerToNarrowCorridorVector = narrowCorridor.transform.position - transform.position;

                var dotProdToNarrowCorridor = Vector3.Dot(fromPlayerToNarrowCorridorVector.normalized, transform.forward);

                bool facingTheNarrowCorridor = dotProdToNarrowCorridor > 0;

                if (facingTheNarrowCorridor)
                {
                    var distanceToNarrowCorridor = Vector3.Distance(narrowCorridor.transform.position, transform.position);
                    if (distanceToNarrowCorridor < nearestNarrowCorridorDistance)
                    {
                        nearestNarrowCorridorDistance = distanceToNarrowCorridor;
                        nearestNarrowCorridor = narrowCorridor;
                        fromPlayerToNearestFacingNarrowCorridorVector = fromPlayerToNarrowCorridorVector;
                    }
                }
            }
        }

        distanceToNearestFacingNarrowCorridor = nearestNarrowCorridorDistance;
        return nearestNarrowCorridor;
    }

    #endregion
}
