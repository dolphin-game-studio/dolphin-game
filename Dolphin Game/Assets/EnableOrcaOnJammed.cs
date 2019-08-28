using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOrcaOnJammed : MonoBehaviour
{
    [SerializeField] private OrcaPlayerController _orcaPlayerController;
    private CharacterSelection _characterSelection;
    private Jammer _jammer;
    private bool _done;

    void Awake()
    {
        _jammer = GetComponent<Jammer>();
        _characterSelection = FindObjectOfType<CharacterSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_jammer.IsHacked) {
            _done = true;
            _orcaPlayerController.IsPlayable = true;
            _orcaPlayerController.Achieved = true;
            _characterSelection.Visible = true;
            gameObject.SetActive(false);
        }
    }
}
