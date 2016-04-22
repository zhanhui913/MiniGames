using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AvatarSelection :  HorizontalListCreator{

	private List<GameObject> gaList = new List<GameObject>();

	void Awake(){
		base.Awake ();
	}

	void Start(){
		base.Start();
	}

	public void setData(List<GameObject> list){
		for(int i = 0; i < list.Count; i++){
			Transform t = Instantiate(item, Vector3.zero, Quaternion.identity) as Transform;

			//t.GetComponent<PoliceItem> ().item = t;
			gaList.Add(t.gameObject);

			t.SetParent (this.parentRect.transform);
		}


		base.Reposition (gaList);
	}
}
