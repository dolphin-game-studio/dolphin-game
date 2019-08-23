﻿
using UnityEngine;

public class DestructableObstacle : MonoBehaviour
{
    public bool OrcaCanDestroy
    {
        get
        {
            if (Destroyed)
            {
                return false;
            }
            else if (NotDestroyed) {
                if (findable != null)
                {
                    return findable.Found;
                }
                else {
                    return true;
                }
            }
            return false;
        }
    }


    private Findable findable;

    SphereCollider sphereCollider;
    Rigidbody[] stones;
    [SerializeField] private float minDestructableForce = 300;
    [SerializeField] private float maxDestructableForce = 1000;

    private bool destroyed = false;
    public bool Destroyed { get => destroyed; set => destroyed = value; }
    public bool NotDestroyed { get => !Destroyed; set => Destroyed = !value; }


    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();

        if (sphereCollider == null)
        {
            throw new DolphinGameException("SphereCollider component isn't set on Destructable.");
        }

        stones = transform.GetComponentsInChildren<Rigidbody>();

    }

    void Update()
    {

    }

    public void Destroy()
    {
        destroyed = true;
        sphereCollider.enabled = false;

        foreach (var stone in stones)
        {
            float randomForceOnX = Random.Range(-minDestructableForce, maxDestructableForce);
            float randomForceOnY = Random.Range(-minDestructableForce, maxDestructableForce);
            float randomForceOnZ = Random.Range(-minDestructableForce, maxDestructableForce);

            if (Random.value > 0.5)
                randomForceOnX = -randomForceOnX;
            if (Random.value > 0.5)
                randomForceOnY = -randomForceOnY;
            if (Random.value > 0.5)
                randomForceOnZ = -randomForceOnZ;

            stone.AddForce(new Vector3(randomForceOnX, randomForceOnY, randomForceOnZ));
            stone.AddTorque(new Vector3(randomForceOnX, randomForceOnY, randomForceOnZ));

            Destroy(stone.gameObject, 10);
        }
    }
}
