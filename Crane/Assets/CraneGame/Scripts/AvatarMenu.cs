using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[Serializable]
public class Character{
	public Button characterButton;
	public GameObject characterObject;
}

public class AvatarMenu : MonoBehaviour {


	public Character craneCharacter;
	public Character flamingoCharacter;


	public GameObject leftPanel, rightPanel;

	// Use this for initialization
	void Start () {
		craneCharacter.characterButton.onClick.AddListener (() => characterSelect(craneCharacter.characterObject));
		flamingoCharacter.characterButton.onClick.AddListener (() => characterSelect(flamingoCharacter.characterObject));
	}

	private void characterSelect(GameObject avatarObject){
		avatarObject.gameObject.SetActive (true);

		GameManager.PopupShow(false);
		GameManager.setStartGame (true);
		GameManager.setAvatar (avatarObject);

		//Let movement script know it can fetch the avatar information now
		leftPanel.GetComponent<Movement> ().setAvatarMovement (avatarObject);
		rightPanel.GetComponent<Movement> ().setAvatarMovement (avatarObject);
	}
}
