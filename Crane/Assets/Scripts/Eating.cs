using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Eating : MonoBehaviour {

	public Text foodCountText;

	private int foodCount = 0;

	void Start(){
		foodCountText.text = "Food Eaten : 0";
	}

	void OnTriggerEnter2D(Collider2D other){
		//Increase energy
		if(other.gameObject.name == "BigFish(Clone)"){
			EnergyBar.addFoodTime (1f);     Debug.Log ("bigfish eaten");
			Destroy(other.gameObject);
			foodCount++;
			foodCountText.text = "Food Eaten : "+foodCount;
		}else if(other.gameObject.name == "SmallFish(Clone)"){
			EnergyBar.addFoodTime (0.5f);    Debug.Log ("smallfish eaten");
			Destroy(other.gameObject);
			foodCount++;
			foodCountText.text = "Food Eaten : "+foodCount;
		}else if(other.gameObject.name == "Fedora(Clone)"){
			Debug.Log ("eaten fedora");
			Destroy(other.gameObject);
			GameObject f = GameObject.Find (GameManager.getAvatar());
			GameObject c = f.transform.Find (GameManager.getAvatar()+"Head").gameObject;
			GameObject fe = c.transform.Find ("Fedora").gameObject;
			fe.SetActive(true);
		}
			
		Debug.Log ("need fish to win : "+GameManager.getNumFoodToWin ());
		if(foodCount == GameManager.getNumFoodToWin()){
			GameManager.setStartGame(false);
			GameManager.GameOver(true);
			Debug.Log ("game overrr");
		}
	}
}
