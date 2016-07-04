using UnityEngine;
using System.Collections;

public class PistonsController : MonoBehaviour {
	// rotation 
	public Transform[] rotateObjects;
	
	private float sp = 0f;
	
	// define the piston points that push plate
	public Transform p1;
	public Transform p2;
	public Transform p3;
	public Transform targetObj;
		 
	// visualize plane, normals
	private Plane plane;
	private Mesh mesh;
	private float adjustSpeed = 30;
		
	// set limits to piston y movement
	private float y_init;
	private float dy_max = 0.02f;
	private float dy_min = 0.015f;
	
	// Use this for initialization
	void Start () {
	
		Vector3 _1 = p1.position;
        Vector3 _2 = p2.position;
        Vector3 _3 = p3.position;
                
		plane = new Plane(_1, _2, _3);
		        
        y_init = p1.position.y;
        
		/*	
		gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        
        mesh.vertices = new Vector3[] {_1, _2, _3};
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles = new int[] {0, 1, 2}; 
        */
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 _1 = p1.position;
        Vector3 _2 = p2.position;
        Vector3 _3 = p3.position;
        
        //p1.position = new Vector3(_1.x, y_init + -dy_min + Mathf.PingPong(0.02f*Time.time,dy_max-dy_min) , _1.z);
        
	  	plane.Set3Points(_1, _2, _3);
	  	
        // set plane
	  	Vector3 newUp = Vector3.RotateTowards(targetObj.up, plane.normal, adjustSpeed*Mathf.Deg2Rad, 0);
      	targetObj.rotation = Quaternion.FromToRotation(transform.up, plane.normal);
      	
      	// move plate as plane moves
      	Vector3 obj_pos = targetObj.position;
      	obj_pos.y = -1.0f*plane.distance;
      	targetObj.position = obj_pos;
      	      	      	     
       	// debug - update mesh for visualizing plane
       	/*
        mesh.Clear();
        mesh.vertices = new Vector3[] {_1, _2, _3};
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles = new int[] {0, 1, 2};
        */
       
	}
	
	void LateUpdate(){
		if (!Input.GetMouseButton(0))
		{
        
        	if( (p1y_old==piston1y) && (p2y_old==piston2y) && (p3y_old==piston3y) && (slidersp_old==slidersp)){
        		itsKGFOrbitCam.SetPanningEnable( true ) ;
       	 	};

			p1y_old=piston1y;
    		p2y_old=piston2y;
    		p3y_old=piston3y;
        	slidersp_old=slidersp;
		}
	}
	
	// return angle opposite to side l1, where l1, l2, l3 defines points of a triangle
	private float GetAngleSSS(float l1, float l2, float l3){
		return 0;
	}
	
	private void DoRotate(){
		
		foreach(Transform t in rotateObjects){
			t.localRotation = Quaternion.Euler(0, sp*Time.time, 0);	
		}
	}
	
	// for testing
	public GameObject fixCam; // fixed-view cam reference
	private bool fixedViewOnOff = false;
	private float piston1y = 0;
	private float piston2y = 0;
	private float piston3y = 0;
	private float slidersp = 0;
	private float p1y_old = 0;
    private float p2y_old = 0;
    private float p3y_old = 0;
    private float slidersp_old = 0;
	private float sp_max = 200;
	public GUISkin skin;
	public KGFOrbitCam itsKGFOrbitCam;	//reference to the orbitcam		
	
	void OnGUI(){
		Vector3 _1 = p1.position;
        Vector3 _2 = p2.position;
        Vector3 _3 = p3.position;
                        
        // draw GUI elements        
        GUI.skin = skin;
        GUILayout.BeginHorizontal(new GUIStyle("box"));
            	
        piston1y = GUILayout.VerticalSlider(piston1y, dy_max, -dy_min);		
        DetectOnChange(p1y_old, piston1y);
		piston2y = GUILayout.VerticalSlider(piston2y, dy_max, -dy_min);	
		DetectOnChange(p2y_old, piston2y);	
		piston3y = GUILayout.VerticalSlider(piston3y, dy_max, -dy_min);		
		DetectOnChange(p3y_old, piston3y);
						
		GUILayout.EndHorizontal();

		slidersp = GUILayout.HorizontalSlider(slidersp, 0, sp_max, new GUIStyle("box"), new GUIStyle("horizontalsliderthumb"), GUILayout.Width(200));			
    	DetectOnChange(slidersp_old, slidersp);
    	
		// handle GUI slider values		
		_1.y = y_init + piston1y;
		p1.position = _1;
		
		_2.y = y_init + piston2y;
		p2.position = _2;
		
		_3.y = y_init + piston3y;
		p3.position = _3;
		
		if (slidersp_old==slidersp){
			sp = slidersp;// Mathf.Lerp(sp, slidersp,Time.deltaTime);
		  	DoRotate();	
		}
		
		
		if (fixCam){
			fixedViewOnOff = GUILayout.Toggle(fixedViewOnOff,"Toggle Fixed View Camera", new GUIStyle("button"));
			fixCam.SetActive(fixedViewOnOff);
		}
	}
	
	
	float DetectOnChange( float oldVal, float newVal ) {	
		
		if(itsKGFOrbitCam != null){
			// detect if user is using sliders -- slider values changing -- if so, disable mouse control
			
			if (oldVal != newVal){
				itsKGFOrbitCam.SetPanningEnable(false);
			}
		}
		
		
		return newVal;
	
	}

	
}
