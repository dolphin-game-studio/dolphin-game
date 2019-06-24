using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorUtils
{
    public static void DrawFieldOfView(Transform transform, Vector3 normal, Vector3 from, float angle, float radius, float viewAngle)
    {
        var position = transform.position;
        var angles = transform.eulerAngles;

        var lookRotationLeftBound = Quaternion.Euler(angles.x + angle - viewAngle / 2, angles.y, angles.z);
        var lookRotationRightBound = Quaternion.Euler(angles.x + angle + viewAngle / 2, angles.y, angles.z);

        Vector3 leftAngle = lookRotationLeftBound * Vector3.forward;
        Vector3 rightAngle = lookRotationRightBound * Vector3.forward;

        Handles.DrawSolidArc(position, normal, leftAngle, viewAngle, radius);
    }
}
