using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {


	public static EnergyBar self;

	public float energy; //How much energy the player will start with

	private Vector2 newXPos; //The new x position of energyBar after change.
	private Vector2 origXPos; //The original x position of energyBar.
	private float energyWidth = 0; //Width of the energyBar after rectTransform transformation.
	private static float foodTime;     //Food time, used to check if energy is depleted or not. (Can decrease as user eats food)


	//For every 1 second, the energyBar should deplete by "offsetWidth" pixels.
	//So that by the end of the amount of seconds set by the variable seconds, the energyBar will be depleted
	private float offsetWidth = 0;


	void Awake(){
		self = this;
	}

	// Use this for initialization
	void Start () { 
		energyWidth = GetComponent<RectTransform> ().rect.width;

		offsetWidth = (energyWidth/energy); 

		origXPos = new Vector2 (transform.position.x, transform.position.y);
		foodTime = 0; //Temporary timer starts at 0
	}
	
	// Update is called once per frame
	void Update () {
		//Once the game has started
		if(GameManager.startGame){
			if (foodTime <= (energy + 0.5)) { //Added 0.5 seconds so the energyBar is not visible at all when the energy depletes.
				foodTime += (Time.deltaTime);

				newXPos = new Vector2(origXPos.x - (foodTime * offsetWidth) , transform.position.y);
				
				//Decrease energy bar position based on seconds left
				transform.position = newXPos;
			}else{
				//Game Over
				GameManager.startGame = false;

				GameManager.GameOver (true);
			}
		}
	}



	public static float getFoodTime(){
		return foodTime;
	}

	public static void addFoodTime(float addition){
		if((foodTime -= addition) >= 0.5){ //Dont allow energy to go past its max capacity
			foodTime -= addition; //Dont change this to +=
		}else{
			foodTime = 0;
		}
	}
}
