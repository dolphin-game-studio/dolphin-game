using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSharkOnDestructed : MonoBehaviour
{
    [SerializeField] SharkPlayerController _sharkPlayerController;
    CharacterSelection _characterSelection;
    DestructableObstacle _destructableObstacle;

    void Awake()
    {
        _destructableObstacle = GetComponent<DestructableObstacle>();

        _characterSelection = FindObjectOfType<CharacterSelection>();
    }

    void Update()
    {
        if (_destructableObstacle.Destroyed) {
            _sharkPlayerController.IsPlayable = true;
            _sharkPlayerController.Achieved = true;

            _characterSelection.Visible = true;

            this.gameObject.SetActive(false);
        }
    }
}
