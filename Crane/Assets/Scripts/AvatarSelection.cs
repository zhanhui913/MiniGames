using UnityEngine;
using System.Collections;

public class AvatarSelection : MonoBehaviour {

	public string avatarSelect;

	private GameObject leftPanel;
	private GameObject rightPanel;

	// Use this for initialization
	void Start () {
		leftPanel = GameObject.Find ("LeftPanel");
		rightPanel = GameObject.Find ("RightPanel");
	}

	void OnClick(){
		Debug.Log ("hello this is new ngui button onclick "+avatarSelect);

		GameManager.PopupShow(false);
		GameManager.setAvatar(avatarSelect);
		GameManager.setStartGame (true);

		//Let movement script know it can fetch the avatar information now
		leftPanel.GetComponent<Movement> ().setAvatarMovement ();
		rightPanel.GetComponent<Movement> ().setAvatarMovement ();

	}
}
