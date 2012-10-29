using UnityEngine;
using System.Collections;


	

public class Move : MonoBehaviour {
	
	public float baseSpeed = 10.0f;
	public float moveSpeedX = 10.0f;
	public float moveSpeedY = 10.0f;
	public float accelerationX = 1.0f;
	public float accelerationY = 0.0f;
	public float max_accelerationX = 1.0f;
	public float max_accelerationY = 1.0f;
	public float accelerationThreshold = 0.1f;
	
	public float gravity = 0.01f;
	public float resistance = 0.1f;

	public bool ground = false;
	public bool air = false;
	public bool water = true;
	
	public bool surface = false;
	public bool dive = false;
	
	public bool no_vertical_control = false;
	
	
	// COLLISIONS WITH PHYSICS BOXES
	
	void OnTriggerEnter (Collider collider){
			
		if (collider.name == "groundBox"){
			ground = true;
			water = false;
			dive = false;
			no_vertical_control = true;
			Debug.Log ("ground entered");
			}
	
		if (collider.name == "skyBox"){
				air = true;
				water = false;
				surface = false;
				
			
		
			Debug.Log ("air entered");
		}
		
		if (collider.name == "diveBox"){
				dive = true;
				
				Debug.Log ("dive entered");
		
				}	
	}
	

	void OnTriggerExit (Collider collider){
			
		if (collider.name == "groundBox"){
			ground = false;
			water = true ;
			dive = true ;
			
			Debug.Log ("ground exited");
			}
	
		if (collider.name == "skyBox"){
			air = false;
			water = true;
			surface = true ;
			
			Debug.Log ("air exited");
			}	
		
		if (collider.name == "diveBox"){
				dive = false;
				no_vertical_control = false;
				Debug.Log ("dive exited");
		
				}
		}
	
	
	// Use this for initialization
	void Start () {
		
	moveSpeedX = baseSpeed * accelerationX;
	moveSpeedY = baseSpeed * accelerationY;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	
		
	
		//if (Input.GetAxis ("Vertical") > 0){
		//transform.position += new Vector3(0, (moveSpeed + accelerationY) * Time.deltaTime, 0);
		//}
		
		// INPUT CONTROLS
		
		if (!no_vertical_control){
			if (!air){
				if (Input.GetAxis ("Vertical") > 0){
					
						// accelerate progressively until you reach the maximum allowed.
						if (accelerationY < max_accelerationY){
							accelerationY += 0.2f;
						}
						// conservation de l'hyper accélération quand on change de direction
						//else if (accelerationY < -max_accelerationY){
						//	accelerationY = -accelerationY;	
						//}		
					
				}
				
				if (Input.GetAxis ("Vertical") < 0){
					if (water){
						
					accelerationY = -1.0f;	
					}	
					
					else if (dive){
					accelerationY = -0.25f;
					}	
					
					else{
					accelerationY = 0.0f;	
					}
				}
			}
		}
		
		if (Input.GetAxis ("Horizontal") < 0){
		transform.Translate((moveSpeedX * accelerationX) * Time.deltaTime,0, 0);
		}
		
		if (Input.GetAxis ("Horizontal") > 0){
		transform.Translate(-(moveSpeedX * accelerationX) * Time.deltaTime,0, 0);
		}
		
	//PHYSICS
		
	// if you jump outside of the sea, you'll be subject to gravity which will force you back into water shortly
	if (air){	
		accelerationY -= gravity;	
		}
	
	// when hitting the ground, you cannot go down anymore: acceleration becomes zero if you try to	
	if (ground){		
			if (accelerationY < -accelerationThreshold){
				accelerationY = 0.0f;
			}
		}
			
		
	// physics properties of water (normal conditions of the game): acceleration is progressively nullifying	
	if (Input.GetAxis ("Vertical") == 0){	
		
			
		// this triggers the comfort force that pulls you back up to the water when you were sticking the ground
		if (ground){
			dive = true;	
			}
			
		// comfort force of the dive
		if (dive){
			accelerationY = 0.25f;	
			}
			
		// the environment resistance (water) progressively reduces acceleration to 0
		if (water && !dive){
				
			if (accelerationY > accelerationThreshold){ 
				accelerationY -= resistance;
				}
			
			else if (accelerationY < -accelerationThreshold){ 
				accelerationY += resistance;
				}
			else{
				accelerationY = 0.0f;	
				}
			}			
	}
		
		
	// APPLY THE ACCELERATION TO THE SPEED	
	moveSpeedY = baseSpeed * accelerationY;
		
	// TRANSLATE THE MODEL
	transform.Translate(0,(baseSpeed * accelerationY) * Time.deltaTime,0);	
		
		
		
	}
	
}
