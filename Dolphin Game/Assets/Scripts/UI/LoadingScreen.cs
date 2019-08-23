using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MenuScreen))]
public class LoadingScreen : MonoBehaviour
{
    MenuScreen menuScreen;

    [SerializeField] private Scene scene;

    AsyncOperation isLoadingGame;

    void Awake()
    {
        menuScreen = GetComponent<MenuScreen>();
    }

    void Update()
    {
        if (menuScreen.Active)
        {
            if (isLoadingGame == null)
            {
                isLoadingGame = SceneManager.LoadSceneAsync("final map shanice", LoadSceneMode.Single);
            }
        }
    }
}
