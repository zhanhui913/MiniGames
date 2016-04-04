using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {

	public LayerMask collisionMask;

	//To know where the collider of the player is 
	private BoxCollider collider;
	private Vector3 size;
	private Vector3 center;

	private float skin = .005f;

	[HideInInspector]
	public bool grounded;

	Ray ray;
	RaycastHit hit;

	void Start(){
		collider = GetComponent<BoxCollider>();
		size = collider.size;
		center = collider.center;
	}

	public void Move(Vector2 moveAmount){
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;

		Vector3 playerPos = transform.position;

		grounded = false;
		//Do ray casting downwards to check for ground
		for(int i = 0; i < 3; i++){
			float direction = Mathf.Sign(deltaY);
			float x = (playerPos.x + center.x - size.x/2) + size.x/2 * i; //Left, center and then rightmost point of collider
			float y = playerPos.y + center.y + size.y/2 * direction; // Bottom of collider
			float z = playerPos.z;

			ray = new Ray(new Vector3(x,y,z), new Vector2(0,direction));
			Debug.DrawRay(ray.origin,ray.direction);
			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaY), collisionMask)){
				//Distance between player position and ground
				float distance = Vector3.Distance(ray.origin,hit.point); 

				//Stop player's downwards movement after coming within skin's width of a collider
				if(distance > skin){
					deltaY = -distance + skin;
				}else{
					deltaY = 0;
				}
				grounded = true;
				break;
			}
		}

		Vector2 finalTransform = new Vector2(deltaX, deltaY);

		transform.Translate (finalTransform);
	}
}
