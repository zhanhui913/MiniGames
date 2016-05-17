using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	public AVATAR_DIRECTION aDirection;

	private GameObject player;
	private int direction = 0;
	private float scaleX;

	//Player Handling
	private const float SPEED = 4;
	private const float ACCELERATION = 30;

	//Player Movement
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private bool onPressed = false;

	private const string MOVE = "Move";

	private Animator Animator;
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			targetSpeed = direction * SPEED;
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed, ACCELERATION);
			
			amountToMove.x = currentSpeed;
			amountToMove.y = 0;
			Move (amountToMove * Time.deltaTime);

			if (onPressed) {
				ContinueWalk ();
			} else {
				StopWalk ();
			}
		}
	}

	//Might not need this and set avatar variable in update or start instead
	public void setAvatarMovement(GameObject avatar){ 
		player = avatar;
		scaleX = player.transform.localScale.x; 
		
		//Get Animation component
		Animator = player.GetComponent<Animator>();
	}

	//Increment n towards target by speed
	private float IncrementTowards(float n, float target, float a){
		if(n == target){
			return n;
		}
		else{
			float dir = Mathf.Sign(target - n); //Must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - n))? n : target; //If n has now passed target then return target, otherwise return n
		}
	}

	private void Move(Vector2 moveAmount){
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		
		Vector2 finalTransform = new Vector2(deltaX, deltaY);
		
		player.transform.Translate (finalTransform);
	}

	private void ContinueWalk(){
		//Used to ensure only 1 chevron can be clicked at a time 
		if( GameManager.getChevronActive() == "false"){  

			GameManager.isAvatarMoving = true;

			GameManager.setChevronActive(aDirection.ToString()); //Set which direction the chevron is being used.

			//Set animator parameter for avatar "Move" to true
			Animator.SetBool (MOVE,true);

			if(aDirection == AVATAR_DIRECTION.LEFT){
				player.transform.localScale = new Vector2(-scaleX,player.transform.localScale.y);
			}else if(aDirection == AVATAR_DIRECTION.RIGHT){
				player.transform.localScale = new Vector2(scaleX,player.transform.localScale.y);
			}
			direction = (int)aDirection;
		}
	}

	private void StopWalk(){
		//Used to ensure only 1 chevron can be clicked at a time 
		if( GameManager.getChevronActive() == aDirection.ToString()){ 
			GameManager.isAvatarMoving = false;

			//Set animator parameter for avatar "Move" to false
			Animator.SetBool (MOVE,false);
			direction = 0;
			GameManager.setChevronActive ("false");
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// 
	/// Pointer up and down
	/// 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnPointerUp(PointerEventData eventData){
		if (GameManager.startGame) {
			onPressed = false;
		}
	}

	public void OnPointerDown(PointerEventData eventData){
		if (GameManager.startGame) {
			onPressed = true;
		}
	}
}