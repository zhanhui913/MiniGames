using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BowDown : MonoBehaviour , IPointerDownHandler, IPointerUpHandler{

	public Movement movement;
	public AVATAR_DIRECTION direction;

	private bool onPressed = false;
	private bool once = false; //Only ensure bowUp happen once after bownDown in the update().
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			//Check for touch input NGUI
			if (onPressed) {
				movement.BowDown((int)direction);
				once = true;
			}else{
				if(once){
					movement.BowUp();
					once = false;
				}
			}
		}
	}

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
