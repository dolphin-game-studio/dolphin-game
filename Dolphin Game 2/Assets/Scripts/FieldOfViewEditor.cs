using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Hai))]
public class FieldOfViewEditor : Editor {
 
    void OnSceneGUI() {
        
		Hai fow = (Hai)target;

        var haiAngle = Vector3.Angle(fow.transform.forward, Vector3.right);

        //var haiAngle = fow.transform.eulerAngles.x;
        Debug.Log(fow.transform.forward);

        Handles.color = new Color(1, 0, 0, 0.4f);
        EditorUtils.DrawFieldOfView(fow.transform, Vector3.back, haiAngle, fow.viewRadiusWhenSuspicious, fow.viewAngle);
        Handles.color = new Color(1,1,1,0.4f);
        EditorUtils.DrawFieldOfView(fow.transform, Vector3.back, haiAngle, fow.ViewRadius, fow.viewAngle);
        Handles.color = new Color(0, 1, 0, 0.4f);
        EditorUtils.DrawFieldOfView(fow.transform, Vector3.back, haiAngle + fow.viewPanAngle1, fow.ViewRadius, fow.viewAngle);
        Handles.color = new Color(0, 0, 1, 0.4f);
        EditorUtils.DrawFieldOfView(fow.transform, Vector3.back, haiAngle + fow.viewPanAngle2, fow.ViewRadius, fow.viewAngle);

        var angleInDegrees = fow.transform.eulerAngles.x;

        var lookRotation = Quaternion.Euler(fow.transform.eulerAngles.x, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);
        var lookRotationLeftBound =  Quaternion.Euler(fow.transform.eulerAngles.x - fow.viewAngle / 2, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);
        var lookRotationRightBound = Quaternion.Euler(fow.transform.eulerAngles.x + fow.viewAngle / 2, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);

        Vector3 viewAngleA = lookRotation * Vector3.forward; 
        Vector3 viewAngleB = lookRotationLeftBound * Vector3.forward; 
        Vector3 viewAngleC = lookRotationRightBound * Vector3.forward; 
 
        Handles.color = Color.red;
		foreach (Transform visibleTarget in fow.visibleTargets) {
			Handles.DrawLine (fow.transform.position, visibleTarget.position);
		}
    }
}
