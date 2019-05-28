using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayerCharacter : MonoBehaviour
{
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
        
        _propBlock = new MaterialPropertyBlock();
        _renderer = hai.viewMeshFilter.GetComponent<Renderer>();

    }

    void Update()
    {
        if (timePlayerNoticed > secondsToSpotPlayer)
        {
            print("Spotted!");
        }
        else {
            print(timePlayerNoticed);
        }

        if (noticedPlayer) {
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
