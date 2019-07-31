using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jammer : MonoBehaviour
{

    [SerializeField] private float radius = 10f;
    public float Radius { get { return radius; } }

    private bool _isHacked;
    public bool IsHacked { get { return _isHacked; } set { _isHacked = value; } }
    public bool IsNotHacked { get { return !_isHacked; } }


    void Start()
    {

    }

    void Update()
    {

    }
}
