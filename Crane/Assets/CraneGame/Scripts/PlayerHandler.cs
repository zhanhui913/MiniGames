using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerHandler : MonoBehaviour {

	//Player Handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
	public float jumpHeight = 12;

	//Player Movement
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	private PlayerPhysics playerPhysics;

	//Can be accessed by other scripts but not in inspector. Default = 0 (not moving), -1 (Left), 1 (Right).
	[HideInInspector]
	public float HorizontalAxis = 0;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
	}
	
	// Update is called once per frame
	void Update () {
		//HorizontalAxis = Input.GetAxisRaw ("Horizontal");
		targetSpeed = HorizontalAxis * speed;
		//targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		Debug.Log ("horizontal "+Input.GetAxisRaw("Horizontal"));
		currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);

		if(playerPhysics.grounded){
			//Reset gravity when touching ground
			amountToMove.y = 0; 

			//Jump
			if(Input.GetButtonDown("Jump")){
				amountToMove.y = jumpHeight;
			}
		}

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove * Time.deltaTime);
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
}
