using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

 public class LoadingScreen : GameScreen
{
 
    [SerializeField] private Scene scene;

    AsyncOperation isLoadingGame;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (Active && FadeComplete)
        {
            if (isLoadingGame == null)
            {
                isLoadingGame = SceneManager.LoadSceneAsync("Final Level Alex", LoadSceneMode.Single);
            }
        }
    }
}
