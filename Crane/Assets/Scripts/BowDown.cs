using UnityEngine;
using System.Collections;

public class BowDown : MonoBehaviour {

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
	
	//NGUI uses OnPress, not OnMouseDown
	void OnPress(bool pressed){
		if(GameManager.startGame){
			if(pressed){
				onPressed = true;
			}else{
				onPressed = false;
			}
		}
	}
}
