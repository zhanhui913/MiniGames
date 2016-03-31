using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarMenu : MonoBehaviour {

	public Button craneBtn, flamingoBtn;
	public GameObject craneAvatar, flamingoAvatar;
	public GameObject leftPanel, rightPanel;

	// Use this for initialization
	void Start () {
		craneBtn.onClick.AddListener (() => {
			characterSelect("Crane",craneAvatar);
		});

		flamingoBtn.onClick.AddListener (() => {
			characterSelect("Flamingo",flamingoAvatar);
		});
	}
	
	private void characterSelect(string avatar, GameObject avatarGO){
		GameManager.PopupShow(false);
		GameManager.setAvatar(avatar);
		GameManager.setStartGame (true);

		//Let movement script know it can fetch the avatar information now
		leftPanel.GetComponent<Movement> ().setAvatarMovement (avatarGO);
		rightPanel.GetComponent<Movement> ().setAvatarMovement (avatarGO);
	}
}
