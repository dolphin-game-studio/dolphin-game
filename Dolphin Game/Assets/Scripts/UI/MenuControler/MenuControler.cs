using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MenuScreen), typeof(Animator))]
public class MenuControler : MonoBehaviour
{
    [SerializeField] private MenuItem[] menuItems;
    private int selectedMenuItemIndex = 0;

    private MenuScreen menuScreen;

    public int SelectedMenuItemIndex
    {
        get => selectedMenuItemIndex; set
        {
            if (selectedMenuItemIndex != value)
            {

                menuItems[selectedMenuItemIndex].Selected = false;

                if (value >= menuItems.Length)
                {
                    selectedMenuItemIndex = 0;
                }
                else if (value < 0)
                {
                    selectedMenuItemIndex = menuItems.Length - 1;
                }
                else
                {
                    selectedMenuItemIndex = value;
                }
                menuItems[selectedMenuItemIndex].Selected = true;
            }
        }
    }

    private Itend _userItend;

    public Itend UserItend
    {
        get => _userItend;

        set
        {
            if (_userItend != value)
            {
                _userItend = value;

                if (_userItend == Itend.up)
                {
                    SelectedMenuItemIndex--;
                }
                else if (_userItend == Itend.down)
                {
                    SelectedMenuItemIndex++;
                }
            }
        }
    }






    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
         
        menuScreen = GetComponent<MenuScreen>();

        if (menuItems.Length == 0)
        {
            throw new DolphinGameException("menuItems is empty!");
        }

    }

    public virtual void Start()
    {
        menuItems[0].Selected = true;
    }

    public virtual void Update()
    {
        if (menuScreen.Active)
        {
            var vertical = Input.GetAxisRaw("Vertical");


            if (UserItend == Itend.none && vertical >= 0.5)
            {
                UserItend = Itend.up;
            }

            if (UserItend == Itend.none && vertical <= -0.5)
            {
                UserItend = Itend.down;
            }

            if (UserItend != Itend.none && vertical == 0)
            {
                UserItend = Itend.none;
            }

            bool aButtonPressed = Input.GetButtonDown("A Button");

            if (aButtonPressed)
            {
                menuItems[selectedMenuItemIndex].Press();
            }
        }

        UpdateNormalizedAnimationTime();

    }


    #region Normalized Animation Time

    Animator animator;


    private void UpdateNormalizedAnimationTime()
    {
        animator.SetFloat("normalizedTime", Time.realtimeSinceStartup);
    }


    #endregion

}

public enum Itend
{
    up,
    down,
    none
}