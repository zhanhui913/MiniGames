using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public class starSection{
	public Image star1,star2,star3;
}
	
public class GameOverScript : MonoBehaviour {

	public starSection mainStar, healthStar, timeStar;
	public Image successPanel, failPanel;

	public Text gameOverText;

	public void setGameStatus(bool isSuccess){
		successPanel.gameObject.SetActive (isSuccess);
		failPanel.gameObject.SetActive (!isSuccess);
	}

	public void setSuccessStatus(int health, int time){
		//set health status
		setHealthStar (health);

		//set time status
		setTimeStar(time);

		//set header star status
		setHeaderStar(Mathf.FloorToInt((health + time) / 2.0f));
	}

	private void resetHeaderStar(){
		mainStar.star1.gameObject.SetActive (false);
		mainStar.star2.gameObject.SetActive (false);
		mainStar.star3.gameObject.SetActive (false);
	}

	private void setHeaderStar(int num){
		resetHeaderStar ();

		if(num == 1){
			mainStar.star1.gameObject.SetActive (true);
		}else if(num == 2){
			mainStar.star1.gameObject.SetActive (true);
			mainStar.star2.gameObject.SetActive (true);
		}else if(num == 3){
			mainStar.star1.gameObject.SetActive (true);
			mainStar.star2.gameObject.SetActive (true);
			mainStar.star3.gameObject.SetActive (true);
		} else {
			throw new Exception ("There can only have 3 stars");
		}
	}

	private void resetHealthStar(){
		healthStar.star1.gameObject.SetActive (false);
		healthStar.star2.gameObject.SetActive (false);
		healthStar.star3.gameObject.SetActive (false);
	}

	private void setHealthStar(int num){
		resetHealthStar ();

		if(num == 1){
			healthStar.star1.gameObject.SetActive (true);
		}else if(num == 2){
			healthStar.star1.gameObject.SetActive (true);
			healthStar.star2.gameObject.SetActive (true);
		}else if(num == 3){
			healthStar.star1.gameObject.SetActive (true);
			healthStar.star2.gameObject.SetActive (true);
			healthStar.star3.gameObject.SetActive (true);
		} else {
			throw new Exception ("There can only have 3 stars");
		}
	}

	private void resetTimeStar(){
		timeStar.star1.gameObject.SetActive (false);
		timeStar.star2.gameObject.SetActive (false);
		timeStar.star3.gameObject.SetActive (false);
	}

	private void setTimeStar(int num){
		resetTimeStar ();

		if (num == 1) {
			timeStar.star1.gameObject.SetActive (true);
		} else if (num == 2) {
			timeStar.star1.gameObject.SetActive (true);
			timeStar.star2.gameObject.SetActive (true);
		} else if (num == 3) {
			timeStar.star1.gameObject.SetActive (true);
			timeStar.star2.gameObject.SetActive (true);
			timeStar.star3.gameObject.SetActive (true);
		} else {
			throw new Exception ("There can only have 3 stars");
		}
	}

	public void setGameOverText(string text){
		gameOverText.text = text;
	}
}
