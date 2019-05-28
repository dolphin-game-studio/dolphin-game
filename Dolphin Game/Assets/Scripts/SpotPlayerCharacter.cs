using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayerCharacter : MonoBehaviour
{
    public Game game;
    Hai hai;
    bool noticedPlayer;
    float timePlayerNoticed;
    public float secondsToSpotPlayer = 2f;

    public Color Color1, Color2;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Start()
    {
        hai = GetComponent<Hai>();
        if (hai == null)
        {
            Debug.LogError("SpotPlayerCharacter must have a Hai component.");
        }

        if (game == null)
        {
            Debug.LogError("SpotPlayerCharacter must have a Game component.");
        }

        _propBlock = new MaterialPropertyBlock();
        _renderer = hai.viewMeshFilter.GetComponent<Renderer>();

    }

    void Update()
    {
        if (timePlayerNoticed > secondsToSpotPlayer)
        {
            game.Spotted = true;
        }

        if (noticedPlayer)
        {
            timePlayerNoticed += Time.deltaTime;
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

        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", Color.Lerp(Color1, Color2, timePlayerNoticed / secondsToSpotPlayer));
        _renderer.SetPropertyBlock(_propBlock);

    }
}
