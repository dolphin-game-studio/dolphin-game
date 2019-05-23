using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Hai))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI() {
        
		Hai fow = (Hai)target;
		Handles.color = Color.white;
		Handles.DrawWireArc (fow.transform.position, fow.transform.right, fow.transform.forward, 360, fow.viewRadius);


            var angleInDegrees = fow.transform.eulerAngles.x;

        var lookRotation = Quaternion.Euler(fow.transform.eulerAngles.x, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);
        var lookRotationLeftBound =  Quaternion.Euler(fow.transform.eulerAngles.x - fow.viewAngle / 2, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);
        var lookRotationRightBound = Quaternion.Euler(fow.transform.eulerAngles.x + fow.viewAngle / 2, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);

        Vector3 viewAngleA = lookRotation * Vector3.forward;// - new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); // fow.DirFromAngle (-fow.viewAngle / 2, false);
        Vector3 viewAngleB = lookRotationLeftBound * Vector3.forward; //fow.DirFromAngle (fow.viewAngle / 2, false);
        Vector3 viewAngleC = lookRotationRightBound * Vector3.forward; //fow.DirFromAngle (fow.viewAngle / 2, false);
         

        Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleC * fow.viewRadius);

        Handles.color = Color.red;
		foreach (Transform visibleTarget in fow.visibleTargets) {
			Handles.DrawLine (fow.transform.position, visibleTarget.position);
		}
	}

}
