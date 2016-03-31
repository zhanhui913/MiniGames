using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float second1 = 0; //First Second count
	private float second2 = 0; //Second Second count
	private float minute1 = 0;  //First Minute count
	private float minute2 = 0;  //Second Minute count
	private UILabel timerText; 
	
	// Use this for initialization
	void Start () {
		timerText = GetComponent<UILabel> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.startGame){
			second1 += Time.deltaTime;

			if(Mathf.Floor (second1) == 10){
				second2++;
				second1 = 0;
			}
			if(second2 == 6){
				minute1++;
				second2 = 0;
				second1 = 0;
			}
			if(minute1 == 10){
				minute2 ++;
				minute1 = 0;
				second2 = 0;
				second1 = 0;
			}
			//THe format will be like 00:00
			timerText.text = minute2+""+minute1+":"+second2+""+Mathf.Floor (second1);			
		}
	}
}
