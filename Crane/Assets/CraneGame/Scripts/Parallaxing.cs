using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds; //Array of all the background/foregrounds to be parallaxed.
	private float[] parallaxScales; //The proportion of the camera's movement to move the background by.
	public float smoothing = 1f;     //How smooth the parallax is going to be (Make sure to set > 0).

	private Transform cam;  //Reference to the main camera's transform.
	private Vector3 previousCamPosition; //The position of the camera in the previous frame.
	
	//Use this to call all the logic before Start() but after all the GameObjects have been set up. Great for references
	void Awake(){
		//Set up the camera reference
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		//The previous frame have the current frame's camera position
		previousCamPosition = cam.position;

		//Get the number of background/foregrounds and put parallaxing into it
		parallaxScales = new float[backgrounds.Length];

		for(int i = 0; i < backgrounds.Length; i++){
			//Assigning corresponding parallaxScales
			parallaxScales[i] = backgrounds[i].position.z * -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//For each background
		for(int i = 0 ;i < backgrounds.Length; i++){
			//The parallax is the opposite from the camera's movement because the previous frame was multiplied by the scale
			float parallax = (previousCamPosition.x - cam.position.x) * parallaxScales[i];

			//Set a target x position which is the current position + the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			//Debug.Log (i+") positions at "+backgroundTargetPosX);
			//Create a target position which is the background/foreground's current position with its target X position
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			//Fade between current position and the target's position using Lerp
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		//Set the previous camera's position to the camera's current position at the end of the frame
		previousCamPosition = cam.position;
	}
}
