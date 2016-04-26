using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

//Testing purposes only
public class AvatarMenu : MonoBehaviour {

	public List<Avatar> avatarList;

	public AvatarSelection selection;

	void Start () {
		selection.setData (avatarList);
	}

}
