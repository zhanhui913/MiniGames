using UnityEngine;
using System.Collections;

public class Fedora : MonoBehaviour {

	private float y;

	// Use this for initialization
	void Start () {
		y = transform.localPosition.y;

	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){

			//transform.position = new Vector2 (transform.position.x, -y * Time.deltaTime * 10);
			if(y < 0.22){

				Debug.Log ("stop updating fedora position");
			}else{
				y -= Time.deltaTime ;
				this.transform.localPosition = new Vector2(transform.localPosition.x,y);
				Debug.Log ("updating fedora position");
			}
		}
	}
}
