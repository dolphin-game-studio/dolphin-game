using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class AbilityIcon : MonoBehaviour
{
    private Image _image;
    private Animator _animator;

    [SerializeField] private float _edgeGlow = 0;


    [SerializeField] private float _filled;
    [SerializeField] private float _pintPow;
    [SerializeField] private float _distMultiplier;
    [SerializeField] private float _starBrightness;

    [SerializeField] private bool _usable = true;
    public bool Usable
    {
        get => _usable; set
        {
            _usable = value;
        }
    }

    [SerializeField] private bool _active = false;
    public bool Active { get => _active; set => _active = value; }

    public float Filled { get => _filled; set => _filled = value; }

    private bool cooldownFinished = false;

    public void TriggerCooldownFinished()
    {
        cooldownFinished = true;
    }
 

    void Awake()
    {
        _image = GetComponent<Image>();
        _image.material = Instantiate(_image.material);

        _animator = GetComponent<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {

        if (cooldownFinished)
        {
            _animator.SetTrigger("cooldown finished");
            cooldownFinished = false;
        }

        _image.material.SetFloat("_Filled", _filled);
        _image.material.SetFloat("_PintPow", _pintPow);
        _image.material.SetFloat("_DistMultiplier", _distMultiplier);
        _image.material.SetFloat("_StarBrightness", _starBrightness);




        if (_usable)
        {
            _image.material.SetFloat("_AbilityUsable", 1);
        }
        else
        {
            _image.material.SetFloat("_AbilityUsable", 0);
        }


        _animator.SetBool("active", _active);

        _image.material.SetFloat("_EdgeGlow", _edgeGlow);




    }
}
