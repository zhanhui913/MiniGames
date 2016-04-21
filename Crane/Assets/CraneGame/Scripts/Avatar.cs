using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public enum AVATAR_DIRECTION{
	LEFT = -1,
	RIGHT = 1
}
	
public class Avatar : MonoBehaviour {

	private GameObject player;
	private Animator animator;

	private AVATAR_DIRECTION avatarDirection;

	//Player Handling
	private const float SPEED = 4;
	private const float ACCELERATION = 30;

	private int direction = 0;
	private float scaleX;

	//Player Movement
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		player = gameObject;
		scaleX = player.transform.localScale.x; 
	}

	void Update () {
		if(GameManager.startGame){
			targetSpeed = direction * SPEED;
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed, ACCELERATION);

			amountToMove.x = currentSpeed;
			amountToMove.y = 0;
			Move (amountToMove * Time.deltaTime);
		}
	}

	/**
	 * Avatar will bow down facing left/right depending on the direction provided.
	 * (If dir = AVATAR_DIRECTION.LEFT, The user clicked on the left side of the waterContainer)
	 * (If dir = AVATAR_DIRECTION.RIGHT, The user clicked on the right side of the waterContainer)
	 * Then depending on what direction the avatar is currently facing, different x scales 
	 * will be applied to it to face the new or same direction.
	 */
	public void BowDown(AVATAR_DIRECTION bowDownDirection){ 
		//Only allow user to bow down if the left & right chevron is not active
		if(GameManager.getChevronActive() == "false"){
			float tempXScale= 0;
			if(bowDownDirection == AVATAR_DIRECTION.LEFT){
				tempXScale = -1 * Mathf.Abs(player.transform.localScale.x);
			}else{
				tempXScale = Mathf.Abs(player.transform.localScale.x);
			}

			avatarDirection = bowDownDirection;

			player.transform.localScale = new Vector2 (tempXScale, player.transform.localScale.y);

			//Set animator parameter for avatar "bowDown" to true
			animator.SetBool ("BowDown",true);

			GameManager.setChevronActive(avatarDirection.ToString()); //Set which direction the chevron is being used.
		}
	}

	public void BowUp(){
		//Set animator parameter for avatar "bowDown" to false
		animator.SetBool ("BowDown",false);
	}

	public void Walk(AVATAR_DIRECTION walkDirection){
		//Used to ensure only 1 chevron can be clicked at a time 
		if( GameManager.getChevronActive() == avatarDirection.ToString()){ 
			//Set animator parameter for avatar "Move" to false
			animator.SetBool ("Move",false);
			direction = 0;
			GameManager.setChevronActive ("false");
		}
	}

	public void CustomMouseUp(){
		//Used to ensure only 1 chevron can be clicked at a time 
		if( GameManager.getChevronActive() == avatarDirection.ToString()){ 
			//Set animator parameter for avatar "Move" to false
			animator.SetBool ("Move",false);
			direction = 0;
			GameManager.setChevronActive ("false");
		}
	}

	public void CustomMouseDown(){
		//Used to ensure only 1 chevron can be clicked at a time 
		if( GameManager.getChevronActive() == "false"){  

			GameManager.setChevronActive(avatarDirection.ToString()); //Set which direction the chevron is being used.

			//Set animator parameter for avatar "Move" to true
			animator.SetBool ("Move",true);

			if(avatarDirection == AVATAR_DIRECTION.LEFT){
				direction = -1;
				player.transform.localScale = new Vector2(-scaleX,player.transform.localScale.y);
			}else if(avatarDirection == AVATAR_DIRECTION.RIGHT){
				direction = 1;
				player.transform.localScale = new Vector2(scaleX,player.transform.localScale.y);
			}
		}
	}

	//Increment n towards target by speed
	private float IncrementTowards(float n, float target, float a){
		if(n == target){
			return n;
		}else{
			float dir = Mathf.Sign(target - n); //Must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - n))? n : target; //If n has now passed target then return target, otherwise return n
		}
	}

	public void Move(Vector2 moveAmount){
		//float deltaY = moveAmount.y;
		//float deltaX = moveAmount.x;
		player.transform.Translate (moveAmount);
	}
}
