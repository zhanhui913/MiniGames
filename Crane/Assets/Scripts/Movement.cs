using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour {
	public string indi;
	public GameObject player;
	
	private int direction = 0;
	private float scaleX;

	//Player Handling
	public float speed = 4;
	public float acceleration = 30;

	//Player Movement
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	private Animator CraneAnimator;

	// Use this for initialization
	void Start () {
		Debug.Log ("movement ftw");
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			targetSpeed = direction * speed;
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);
			
			amountToMove.x = currentSpeed;
			amountToMove.y = 0;
			Move (amountToMove * Time.deltaTime);
		}
	}

	//Might not need this and set avatar variable in update or start instead
	public void setAvatarMovement(GameObject avatar){
		//Get the prefab that was selected by the player
		//string avatar = GameManager.avatar;
		//player = GameObject.Find (avatar);
		player = avatar;
		scaleX = player.transform.localScale.x; 
		
		//Get Animation component
		CraneAnimator = player.GetComponent<Animator>();
	}
	/*
	pub/lic void OnPointerUp(PointerEventData eventData){
		if (GameManager.startGame) {
			CustomMouseUp();
		}
	}

	public void OnPointerDown(PointerEventData eventData){
		if (GameManager.startGame) {
			CustomMouseDown ();
		}
	}*/


	public void CustomMouseDown(){
		//Used to ensure only 1 chevron can be clicked at a time 
		if( GameManager.getChevronActive() == "false"){  

			GameManager.setChevronActive(indi); //Set which direction the chevron is being used.

			//Set animator parameter for avatar "Move" to true
			CraneAnimator.SetBool ("Move",true);
			
			if(indi=="Left"){
				direction = -1;
				player.transform.localScale = new Vector2(-scaleX,player.transform.localScale.y);
			}else if(indi == "Right"){
				direction = 1;
				player.transform.localScale = new Vector2(scaleX,player.transform.localScale.y);
			}

		}
	}

	public void CustomMouseUp(){
		//Used to ensure only 1 chevron can be clicked at a time 
		if( GameManager.getChevronActive() == indi){ 
			//Set animator parameter for avatar "Move" to false
			CraneAnimator.SetBool ("Move",false);
			direction = 0;
			GameManager.setChevronActive ("false");

			//Adding elements into easterEgg array
			//GameManager.addKeyToEasterEgg(indi);
			//Debug.Log ("1) adding key to easter egg "+indi);
		}
	}

	/**
	 * Avatar will bow down facing left/right depending on the direction provided.
	 * (If dir = 1, The user clicked on the right side of the waterContainer)
	 * (If dir = -1, The user clicked on the left side of the waterContainer)
	 * Then depending on what direction the avatar is currently facing, different x scales 
	 * will be applied to it to face the new or same direction.
	 */ 
	public void BowDown(int dir){
		//Only allow user to bow down if the left & right chevron is not active
		if(GameManager.getChevronActive() == "false"){
			float tempXScale= 0;
			if(player.transform.localScale.x < 0){ //If avatar was facing left
				tempXScale = (-1 * dir) * player.transform.localScale.x;
			}else if(player.transform.localScale.x > 0){ //If avatar was facing right
				tempXScale = dir * player.transform.localScale.x;
			}
			
			player.transform.localScale = new Vector2 (tempXScale, player.transform.localScale.y);
			
			//Set animator parameter for avatar "bowDown" to true
			CraneAnimator.SetBool ("BowDown",true);

			GameManager.setChevronActive(indi); //Set which direction the chevron is being used.
		}

		Debug.LogWarning ("bow down direction : "+dir);
	}

	public void BowUp(){

		Debug.LogWarning ("movement bow up - "+GameManager.getChevronActive());


		if(GameManager.getChevronActive() == "false"){
			//Set animator parameter for avatar "bowDown" to false
			CraneAnimator.SetBool ("BowDown",false);
			
			//Adding elements into easterEgg array
			//GameManager.addKeyToEasterEgg("Down");
			//Debug.Log ("2) adding key to easter egg down");


		}

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

	public void Move(Vector2 moveAmount){
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		
		Vector2 finalTransform = new Vector2(deltaX, deltaY);
		
		player.transform.Translate (finalTransform);
	}
}