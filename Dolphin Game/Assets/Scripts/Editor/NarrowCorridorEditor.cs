using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(NarrowCorridor))]

public class NarrowCorridorEditor : Editor
{
    void OnSceneGUI()
    {
        NarrowCorridor thisNarrowCorridor = (NarrowCorridor)target;

        var otherNarrowCorridor = thisNarrowCorridor.OtherNarrowCorridor;

        if (otherNarrowCorridor != null) {

            Handles.color = new Color(1, 0, 0, 1f);
            Handles.DrawLine(thisNarrowCorridor.transform.position, otherNarrowCorridor.transform.position);
        }
    }
}
#endif