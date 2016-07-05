using UnityEngine;
using System.Collections;

public class PistonsController : MonoBehaviour {
	public RadialSlider r_slide;
	
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
        
		
		gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        
        mesh.vertices = new Vector3[] {_1, _2, _3};
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles = new int[] {0, 1, 2}; 
       
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
       	
        mesh.Clear();
        mesh.vertices = new Vector3[] {_1, _2, _3};
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles = new int[] {0, 1, 2};
        
        Debug.Log(r_slide.ang);
       
	}
	
	void LateUpdate(){
		if (!Input.GetMouseButton(0))
		{
        
        	if( (p1y_old==piston1y) 
        		&& (p2y_old==piston2y) 
        		&& (p3y_old==piston3y) 
        		&& (slidersp_old==slidersp) 
        		&& (collectivey_old == collectivey) )
        	{
        		itsKGFOrbitCam.SetPanningEnable( true );
       	 	};

			p1y_old = piston1y;
    		p2y_old = piston2y;
    		p3y_old = piston3y;
        	slidersp_old = slidersp;
        	collectivey_old = collectivey;
		}
	}
		
	private void DoRotate(){
		
		foreach(Transform t in rotateObjects){
			t.localRotation = Quaternion.Euler(0, sp*Time.time, 0);	
		}
	}
	
	// UI - controls 
	public GameObject fixCam; // fixed-view cam reference
	public Transform Collective;
	private bool fixedViewOnOff = false;
	private float collectivey = 0;
	private float piston1y = 0;
	private float piston2y = 0;
	private float piston3y = 0;
	private float slidersp = 0;
	private float collectivey_old = 0;
	private float p1y_old = 0;
    private float p2y_old = 0;
    private float p3y_old = 0;
    private float slidersp_old = 0;
	private float sp_max = 1000;
	public GUISkin skin;
	public KGFOrbitCam itsKGFOrbitCam;	//reference to the orbitcam		
	
	void OnGUI(){
		Vector3 _1 = p1.position;
        Vector3 _2 = p2.position;
        Vector3 _3 = p3.position;
                        
        // draw GUI elements        
        GUI.skin = skin;
        GUILayout.BeginHorizontal();
       /*     	
        piston1y = GUILayout.VerticalSlider(piston1y, dy_max, -dy_min);		
        DetectOnChange(p1y_old, piston1y);
		piston2y = GUILayout.VerticalSlider(piston2y, dy_max, -dy_min);	
		DetectOnChange(p2y_old, piston2y);	
		piston3y = GUILayout.VerticalSlider(piston3y, dy_max, -dy_min);		
		DetectOnChange(p3y_old, piston3y);
				
		// handle GUI slider values		
		_1.y = y_init + piston1y;
		p1.position = _1;
		
		_2.y = y_init + piston2y;
		p2.position = _2;
		
		_3.y = y_init + piston3y;
		p3.position = _3;
		*/	
		
		
			GUILayout.BeginVertical();
			Vector3 par_v = Collective.position;
			GUILayout.Label("Collective");
			collectivey = GUILayout.VerticalSlider(collectivey, 0.02f, -.02f);		
        	DetectOnChange(collectivey_old, collectivey);
        	par_v.y = collectivey;
        	Collective.position = par_v;
        	GUILayout.EndVertical();
        
		GUILayout.EndHorizontal();
		
		//rotation slider
		GUILayout.BeginArea(new Rect(0, Screen.height-50, 200, 50) );
		GUILayout.Label("Rotate");
		slidersp = GUILayout.HorizontalSlider(slidersp, 0, sp_max, new GUIStyle("box"), new GUIStyle("horizontalsliderthumb"), GUILayout.Width(200));			DetectOnChange(slidersp_old, slidersp);
		if (slidersp_old==slidersp){
			sp = slidersp;// Mathf.Lerp(sp, slidersp,Time.deltaTime);
		  	DoRotate();	
		}
		GUILayout.EndArea();
			
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
