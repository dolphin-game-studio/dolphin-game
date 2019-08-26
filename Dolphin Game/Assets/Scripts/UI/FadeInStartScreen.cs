using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInStartScreen : MonoBehaviour
{
    StartScreenControler screenControler;

    void Awake()
    {
        screenControler = FindObjectOfType<StartScreenControler>();
    }

    public void LoadScreenControler() {
     }
}
