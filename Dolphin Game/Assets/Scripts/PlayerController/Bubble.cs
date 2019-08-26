using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    void Awake()
    {
        
    }

    void Update()
    {
        
    }
 
    void OnTriggerEnter(Collider collision)
    {
        DestroyObject(this.gameObject);
    }
}
