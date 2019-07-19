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
        
        var waypoints = haiPatrouille.GetComponentsInChildren<Waypoint>();
        
        foreach (var waypoint in waypoints)
        {
            EditorUtils.DrawFieldOfView(waypoint.transform, waypoint.transform.right, waypoint.observationAngle, 20, 45);
        }
    }
}