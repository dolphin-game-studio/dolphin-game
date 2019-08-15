using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    void Start()
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
