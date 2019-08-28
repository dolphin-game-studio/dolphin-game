using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour
{
    [SerializeField] private bool _isPlayable = true;
    public bool IsPlayable { get => _isPlayable; set => _isPlayable = value; }

    [SerializeField] private bool _achieved = false;
    public bool Achieved { get => _achieved; set => _achieved = value; }


    protected Hai[] haie;
    CapsuleCollider Collider;

    protected Animator _animator;

    public Rigidbody Rigidbody { get; set; }

    protected PlayerController playerController;

    [SerializeField] private float _speed = 10f;
    public float Speed
    {
        get => _speed;
    }

    [SerializeField] private float _fastSwimMultiplier = 2f;
    public float FastSwimMultiplier
    {
        get => _fastSwimMultiplier;
    }

    [SerializeField] private float maxRotationDegreesDelta = 0.1f;

    [SerializeField] private bool funnyMoveMent = false;

    public virtual bool CanMove { get { return true; } }

    void Awake()
    {
        Init();
    }


    protected Hai GetNearestFacingShark(out float distanceToNearestFacingShark, out Vector3 fromPlayerToNearestFacingSharkVector, bool searchForKnockedOutShark = false)
    {
        Hai nearestShark = null;
        float nearestSharkDistance = float.MaxValue;
        fromPlayerToNearestFacingSharkVector = Vector3.zero;

        foreach (var hai in haie)
        {
            bool searchingForKnockedOutSharkButSharkIsNotKnockedOut = searchForKnockedOutShark && hai.IsNotKnockedOut;

            bool searchingForNotKnockedOutSharkButSharkIsKnockedOut = !searchForKnockedOutShark && hai.IsKnockedOut;

            if (searchingForKnockedOutSharkButSharkIsNotKnockedOut)
                continue;

            if (searchingForNotKnockedOutSharkButSharkIsKnockedOut)
                continue;

            var fromPlayerToSharkVector = hai.transform.position - transform.position;

            var dotProdToShark = Vector3.Dot(fromPlayerToSharkVector.normalized, transform.forward);

            bool facingTheShark = dotProdToShark > 0;

            if (facingTheShark)
            {
                var distanceToShark = Vector3.Distance(hai.transform.position, transform.position);
                if (distanceToShark < nearestSharkDistance)
                {
                    nearestSharkDistance = distanceToShark;
                    nearestShark = hai;
                    fromPlayerToNearestFacingSharkVector = fromPlayerToSharkVector;
                }
            }

        }

        distanceToNearestFacingShark = nearestSharkDistance;
        return nearestShark;
    }

    protected virtual void Init()
    {
        playerController = FindObjectOfType<PlayerController>();


        haie = FindObjectsOfType<Hai>();

        Collider = GetComponent<CapsuleCollider>();
        if (Collider == null)
        {
            throw new DolphinGameException("collider is not set.");
        }

        Rigidbody = GetComponent<Rigidbody>();
        if (Rigidbody == null)
        {
            throw new DolphinGameException("Rigidbody Component is not set on this PlayerController");
        }

        if (playerController == null)
        {
            throw new DolphinGameException("The essentials are not present in this scene. Please add one Essentials object from the Prefabs Folder.");
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            throw new DolphinGameException("Animator Component is not set on DolphinPlayerController");
        }
    }

    protected virtual void Update()
    {
        if (playerController.CurrentPlayerController != this && !IsPlayable)
            return;

    }

    public void Move(float horizontal, float vertical)
    {
        var cantMove = !CanMove;
        if (cantMove)
        {
            return;
        }

        var movement = new Vector3(horizontal, vertical);
        Rigidbody.velocity = movement;

        if (funnyMoveMent)
        {
            transform.Rotate(movement);
        }
        if (movement == Vector3.zero)
        {
            movement = Vector3.forward;
        }
        var lookRotation = Quaternion.LookRotation(movement);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, maxRotationDegreesDelta);

        _animator.SetFloat("Speed", movement.magnitude);
    }
}
