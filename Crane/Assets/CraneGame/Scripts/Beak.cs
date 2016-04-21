using UnityEngine;
using System.Collections;

public class Beak : MonoBehaviour {

	private const string BIG_FISH = "BigFish";
	private const string SMALL_FISH = "SmallFish";
	private const string FEDORA = "Fedora";

	private int foodCount = 0;

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
		}else if(other.tag == FEDORA){
			Destroy(other.gameObject);
			transform.FindChild ("Fedora").gameObject.SetActive (true);
		}
			
		GameManager.self.updateFoodCount (foodCount);
	}
}
