using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Hai))]
public class FieldOfViewEditor : Editor {

 

    void DrawFieldOfView(Transform transform, Vector3 normal, Vector3 from, float angle, float radius, float viewAngle) {
        var position = transform.position;
        var angles = transform.eulerAngles;

        var lookRotationLeftBound = Quaternion.Euler(angles.x + angle - viewAngle / 2, angles.y, angles.z);
        var lookRotationRightBound = Quaternion.Euler(angles.x + angle + viewAngle / 2, angles.y, angles.z);

        Vector3 leftAngle = lookRotationLeftBound * Vector3.forward;  
        Vector3 rightAngle  = lookRotationRightBound * Vector3.forward;  

        //Handles.DrawLine(position, position + leftAngle * radius);
        //Handles.DrawLine(position, position + rightAngle * radius);

        Handles.DrawSolidArc(position, normal, leftAngle, viewAngle, radius);
    }
 
    void OnSceneGUI() {
        
		Hai fow = (Hai)target;

        Handles.color = new Color(1, 0, 0, 0.4f);
        DrawFieldOfView(fow.transform, fow.transform.right, fow.transform.forward, 0, fow.viewRadiusWhenSuspicious, fow.viewAngle);
        Handles.color = new Color(1,1,1,0.4f);
        DrawFieldOfView(fow.transform, fow.transform.right, fow.transform.forward, 0, fow.ViewRadius, fow.viewAngle);
        Handles.color = new Color(0, 1, 0, 0.4f);
        DrawFieldOfView(fow.transform, fow.transform.right, fow.transform.forward, fow.viewPanAngle1, fow.ViewRadius, fow.viewAngle);
        Handles.color = new Color(0, 0, 1, 0.4f);
        DrawFieldOfView(fow.transform, fow.transform.right, fow.transform.forward, fow.viewPanAngle2, fow.ViewRadius, fow.viewAngle);

        var angleInDegrees = fow.transform.eulerAngles.x;

        var lookRotation = Quaternion.Euler(fow.transform.eulerAngles.x, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);
        var lookRotationLeftBound =  Quaternion.Euler(fow.transform.eulerAngles.x - fow.viewAngle / 2, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);
        var lookRotationRightBound = Quaternion.Euler(fow.transform.eulerAngles.x + fow.viewAngle / 2, fow.transform.eulerAngles.y, fow.transform.eulerAngles.z);

        Vector3 viewAngleA = lookRotation * Vector3.forward;// - new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); // fow.DirFromAngle (-fow.viewAngle / 2, false);
        Vector3 viewAngleB = lookRotationLeftBound * Vector3.forward; //fow.DirFromAngle (fow.viewAngle / 2, false);
        Vector3 viewAngleC = lookRotationRightBound * Vector3.forward; //fow.DirFromAngle (fow.viewAngle / 2, false);
         

        //Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		//Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
        //Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleC * fow.viewRadius);

        Handles.color = Color.red;
		foreach (Transform visibleTarget in fow.visibleTargets) {
			Handles.DrawLine (fow.transform.position, visibleTarget.position);
		}




       

    }

}
