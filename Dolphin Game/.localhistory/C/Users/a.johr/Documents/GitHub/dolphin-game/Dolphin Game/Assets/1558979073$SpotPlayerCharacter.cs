using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayerCharacter : MonoBehaviour
{
    Hai hai;
    bool noticedPlayer;
    float timePlayerNoticed;
    public float secondsToSpotPlayer = 5f;

    public Color Color1, Color2;
    public float Speed = 1, Offset;

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
        _renderer = GetComponent<Renderer>();

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
        // Assign our new value.
        _propBlock.SetColor("_Color", Color.Lerp(Color1, Color2, (Mathf.Sin(Time.time * Speed + Offset) + 1) / 2f));
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
    }
}
