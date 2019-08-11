using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisualisationPanel : MonoBehaviour
{
    [SerializeField] private RectTransform image;
    [SerializeField] private RectTransform panel;


    public RectTransform Image => image;
    public RectTransform Panel => panel;
     
}
