using UnityEngine;
using System.Collections;

public class JointArm : MonoBehaviour {
	public Transform arm_p1;  			// top arm
	public Transform arm_p1_target;  	// joint - lookat target for top arm
	public Transform arm_p2;  			// bottom arm
	public Transform arm_p2_target;  	// joint on top arm - lookat target for bottom arm
	
	//private Vector3 newpos;
	
	void LateUpdate () {
	        
		// update arm angles
		//Vector3 targetDir = arm_p1_target.position - arm_p1.position;
        arm_p1.LookAt(arm_p1_target);
        arm_p2.LookAt(arm_p2_target);
       
        Debug.DrawLine(arm_p2.position, arm_p2_target.position, Color.red);
        Debug.DrawLine(arm_p1.position, arm_p1_target.position, Color.green);	
	}
}
