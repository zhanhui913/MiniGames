using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BowDown : MonoBehaviour , IPointerDownHandler, IPointerUpHandler{

	public AVATAR_DIRECTION direction;

	private bool onPressed = false;
	private bool once = false; //Only ensure bowUp happen once after bownDown in the update().
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			//Check for touch input NGUI
			if (onPressed) {
				GameManager.avatarScript.BowDown (direction);
				once = true;
			}else{
				if(once){
					GameManager.avatarScript.BowUp();
					once = false;
				}
			}
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
