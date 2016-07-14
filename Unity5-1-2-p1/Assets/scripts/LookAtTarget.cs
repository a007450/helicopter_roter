using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {
	public Transform target;  			
		
	void Update () {
		//dir = new Vector3(-1,0,0);
        transform.LookAt(target);
    
	}
}
