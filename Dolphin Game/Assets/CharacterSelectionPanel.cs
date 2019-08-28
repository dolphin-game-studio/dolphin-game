using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class CharacterSelectionPanel : MonoBehaviour
{
    [SerializeField] private PlayerControllerBase _playerController;

    private Image _image;
    private Animator _animator;

    [SerializeField] private float _edgeGlow = 0;

    [SerializeField] private bool _usable = true;

    [SerializeField] private bool _active = false;
    public bool Active { get => _active; set => _active = value; }

 
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
         
        if (_playerController.IsPlayable)
        {
            _image.material.SetFloat("_AbilityUsable", 1);
        }
        else
        {
            _image.material.SetFloat("_AbilityUsable", 0);
        }

        _animator.SetBool("achieved", _playerController.Achieved);

        if (_playerController.Achieved)
        {
 
            _image.material.SetFloat("_EdgeGlow", _edgeGlow);
        }
        else
        {
            _image.material.SetFloat("_EdgeGlow", 0);
        }



        //_image.material.SetFloat("_EdgeGlow", 1);

        //_image.material.SetFloat("_EdgeGlow", _edgeGlow);




    }
}
