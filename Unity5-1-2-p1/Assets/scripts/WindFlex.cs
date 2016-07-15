using UnityEngine;
using System.Collections;

// attach to Prop holder/Prop pivot/prop mesh holder
public class WindFlex : MonoBehaviour {
	public PistonsController script;
	public float ang = 0;  // flex angle
	public Transform ref_t;
	
	public float deltay;
	
	private bool onoff = false;
	private Vector3 v_ang;
	
	SpriteRenderer[] sprites;
	private SpriteRenderer sprite_top;
	private SpriteRenderer sprite_bot;
	
	void Start () {
		v_ang = transform.eulerAngles;
		
		sprites = transform.GetComponentsInChildren<SpriteRenderer>();
		
		if (sprites.Length>0){
			
			sprite_top = sprites[0];
			sprite_bot = sprites[1];

		} 
	}

	void LateUpdate () {
		Color top_a;
		Color bot_a;
		
		v_ang = transform.eulerAngles;
		
		deltay =transform.eulerAngles.x; 
		
		onoff = script.WindFx;
		if (onoff){
			// pointing down
			if (deltay<180){
				ang = (deltay) * .2f;
			}else{
			// pointing up
				ang = (deltay-360) * .2f;				
			}
			
			if (sprites.Length>0){
				top_a = sprite_top.color;
				bot_a = sprite_bot.color;
				top_a.a =  Mathf.Clamp(ang, 0.05f, 1);
				bot_a.a =  Mathf.Clamp(-(ang-.5f), 0.1f, 1);
				sprite_top.color = top_a;
				sprite_bot.color = bot_a;
			}
		}else{
			ang = 0;
				if (sprites.Length>0){
					top_a = sprite_top.color;
					bot_a = sprite_bot.color;
					top_a.a = 0;
					bot_a.a = 0;
					sprite_top.color = top_a;
					sprite_bot.color = bot_a;
				}
		}
		
		
		v_ang.z = ang;// = transform.localEulerAngles
		transform.eulerAngles = v_ang;
		
		
	}
}
