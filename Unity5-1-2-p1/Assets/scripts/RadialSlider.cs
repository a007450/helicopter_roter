using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialSlider: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	bool isPointerDown=false;
	public KGFOrbitCam itsKGFOrbitCam;	//reference to the orbitcam	
	
	private RectTransform throttle_rect;
	private RectTransform thisRect;
	
	void Start(){
		GameObject throttle_img = GameObject.FindWithTag("GameController");	
		throttle_rect = throttle_img.GetComponent<RectTransform>();
		thisRect = gameObject.GetComponent<RectTransform>();
	}

	// Called when the pointer enters our GUI component.
	// Start tracking the mouse
	public void OnPointerEnter( PointerEventData eventData )
	{
		StartCoroutine( "TrackPointer" );            
	}
	
	// Called when the pointer exits our GUI component.
	// Stop tracking the mouse
	public void OnPointerExit( PointerEventData eventData )
	{
		StopCoroutine( "TrackPointer" );
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isPointerDown= true;
		//Debug.Log("mousedown");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPointerDown= false;
		//Debug.Log("mousedown");
	}
	public float ang = 0;
	public float rad = 0;
	
	// mainloop
	IEnumerator TrackPointer()
	{
		var ray = GetComponentInParent<GraphicRaycaster>();
		var input = FindObjectOfType<StandaloneInputModule>();

		var text = GetComponentInChildren<Text>();
		
		if( ray != null && input != null )
		{
			while( Application.isPlaying )
			{                    

				// TODO: if mousebutton down
				if (isPointerDown)
				{

					Vector2 localPos; // Mouse position  
					RectTransformUtility.ScreenPointToLocalPointInRectangle( transform as RectTransform, Input.mousePosition, ray.eventCamera, out localPos );
						
					// local pos is the mouse position.
					float angle = (Mathf.Atan2(-localPos.y, localPos.x)*180f/Mathf.PI+180f)/360f;

					GetComponent<Image>().fillAmount = angle;
										
					ang = (int)((angle)*360f );
					
					
					//text.text = ((int)((angle)*360f )).ToString();
					if( localPos.magnitude < (0.43f*thisRect.rect.width)){
						
						throttle_rect.anchoredPosition = localPos;

					}else {
						
						throttle_rect.anchoredPosition = localPos.normalized * (0.4f*thisRect.rect.width);
						
					};	
					
					rad = ( localPos.magnitude ) / (0.5f*thisRect.rect.width);
					
					//Vector3 rot = throttle_rect.localEulerAngles;
					//rot.z = -ang;
					//throttle_rect.localEulerAngles = rot;
					
				}
				itsKGFOrbitCam.SetPanningEnable( !isPointerDown );
				yield return 0;
			}        
		}
		else
			UnityEngine.Debug.LogWarning( "Could not find GraphicRaycaster and/or StandaloneInputModule" );        
	}





}
