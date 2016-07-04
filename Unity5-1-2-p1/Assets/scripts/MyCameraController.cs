using UnityEngine;
using System.Collections;

public class MyCameraController : MonoBehaviour
{
	private KGFOrbitCam itsKGFOrbitCam;	//reference to the orbitcam		
	
	public void Start()
	{
		itsKGFOrbitCam = KGFAccessor.GetObject<KGFOrbitCam>();	//get the KGFOrbitcam by using the KGFAccessor class
	}
	
	void Update(){
		
		if(itsKGFOrbitCam != null)
		{
			float curZoom = ( itsKGFOrbitCam.GetZoom() );
            float maxZoom = ( itsKGFOrbitCam.GetZoomMaxLimit() );
            float minZoom = ( itsKGFOrbitCam.GetZoomMinLimit() );
            float speedZoom = 0.05f*( itsKGFOrbitCam.GetZoomSpeed() );
			
			// zoom in - keyboard
			if ( Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Minus)){
            	if (curZoom <= maxZoom && curZoom >= minZoom){
            		itsKGFOrbitCam.SetZoom(curZoom - speedZoom);
            	}else{
            		itsKGFOrbitCam.SetZoom(minZoom + speedZoom);
            	}
			}
			
			//zoom out - keyboard
			if ( Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Plus)){
				
            	if (curZoom <= maxZoom && curZoom >= minZoom){
            		itsKGFOrbitCam.SetZoom(curZoom + speedZoom);
            	}else{
            		itsKGFOrbitCam.SetZoom(maxZoom - speedZoom);
            	}
			}
			
			/* idk why this isnt detected in browser?? 
			if (Input.GetMouseButtonDown(0))
			{
			
				if(Input.mousePosition.x < 200 && Screen.height - Input.mousePosition.y < 130){
        			itsKGFOrbitCam.SetPanningEnable(false);
       	 		}else{
        			itsKGFOrbitCam.SetPanningEnable(true);
        		};
        		
        		string output = "x: " + Input.mousePosition.x + ", " + Input.mousePosition.y;
        		Application.ExternalCall("Output", output);
			}*/

		}
	}
	
	
}
