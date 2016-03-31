using UnityEngine;
using System.Collections;

public class LeftRightPanel : MonoBehaviour {

	public GameObject panel;

	private Movement m;
	private bool onPressed = false;

	// Use this for initialization
	void Start () {
		m = panel.GetComponent<Movement> ();
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
