using UnityEngine;

public class RayPlayerController : PlayerControllerBase
{
    void Awake()
    {
        base.Init();

        if (_rayRenderer == null)
        {
            throw new DolphinGameException("_rayRenderer is not set");
        }

        _visibleMaterial = _rayRenderer.material;

        _narrowCorridors = FindObjectsOfType<NarrowCorridor>();
    }

    protected override void Update()
    {
        base.Update();

        if (Rigidbody.velocity.magnitude > 1) {
            InvisibleToShark = false;
        }

        HandleSting();
        HandleNarrowCorridors();
    }

    #region Invisibility

    [SerializeField] private AudioSource _rayInvisibilityClip;


    private Material _visibleMaterial;
    [SerializeField] private Material _invisibleMaterial;

    [SerializeField] private Renderer _rayRenderer;


    private bool _invisibleToShark;
    public bool InvisibleToShark
    {
        get => _invisibleToShark; set
        {
            if (_invisibleToShark != value)
            {
                _invisibleToShark = value;

                if (_invisibleToShark)
                {
                    _rayRenderer.material = _invisibleMaterial;
                    _rayInvisibilityClip.Play();
                }
                else
                {
                    _rayRenderer.material = _visibleMaterial;
                }
            }

        }
    }
    #endregion

    #region HandleNarrowCorridors
    private NarrowCorridor[] _narrowCorridors;

    [SerializeField] private float maxDistanceToNarrowCorridor = 15;

    private NarrowCorridor _nearestFacingNarrowCorridor;
    private float _distanceToNearestNarrowCorridor;
    private Vector3 _fromPlayerToNarrowCorridorVector;

    public bool NarrowCorridorInReach =>
        _nearestFacingNarrowCorridor != null
        && _distanceToNearestNarrowCorridor < maxDistanceToNarrowCorridor
        && _nearestFacingNarrowCorridor.RayCanSlipThrough;

    [SerializeField] private AudioSource narrowCorridorClip;

    private void HandleNarrowCorridors()
    {
        _nearestFacingNarrowCorridor = GetNearestFacingNarrowCorridor(out _distanceToNearestNarrowCorridor, out _fromPlayerToNarrowCorridorVector);

        bool narrowCorridorButtonPressed = Input.GetButtonDown("Y Button 2");

        if (narrowCorridorButtonPressed)
        {
            if (NarrowCorridorInReach)
            {
                this.transform.position = _nearestFacingNarrowCorridor.OtherNarrowCorridor.OutputPosition;
                narrowCorridorClip.Play();
            }
        }
    }
    #endregion

    #region HandleSting

    private float _maxStingDistance = 15;


    private float _distanceToNearestFacingShark;
    private Vector3 _fromPlayerToSharkVector;
    private Hai _nearestFacingShark;

    public bool SharkToStunInReach =>
        _nearestFacingShark != null
        && _nearestFacingShark.IsNotKnockedOut
        && _distanceToNearestFacingShark < _maxStingDistance;

    [SerializeField] private AudioSource _rayStingClip;

    int stingHash = Animator.StringToHash("Sting");

    private void HandleSting()
    {
        _nearestFacingShark = GetNearestFacingShark(out _distanceToNearestFacingShark, out _fromPlayerToSharkVector);

        bool stingButtonPressed = Input.GetButtonDown("X Button 2");

        if (stingButtonPressed)
        {
            _animator.SetTrigger(stingHash);
            _rayStingClip.Play();

            if (SharkToStunInReach)
            {
                _nearestFacingShark.IsStunned = true;
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

        foreach (var narrowCorridor in _narrowCorridors)
        {
            if (narrowCorridor.RayCanSlipThrough)
            {
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
