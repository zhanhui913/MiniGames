using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour {

	private float second = 0;
	public Text timerText; 
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			second += Time.deltaTime;

			TimeSpan t = TimeSpan.FromSeconds(Mathf.Floor(second));

			string answer = string.Format("{0:D2}:{1:D2}:{2:D2}", 
				t.Hours, 
				t.Minutes, 
				t.Seconds);

			timerText.text = answer;
		}
	}

	public float getTime(){
		return second;
	}
}
