using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float spinningVelocity = 20f;

    void Update()
    {
        transform.Rotate(Vector3.forward, spinningVelocity * Time.deltaTime);
    }
}
