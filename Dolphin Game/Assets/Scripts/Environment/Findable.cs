using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Findable : MonoBehaviour
{
    #region Found
    private HashSet<Echo> echosThatFoundTheCorridor = new HashSet<Echo>();

    [SerializeField] private float timeUntilIsNotFoundAnymore = 10f;
    private float timeSinceFound = 0;
     
    private bool found = false;

    public bool Found
    {
        get => found;
        set
        {
            if (found != value)
            {
                found = value;
                timeSinceFound = 0;
            }
        }
    }

    public void FoundByEcho(Echo echo)
    {
        bool isNewEcho = !echosThatFoundTheCorridor.Contains(echo);
        if (isNewEcho)
        {
            echosThatFoundTheCorridor.Add(echo);
            Found = true;
        }
    }
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        if (Found)
        {
            timeSinceFound += Time.deltaTime;

            if (timeSinceFound > timeUntilIsNotFoundAnymore)
            {
                Found = false;
            }
        }
    }
}
