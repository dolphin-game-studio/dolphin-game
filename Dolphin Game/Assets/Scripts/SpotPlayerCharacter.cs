﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayerCharacter : MonoBehaviour
{
    private Game game;
    Hai hai;
    bool noticedPlayer;
    float timePlayerNoticed;
    public float secondsToSpotPlayer = 2f;

    private Color colorNormal = new Color(0, 1, 0), colorSuspicious = new Color(1, 1, 0), colorSpotted = new Color(1, 0, 0);

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Start()
    {
        hai = GetComponent<Hai>();

        game = FindObjectOfType<Game>();

        if (hai == null)
        {
            throw new DolphinGameException("SpotPlayerCharacter must have a Hai component.");
        }

        if (game == null)
        {
            throw new DolphinGameException("The essentials are not present in this scene. Please add one Essentials object from the Prefabs Folder.");
        }

        _propBlock = new MaterialPropertyBlock();
        _renderer = hai.viewMeshFilter.GetComponent<Renderer>();

    }

    void Update()
    {

        if (hai.Conscious)
        {
            SpotPlayer();

        }


    }

    private void SpotPlayer()
    {
        _renderer.GetPropertyBlock(_propBlock);

        if (timePlayerNoticed > secondsToSpotPlayer)
        {
            game.Spotted = true;
        }

        if (noticedPlayer)
        {
            timePlayerNoticed += Time.deltaTime;

            _propBlock.SetColor("_Color", new Color(1, Mathf.Lerp(1, 0, timePlayerNoticed / secondsToSpotPlayer), 0));
        }
        else
        {
            _propBlock.SetColor("_Color", colorNormal);
        }

        if (hai.visibleTargets.Count > 0)
        {
            noticedPlayer = true;
        }
        else
        {
            noticedPlayer = false;
            timePlayerNoticed = 0;
        }


        _renderer.SetPropertyBlock(_propBlock);
    }
}
