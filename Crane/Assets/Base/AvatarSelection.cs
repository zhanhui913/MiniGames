﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AvatarSelection :  HorizontalListCreator{

	private List<GameObject> gaList = new List<GameObject>();
	private List<Avatar> avatarList;

	private const int MAX_AVATAR_SCROLL = 4;
	public Sprite blankSprite;

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

		if (avatarList.Count <= MAX_AVATAR_SCROLL && avatarList.Count > 0) { Debug.LogWarning ("there are less than 4");
			//Disable scroll
			this.transform.parent.GetComponent<ScrollRect>().enabled = false;

			//Do not use value "itemWidthPercent" set in public, change its value based on left, right, and middle padding
			float newItemWidthPercent = (1 - padding.left - padding.right - (padding.middle * (avatarList.Count - 1))) / avatarList.Count;

			//float newItemWidthPercent = (1 - padding.left - padding.right - (padding.middle * (MAX_AVATAR_SCROLL - 1))) / MAX_AVATAR_SCROLL;

			Debug.LogWarning("value to use as itemWidthPercent is "+newItemWidthPercent);
			itemWidthPercent = newItemWidthPercent;

			int diff = MAX_AVATAR_SCROLL - avatarList.Count;

			//Create those avatars
			for (int i = 0; i < avatarList.Count; i++) {
				Avatar ava = avatarList [i];
				Transform go = Instantiate (item, Vector3.zero, Quaternion.identity) as Transform;
				go.SetParent (this.parentRect.transform);

				//Set the image of the avatar for the button in the avatar selection menu popup
				go.GetComponent<Image> ().sprite = ava.imageAvatar;

				go.GetComponent<Button> ().onClick.AddListener (() => {
					HideAvatarsExcept (ava);
					ava.SetAvatar ();

					//Let movement script know it can fetch the avatar information
					leftPanel.GetComponent<Movement> ().setAvatarMovement (ava.gameObject);
					rightPanel.GetComponent<Movement> ().setAvatarMovement (ava.gameObject);
				});
				gaList.Add (go.gameObject);
			}

			//Create remaining blank avatars
			for(int i = 0; i < diff; i++){
				Transform go = Instantiate (item, Vector3.zero, Quaternion.identity) as Transform;
				go.SetParent (this.parentRect.transform);

				//Set the image of the avatar for the button in the avatar selection menu popup
				go.GetComponent<Image> ().sprite = blankSprite;

				gaList.Add (go.gameObject);
			}

			base.Reposition (gaList);
		} else if (avatarList.Count > MAX_AVATAR_SCROLL) {
			//Enable scroll
			this.transform.parent.GetComponent<ScrollRect>().enabled = true;

			//Use value "itemWidthPercent" set in public
		
			//Create those avatars
			for (int i = 0; i < avatarList.Count; i++) {
				Avatar ava = avatarList [i];
				Transform go = Instantiate (item, Vector3.zero, Quaternion.identity) as Transform;
				go.SetParent (this.parentRect.transform);

				//Set the image of the avatar for the button in the avatar selection menu popup
				go.GetComponent<Image> ().sprite = ava.imageAvatar;

				go.GetComponent<Button> ().onClick.AddListener (() => {
					HideAvatarsExcept (ava);
					ava.SetAvatar ();

					//Let movement script know it can fetch the avatar information
					leftPanel.GetComponent<Movement> ().setAvatarMovement (ava.gameObject);
					rightPanel.GetComponent<Movement> ().setAvatarMovement (ava.gameObject);
				});
				gaList.Add (go.gameObject);
			}
		} else {
			throw new UnityException ("Need to have at least 1 Avatar data in the list.");
		}
	}

	public void HideAvatarsExcept(Avatar ava){
		for(int i = 0; i < avatarList.Count; i++){
			Avatar avaRef = avatarList [i];

			if(avaRef.GetInstanceID() != ava.GetInstanceID()){
				avaRef.gameObject.SetActive (false);
			}
		}
	}
}
