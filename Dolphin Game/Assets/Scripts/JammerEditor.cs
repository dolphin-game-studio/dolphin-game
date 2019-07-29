using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

[CustomEditor(typeof(Jammer))]
public class JammerEditor : Editor
{
    void OnSceneGUI()
    {
        Jammer jammer = (Jammer)target;

        Handles.color = new Color(1, 0, 0, 0.4f);

        Handles.DrawSolidDisc(jammer.transform.position, Vector3.forward, jammer.Radius);
        
         
    }
}
#endif