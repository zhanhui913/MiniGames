using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AvatarSelection :  HorizontalListCreator{

	private List<GameObject> gaList = new List<GameObject>();
	private List<Avatar> avatarList;

	public GameObject leftPanel, rightPanel;

	public override void Awake(){
		base.Awake ();
	}

	public override void Start(){
		base.Start();
	}

	//Replace the parameter with a list of avatar list view model.
	public void setData(List<Avatar> avatarList){
		this.avatarList = avatarList;

		for(int i = 0; i < avatarList.Count; i++){
			Avatar ava = avatarList [i];
			Transform go = Instantiate(item, Vector3.zero, Quaternion.identity) as Transform;
			go.SetParent (this.parentRect.transform);
			Debug.LogWarning ("ava ref "+ava.nameForDebug);

			//Set the image of the avatar for the button in the avatar selection menu popup
			go.GetComponent<Image> ().sprite = ava.imageAvatar;

			go.GetComponent<Button> ().onClick.AddListener (() => {
				Debug.LogWarning ("ava click "+ava.nameForDebug);
				HideAvatarsExcept(ava);
				ava.SetAvatar();

				//Let movement script know it can fetch the avatar information
				leftPanel.GetComponent<Movement> ().setAvatarMovement (ava.gameObject);
				rightPanel.GetComponent<Movement> ().setAvatarMovement (ava.gameObject);
			});

			gaList.Add(go.gameObject);
		}
			
		base.Reposition (gaList);
	}

	public void HideAvatarsExcept(Avatar ava){
		Debug.LogWarning ("dont hide "+ava.nameForDebug);
		for(int i = 0; i < avatarList.Count; i++){
			Avatar avaRef = avatarList [i];

			Debug.LogWarning ("comparing ref("+avaRef.GetInstanceID()+") "+avaRef.nameForDebug +" with ("+ava.GetInstanceID()+") "+ava.nameForDebug);

			if(avaRef.GetInstanceID() != ava.GetInstanceID()){
				Debug.LogWarning ("hide avatar "+ava.nameForDebug);	
				avaRef.gameObject.SetActive (false);
			}
		}
	}
}
