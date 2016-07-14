using UnityEngine;
using System.Collections;

public class PistonsController : MonoBehaviour {
	
	
	// UI - controls 
	public GUISkin skin;
	public KGFOrbitCam itsKGFOrbitCam;	//reference to the orbitcam		
	public RadialSlider r_slide;
	public Transform control_helper; 
	public Transform fixed_cam;
	private float collectivey = 0;
	private float slidersp = 0;
	private float collectivey_old = 0;
    private float slidersp_old = 0;
	private float sp_max = 300;
	
	private Vector3 fc_init;
	private float slider_fixedcam = 0;
	private float slider_fixedcam_min = -2.2f;
	private float slider_fixedcam_max = 2.2f;
	private float slider_fixedcam_old;
	
	// rotation 
	public Transform[] rotateObjects;
	public bool WindFx = false;
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
	public float dy_max = 0.012f;
	public float dy_min = -0.01f;
	public float ang_max = 20f;
	
	private Vector3 targy;
	
	void Start () {
	
		Vector3 _1 = p1.position;
        Vector3 _2 = p2.position;
        Vector3 _3 = p3.position;
                
		plane = new Plane(_1, _2, _3);
		
		targy = control_helper.position;
        y_init = targy.y;
        
        fc_init = fixed_cam.localPosition;
             
	}
	
	void Update () {

		Vector3 _1 = p1.position;
        Vector3 _2 = p2.position;
        Vector3 _3 = p3.position;
           
		if (control_helper){
			control_helper.rotation = Quaternion.Euler(0, r_slide.ang, r_slide.rad*ang_max);
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
        		&& (collectivey_old == collectivey) 
        		&&(slider_fixedcam_old==slider_fixedcam) )
        	{
        		itsKGFOrbitCam.SetPanningEnable( true );
       	 	};

        	slidersp_old = slidersp;
        	collectivey_old = collectivey;
        	slider_fixedcam_old=slider_fixedcam;
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
				
		// side UI control
		float area_w = 200;
		
		if (slidersp >=0) {
			GUILayout.BeginArea(new Rect(Screen.width - area_w, Screen.height*.22f, area_w, 150), new GUIStyle("window") );
			
			GUILayout.Label("Fixed Position Camera");
			slider_fixedcam = GUILayout.HorizontalSlider(slider_fixedcam, slider_fixedcam_min, slider_fixedcam_max, new GUIStyle("box"), new GUIStyle("horizontalsliderthumb"), GUILayout.Width(area_w-5));	
			GUILayout.Space(10);
			
			WindFx = GUILayout.Toggle(WindFx,"Toggle airflow effect");
			
			GUILayout.Label("Rotor speed");
			
			slidersp = GUILayout.HorizontalSlider(slidersp, 0, sp_max, new GUIStyle("box"), new GUIStyle("horizontalsliderthumb"), GUILayout.Width(area_w-5));			
			
			GUILayout.EndArea();
			
			// handle rotor slider
			DetectOnChange(slidersp_old, slidersp);
			if (slidersp_old == slidersp){
				sp = slidersp;
				DoRotate();	
			}
			
			// handle fixed cam slider
			DetectOnChange(slider_fixedcam_old, slider_fixedcam);
					
			Vector3 newfc_pos = fixed_cam.localPosition;
			newfc_pos.x = slider_fixedcam;
		
			float theta = Vector3.Angle( newfc_pos, fc_init) - Vector3.Angle( fc_init, new Vector3(0,fc_init.y,0));
			float _z = Vector3.Distance(fc_init,new Vector3(0,fc_init.y,0)) * Mathf.Sin(theta * Mathf.Deg2Rad);	
			newfc_pos.z = -Mathf.Abs( _z );
		
			
			fixed_cam.localPosition = Vector3.MoveTowards(fixed_cam.localPosition, newfc_pos, 100);
							
			
			fixed_cam.gameObject.SetActive(true);
		}else{
			fixed_cam.gameObject.SetActive(false);
		}
		
		//rotate rotors slider
		
		
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
