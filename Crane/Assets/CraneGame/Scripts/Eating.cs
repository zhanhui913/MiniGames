using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Eating : MonoBehaviour {

	public Text foodCountText;

	private int foodCount = 0;
	private const string BIG_FISH = "BigFish";
	private const string SMALL_FISH = "SmallFish";
	private const string FEDORA = "Fedora";

	void Start(){
		foodCountText.text = "Food Eaten : 0";
	}

	void OnTriggerEnter2D(Collider2D other){
		//Increase energy
		if(other.tag == BIG_FISH || other.tag == SMALL_FISH){
			if (other.tag == BIG_FISH) {
				EnergyBar.addFoodTime (1f);	
			} else {
				EnergyBar.addFoodTime (0.5f);	
			}

			Destroy(other.gameObject);
			foodCount++;
			foodCountText.text = "Food Eaten : "+foodCount;
		}else if(other.tag == FEDORA){
			Destroy(other.gameObject);
			transform.FindChild ("Fedora").gameObject.SetActive (true);
		}
			
		Debug.Log ("need fish to win : "+GameManager.getNumFoodToWin ());
		if(foodCount == GameManager.getNumFoodToWin()){
			GameManager.setStartGame(false);
			GameManager.GameOver(true);
			Debug.Log ("game overrr");
		}
	}
}
