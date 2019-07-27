using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    void OnSceneGUI()
    {
        Waypoint waypoint = (Waypoint)target;

        EditorUtils.DrawFieldOfView(waypoint.transform, waypoint.transform.right, waypoint.observationAngle, 20, 45);
    }
}
#endif