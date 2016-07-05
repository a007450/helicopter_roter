using UnityEngine;
using System.Collections;

public class ContactPlane : MonoBehaviour {
	
	private Vector3 newPos;
	private RaycastHit hit;
	
	void Start(){
		newPos = transform.position;
	}
	
	void Update() {
        Vector3 dir = transform.TransformDirection(Vector3.up);
        if ( Physics.Raycast(transform.position, dir, out hit, 50)||
        	 Physics.Raycast(transform.position, -dir, out hit, 50)){
        	
        	newPos.y = hit.point.y;
            transform.position = newPos;
        	
            //print(hit.point.y + " hit " + hit.distance);
        
        }
        
    }
    
}
