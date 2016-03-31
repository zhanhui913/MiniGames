using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BowDown : MonoBehaviour , IPointerDownHandler, IPointerUpHandler{

	public GameObject panel;
	public int direction; // direction = -1 (left), direction = 1 (right)

	private Movement m;
	private bool onPressed = false;
	private bool once = false; //Only ensure bowUp happen once after bownDown in the update().
	
	// Use this for initialization
	void Start () {
		m = panel.GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			//Check for touch input NGUI
			if (onPressed) {
				m.BowDown(direction);
				once = true;
			}else{
				if(once){
					m.BowUp();
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
