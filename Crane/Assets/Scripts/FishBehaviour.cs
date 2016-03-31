using UnityEngine;
using System.Collections;

public class FishBehaviour : MonoBehaviour {
	
	private int direction;
	private float speed;

	//Fish Movement
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	public float maxSpeed = 2.0f;
	public float minSpeed = 0.5f;

	//Container min/max x
	private float waterMax = 0f;
	private float waterMin = 0f;
	private int c = 0; //Use to ensure 1 time execution only

	// Use this for initialization
	void Start () {
		//Get the current direction
		if (gameObject.transform.localScale.x > 0) { //Facing right
			direction = 1;
		} else if(gameObject.transform.localScale.x < 0) { //Facing left
			direction = -1;
		}


		speed = Random.Range (minSpeed, maxSpeed);
		targetSpeed = direction * speed;
		//Debug.Log ("speed is "+speed+", targetspeed = "+targetSpeed);

		//Gets the max/min x position that will be used by the fish.
		waterMax = getWaterContainerWidth ()/2;
		waterMin = -waterMax;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			amountToMove.y = 0;

			//If fish is near either side of the end, change direction
			if((this.transform.position.x > waterMax - 0.5) || (this.transform.position.x < waterMin + 0.5)){
				if(c == 0){
					this.transform.localScale = new Vector3(this.transform.localScale.x * -1,this.transform.localScale.y,this.transform.localScale.z);
					targetSpeed = targetSpeed * -1;
					direction = direction * -1;
					c = 1;
				}
			}else{
				c = 0;
			}
			amountToMove.x = targetSpeed;
			Move (amountToMove);
		}
	}

	//Increment n towards target by speed
	private float IncrementTowards(float n, float target){
		if(n == target){
			return n;
		}
		else{
			float dir = Mathf.Sign(target - n); //Must n be increased or decreased to get closer to target
			n +=  Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - n))? n : target; //If n has now passed target then return target, otherwise return n
		}
	}
	
	public void Move(Vector2 moveAmount){
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		
		Vector2 finalTransform = new Vector2(deltaX, deltaY);
		
		this.transform.Translate (finalTransform);
	}

	/**
	 * Get waterContainer's width 
	 */ 
	private float getWaterContainerWidth(){
		GameObject waterContainer = GameObject.Find ("WaterContainer");
		
		//Get number of children with the tag "WaterTag"
		int childCount = GameObject.FindGameObjectsWithTag ("WaterTag").Length - 1; //Because the width is always n - 1
		
		//Get the size of 1 child's spriteRenderer's width. 
		//(Multiply by 2 because it returns just half the width from the pivot point which is located in the center)
		float childWidth = (waterContainer.GetComponentInChildren<SpriteRenderer> ().sprite.bounds.size.x)*2;
		
		return childCount * childWidth;
	}
}
