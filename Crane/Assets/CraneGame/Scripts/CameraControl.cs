using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	//Counter to only process once
	private int counter = 0;

	private Camera cam;
	private GameObject waterContainer;
	private float offsetX = 0.1f;
	private float width; //Width of waterContainer object

	private GameObject rightPanel;
	private GameObject leftPanel;

	private int c = 0;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		waterContainer = GameObject.Find ("WaterContainer");
		width = getWaterContainerWidth (); 
		rightPanel = GameObject.Find ("RightPanel");
		leftPanel = GameObject.Find ("LeftPanel");
	}

	//Used instead of Update so that moving to the left will not have the jagged cloud movement
	void LateUpdate(){
		//If the game has started
		if (GameManager.startGame) {
			if(GameManager.avatar != null){
				//Only enter once as GameObject.Find is an expensive operation
				if(counter == 0){
					counter++;
				}else{
					TrackPlayer ();		
				}
			}
		}
	}

	/**
	 * Will keep camera's x position the same as the avatar. 
	 * Dont allow the avatar to move past the waterContainer's left or right position.
	 */ 
	void TrackPlayer(){
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = GameManager.avatar.transform.position.x;
		
		// Set the camera's position to the target position with the same z component.
		this.transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

		//Calculate the camera's extend (half the width of what the camera can see in world's coordinates)
		float camHorizontalExtend = cam.orthographicSize * (Screen.width/Screen.height);

		//Calculate the x position where the camera can see the edge of the sprite
		float edgeVisiblePositionRight = (waterContainer.transform.position.x + (width/2)) - camHorizontalExtend;
		float edgeVisiblePositionLeft = (waterContainer.transform.position.x - (width / 2)) + camHorizontalExtend;

		//Checking if we can see the edge of the element and then 
		if(cam.transform.position.x >= edgeVisiblePositionRight - offsetX){
			//Disable right panel to indicate the end 
			rightPanel.SetActive(false);

			c = 1;
		}else if(cam.transform.position.x <= edgeVisiblePositionLeft + offsetX){
			//Disable left panel to indicate the end 
			leftPanel.SetActive(false);

			c = 1;
		}else{
			//No need to constantly check, once is enough (Safe GPU power for something else)
			if(c == 1){
				//Enable left & right panel
				rightPanel.SetActive(true);
				leftPanel.SetActive(true);
				c = 0;
			}
		}
	}

	/**
	 * Get waterContainer's width 
	 */ 
	private float getWaterContainerWidth(){
		//Get number of children with the tag "WaterTag"
		int childCount = GameObject.FindGameObjectsWithTag ("WaterTag").Length - 1; //Because the width is always n - 1

		//Get the size of 1 child's spriteRenderer's width. 
		//(Multiply by 2 because it returns just half the width from the pivot point which is located in the center)
		float childWidth = (waterContainer.GetComponentInChildren<SpriteRenderer> ().sprite.bounds.size.x)*2;
		
		return childCount * childWidth;
	}
}
