using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBackgroundMusicControler : MonoBehaviour
{
    [SerializeField] private BackgroundMusicControler backgroundMusicControlerPrefab;

    void Awake()
    {
        BackgroundMusicControler existingMusicManagers = GameObject.FindObjectOfType<BackgroundMusicControler>();
        if (existingMusicManagers == null)
        {
            var controler = Instantiate<BackgroundMusicControler>(backgroundMusicControlerPrefab);
            DontDestroyOnLoad(controler);
        }
    }
 
}
