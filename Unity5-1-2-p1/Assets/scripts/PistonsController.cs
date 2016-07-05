using UnityEngine;
using System.Collections;

public class PistonsController : MonoBehaviour {
	
	
	// UI - controls 
	public GUISkin skin;
	public KGFOrbitCam itsKGFOrbitCam;	//reference to the orbitcam		
	public RadialSlider r_slide;
	public Transform control_helper; 
	private float collectivey = 0;
	private float slidersp = 0;
	private float collectivey_old = 0;
    private float slidersp_old = 0;
	private float sp_max = 1000;
		
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
	public float dy_max = 0.02f;
	public float dy_min = -0.015f;
	
	private Vector3 targy;
	
	// Use this for initialization
	void Start () {
	
		Vector3 _1 = p1.position;
        Vector3 _2 = p2.position;
        Vector3 _3 = p3.position;
                
		plane = new Plane(_1, _2, _3);
		
		targy = control_helper.position;
        y_init = targy.y;
        
		/*gameObject.AddComponent<MeshFilter>();
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
           
		if (control_helper){
			control_helper.rotation = Quaternion.Euler(0, r_slide.ang, r_slide.rad*8);
			targy.y = y_init + collectivey;
			control_helper.position = Vector3.MoveTowards(control_helper.position, targy, 100);		
		}
 
	  	plane.Set3Points(_1, _2, _3);
	  	
        // set plane
	  	Vector3 newUp = Vector3.RotateTowards(targetObj.up, plane.normal, adjustSpeed*Mathf.Deg2Rad, 0);
      	targetObj.rotation = Quaternion.FromToRotation(transform.up, plane.normal);
      	
      	// move plate as plane moves
      	Vector3 obj_pos = targetObj.position;
      	obj_pos.y = -1.0f*plane.distance;
      	targetObj.position = obj_pos;
      	              
	}
	
	void LateUpdate(){
		if (!Input.GetMouseButton(0))
		{
        
        	if( (slidersp_old==slidersp) 
        		&& (collectivey_old == collectivey) )
        	{
        		itsKGFOrbitCam.SetPanningEnable( true );
       	 	};

        	slidersp_old = slidersp;
        	collectivey_old = collectivey;
		}
	}
		
	private void DoRotate(){
		
		foreach(Transform t in rotateObjects){
			t.localRotation = Quaternion.Euler(0, sp*Time.time, 0);	
		}
	}
		
	void OnGUI(){

        // draw GUI elements        
        GUI.skin = skin;
        GUILayout.BeginHorizontal();
      				
			GUILayout.BeginVertical();
			
			GUILayout.Label("Collective");
			collectivey = GUILayout.VerticalSlider(collectivey, dy_max, dy_min);		
        	DetectOnChange(collectivey_old, collectivey);
        	
        	GUILayout.EndVertical();
        
		GUILayout.EndHorizontal();
		
		//rotate rotors slider
		GUILayout.BeginArea(new Rect(0, Screen.height-50, 200, 50) );
		GUILayout.Label("Rotate");
		slidersp = GUILayout.HorizontalSlider(slidersp, 0, sp_max, new GUIStyle("box"), new GUIStyle("horizontalsliderthumb"), GUILayout.Width(200));			DetectOnChange(slidersp_old, slidersp);
		if (slidersp_old==slidersp){
			sp = slidersp;// Mathf.Lerp(sp, slidersp,Time.deltaTime);
		  	DoRotate();	
		}
		GUILayout.EndArea();
		
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
