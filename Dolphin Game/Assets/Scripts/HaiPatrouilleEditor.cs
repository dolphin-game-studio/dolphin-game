using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HaiPatrouille))]
public class HaiPatrouilleEditor : Editor
{
    void OnSceneGUI()
    {
        HaiPatrouille haiPatrouille = (HaiPatrouille)target;

        foreach (var waypoint in haiPatrouille.waypoints)
        {
            EditorUtils.DrawFieldOfView(waypoint.transform, waypoint.transform.right, waypoint.transform.forward, waypoint.observationAngle, 20, 45);
        }


    }
}