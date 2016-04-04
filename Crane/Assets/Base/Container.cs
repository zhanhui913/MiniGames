using UnityEngine;
using System.Collections;

//Doesnt seems to be used
public class Container : MonoBehaviour {

	private float childWidth = 0;     //The width of the child's spriteRenderer's width
	private static float containerWidth = 0;  //The width of the container
	private static float containerHeight = 0; //The height of the container (doesnt change)
	private static float containerPosX = 0;   //The x position of the container
	private static float containerPosY = 0;   //The y position of the container (doesnt change)

	// Use this for initialization
	void Start () {
		//Get the size of 1 child's spriteRenderer's width. (Multiply by 2 because it returns just half the width from the pivot point which is located in the center)
		childWidth = (gameObject.GetComponentInChildren<SpriteRenderer> ().sprite.bounds.size.x)*2;

		//Get the container height, since it doesnt change I only need to get it once.
		//Multiply it by 100 since it returns in units per 100 pixels. (might not be true)
		containerHeight = gameObject.GetComponentInChildren<SpriteRenderer> ().sprite.bounds.size.y;

		//Get the container y position, since it doesnt change I only need to get it once.
		containerPosY = gameObject.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.LogWarning ("fds");
		//Update the width of the container as child's count increases.
		//Multiply it by 100 since it returns in units per 100 pixels. (might not be true)
		containerWidth = childWidth * gameObject.transform.childCount;

		//Get the container's x position, since there is parallax affect as the user moves the container moves as well.
		containerPosX = gameObject.transform.position.x;
		Debug.Log ("childCount = "+gameObject.transform.childCount+", childWidth = "+childWidth+", containerPosX = "+containerPosX+", containerWidth = "+containerWidth);
	}

	public static float getContainerWidth(){
		return containerWidth;
	}
	
	public static float getContainerHeight(){
		return containerHeight;
	}

	public static float getContainerPosX(){
		return containerPosX;
	}

	public static float getContainerPosY(){
		return containerPosY;
	}
}
