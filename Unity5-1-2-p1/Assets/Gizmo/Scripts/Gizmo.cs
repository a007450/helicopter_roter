using UnityEngine;
using System.Collections;


public class Gizmo : MonoBehaviour {

	[Space(10)]
	public LayerMask layer;
	[Space(10)]
	public Collider xAxis;
	public Collider yAxis;
	public Collider zAxis;
	[Space(10)]
	public Collider xyPlane;
	public Collider xzPlane;	
	public Collider yzPlane;
	[Space(10)]
	public Collider xyRotate;
	[Space(10)]
	public float rotationSpeed = 4.0f;

	private enum GizmoAxis {X, Y, Z, XY, XZ, YZ, XYRotate};
	private GizmoAxis selectedAxis;

	private RaycastHit hit; 
	private Ray ray, ray1;
	private Plane dragplane, mousePlane;
	private float rayDistance, distToCenter, newRotY;
	private Vector3 hitvector, mousePos, direction;
	private bool dragging, rotating;



	void Start(){	
		
		newRotY = transform.localEulerAngles.y;
	}

	void Update () {


		if(Input.GetMouseButtonDown(0)){

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			dragplane = new Plane();

			if (Physics.Raycast(ray, out hit,1000f, layer)){
		
				hitvector = hit.point - transform.position;

				if (hit.collider == xAxis){
					selectedAxis = GizmoAxis.X;
					direction = Vector3.right;
					dragplane.SetNormalAndPosition(transform.up, transform.position);
				}
				else if (hit.collider == yAxis){
					selectedAxis = GizmoAxis.Y;
					direction = Vector3.up;
					dragplane.SetNormalAndPosition(transform.forward, transform.position);
				}
				else if (hit.collider == zAxis){
					selectedAxis = GizmoAxis.Z;	
					direction = Vector3.forward;
					dragplane.SetNormalAndPosition(transform.up, transform.position);
				}
				else if(hit.collider == xyPlane){
					selectedAxis = GizmoAxis.XY;				
					dragplane.SetNormalAndPosition(transform.forward, transform.position);
				}
				else if(hit.collider == xzPlane){
					selectedAxis = GizmoAxis.XZ;				
					dragplane.SetNormalAndPosition(transform.up, transform.position);
				}
				else if(hit.collider == yzPlane){
					selectedAxis = GizmoAxis.YZ;				
					dragplane.SetNormalAndPosition(transform.right, transform.position);
				}
				else if(hit.collider == xyRotate){
					selectedAxis = GizmoAxis.XYRotate;				
					rotating=true;
				}

				if(dragplane.Raycast(ray, out rayDistance)){
					distToCenter = Vector3.Distance(hit.point, transform.position);
				}

				dragging = true;
			}
		}	

		if(dragging && !rotating ){

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if(dragplane.Raycast(ray, out rayDistance)){

				mousePos = ray.GetPoint(rayDistance);
			}

			mousePlane = new Plane();

			switch(selectedAxis){

				case GizmoAxis.X:{					
					mousePlane.SetNormalAndPosition(transform.right, mousePos);
					ray = new Ray(transform.position, transform.right);
					break;
				}
				case GizmoAxis.Y:{					
					mousePlane.SetNormalAndPosition(transform.up, mousePos);
					ray = new Ray(transform.position, transform.up);
					break;
				}
				case GizmoAxis.Z:{					
					mousePlane.SetNormalAndPosition(transform.forward, mousePos);
					ray = new Ray(transform.position, transform.forward);
					break;
				}
				case GizmoAxis.XY: case GizmoAxis.XZ: case GizmoAxis.YZ:{
					transform.position = mousePos - hitvector;					
					break;
				}

				break;
			}

			if(mousePlane.Raycast(ray, out rayDistance)){
				Vector3 trans = direction * (rayDistance-distToCenter);
				transform.Translate(trans);			
			}

			if(Input.GetMouseButtonUp(0)) 
				SetFalse();

		}

		if(rotating){
			
			newRotY -= Input.GetAxis("Mouse X") * rotationSpeed;
			transform.localEulerAngles  = new Vector3(transform.localEulerAngles.x, newRotY,transform.localEulerAngles.z);

			if(Input.GetMouseButtonUp(0)) 
				SetFalse();	
		}
	}

	void SetFalse(){

		rotating = false;		
		dragging = false;
	}
}