using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeftRightPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private Movement m;
	private bool onPressed = false;

	// Use this for initialization
	void Start () {
		m = GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			//Check for touch input NGUI
			if (onPressed) {
				m.CustomMouseDown ();
			} else {
				m.CustomMouseUp ();
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
