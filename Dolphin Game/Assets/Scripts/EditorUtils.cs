using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class EditorUtils
{
    public static void DrawFieldOfView(Transform transform, Vector3 normal, float angle, float radius, float viewAngle)
    {
        var position = transform.position;
        var angles = transform.eulerAngles;

        Vector3 leftAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad + (viewAngle * Mathf.Deg2Rad / 2)), Mathf.Sin(angle * Mathf.Deg2Rad + (viewAngle * Mathf.Deg2Rad / 2)), 0);
        //Vector3 rightAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad  + (viewAngle * Mathf.Deg2Rad / 2)), Mathf.Sin(angle * Mathf.Deg2Rad + (viewAngle * Mathf.Deg2Rad / 2)), 0);


        //var lookRotationLeftBound = Quaternion.Euler(angles.x + angle - viewAngle / 2, angles.y, angles.z);
        //var lookRotationRightBound = Quaternion.Euler(angles.x + angle + viewAngle / 2, angles.y, angles.z);

        //Vector3 leftAngle = lookRotationLeftBound * Vector3.forward;
        //Vector3 rightAngle = lookRotationRightBound * Vector3.forward;

        Handles.DrawSolidArc(position, normal, leftAngle, viewAngle, radius);
    }
}
#endif