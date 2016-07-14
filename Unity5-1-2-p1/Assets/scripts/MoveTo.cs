using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {
	public Transform target;
	
	private Vector3 newpos;
	void Update () {
		float step = 10f * Time.deltaTime;
		newpos = transform.localPosition;
		newpos.x = target.localPosition.x;
		newpos.y = target.localPosition.y;
		//print (newpos.x);
		transform.localPosition = newpos;//Vector3.MoveTowards(transform.position, newpos, step);
	
	}
}
