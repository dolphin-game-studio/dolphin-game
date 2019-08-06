 
using UnityEngine;

public class RandomAnimationSpeed : MonoBehaviour
{
    Animator animator;

    [SerializeField] private float swimSpeed = 0.1f;
    [SerializeField] private float minAnimatorSpeed = 10f;
    [SerializeField] private float randomAnimatorSpeedMultiplier = 10;

    public float RandomAnimatorSpeedMultiplier
    {
        get => randomAnimatorSpeedMultiplier; set
        {
            if (randomAnimatorSpeedMultiplier != value) {
                randomAnimatorSpeedMultiplier = value;
                UpdateAnimatorParameters();
            }
        }
    }
    public float MinAnimatorSpeed
    {
        get => minAnimatorSpeed; set
        {
            if (minAnimatorSpeed != value)
            {
                minAnimatorSpeed = value;
                UpdateAnimatorParameters();
            }
        }
    }

    public float SwimSpeed { get => swimSpeed; set => swimSpeed = value; }
    
    void Start()
    {
        animator = GetComponent<Animator>();

        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        animator.SetFloat("Speed", MinAnimatorSpeed + Random.value * RandomAnimatorSpeedMultiplier);
    }

     void Update()
    {
        transform.position += transform.forward * SwimSpeed * Time.deltaTime;
    }
}
