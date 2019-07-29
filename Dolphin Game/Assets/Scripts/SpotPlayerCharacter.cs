using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayerCharacter : MonoBehaviour
{
    private Game game;
    Hai hai;
    bool noticedPlayer;
    float timePlayerNoticed;
    public float secondsToSpotPlayer = 2f;
    public float secondsToSpotPlayerWhenNear = 0.2f;


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
         
        if (hai.NearestTarget != null && hai.NearestTarget.GetComponent<Bubble>() == null )
        {
            var distanceToPlayer = Vector3.Distance(hai.transform.position, hai.NearestTarget.transform.position);
             
            var timeToSpotPlayerRegardingDistanceToPlayer = Mathf.Lerp(secondsToSpotPlayer, secondsToSpotPlayerWhenNear, 1 - distanceToPlayer / hai.viewRadiusWhenSuspicious);

            if (timePlayerNoticed > timeToSpotPlayerRegardingDistanceToPlayer)
            {
                game.Spotted = true;
            }


            timePlayerNoticed += Time.deltaTime;

            _propBlock.SetColor("_Color", new Color(1, Mathf.Lerp(1, 0, timePlayerNoticed / timeToSpotPlayerRegardingDistanceToPlayer), 0));
        }
        else
        {
            timePlayerNoticed = 0;
            _propBlock.SetColor("_Color", colorNormal);
        }
 

        _renderer.SetPropertyBlock(_propBlock);
    }
}
