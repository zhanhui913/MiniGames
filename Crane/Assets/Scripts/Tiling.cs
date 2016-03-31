using UnityEngine;
using System.Collections;

[RequireComponent (typeof( SpriteRenderer))]
public class Tiling : MonoBehaviour {

	public int offsetX = 2;             //The offset so that we dont get any weird error

	//These are used for checking if we need to instantiate stuff
	public bool hasRightBuddy = false;  
	public bool hasLeftBuddy = false;   

	public bool reverseScale = false; //Used if the object is not tilable

	private float spriteWidth = 0; //The width of our element
	private Camera cam;
	private Transform myTransform;

	void Awake(){
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x; 
		//Debug.Log ("spriteWidth = "+spriteWidth);
	}
	
	// Update is called once per frame
	void Update () {
		float edgeVisiblePositionRight;
		float edgeVisiblePositionLeft;
		//Does it still need buddies? If not do nothing
		if(!hasLeftBuddy || !hasRightBuddy){
			//Calculate the camera's extend (half the width of what the camera can see in world's coordinates)
			float camHorizontalExtend = cam.orthographicSize * (Screen.width/Screen.height);

			//Calculate the x position where the camera can see the edge of the sprite
			edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;

			Transform newBuddy=null;


			//Checking if we can see the edge of the element and then creating a new buddy
			if((cam.transform.position.x >= edgeVisiblePositionRight - offsetX) && (!hasRightBuddy)){
				newBuddy = createBuddy(1);
				hasRightBuddy = true;
			}else if((cam.transform.position.x <= edgeVisiblePositionLeft + offsetX) && (!hasLeftBuddy)){
				newBuddy = createBuddy(-1);
				hasLeftBuddy =  true;
			}

		}
	}


	//A function that creates a buddy on the side that is needed
	Transform createBuddy(int rightOrLeft){
		//Calculating the new position for the new buddy
		Vector3 newPosition = new Vector3 (myTransform.position.x +(spriteWidth * 2) * rightOrLeft, myTransform.position.y, myTransform.position.z);

		//Casting the newly instantiated item into a Transform type variable
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;

		//If tilable, lets reverse the x size of our object to get rid of ugly seams
		if(reverseScale){
			newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1,newBuddy.localScale.y,newBuddy.localScale.z);
		}

		//Used to keep the hierachy clean
		newBuddy.parent = myTransform.parent;

		if (rightOrLeft > 0) {
			newBuddy.GetComponent<Tiling> ().hasLeftBuddy = true;
		} else {
			newBuddy.GetComponent<Tiling> ().hasRightBuddy = true;
		}
		return newBuddy;
	}
}
