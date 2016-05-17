using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class FishDetail{
	public GameObject[] fishPrefabList; //The list of fishes
	public GameObject waterContainer;//The parent container where the fish objects should be spawn under
	public GameObject spawnPrefab;//The fishSpawnPoint prefab
	public int numSpawn; //Number of spawns
	public int numFishInSpawn;//Number of fishes around each spawn point.
	public int fishSpawnRadius;//The radius to which the fishes will spawn within.
	public float offsetSpawnFromSide;//The offset from the left & right x coordinate for the fish spawn points

	[HideInInspector]
	public List<GameObject> fishSpawnList = new List<GameObject> ();//The lists of spawn points prefabs of the fishes.
}

public class GameManager : MonoBehaviour {

	[HideInInspector]
	public static bool startGame = false; //The game has not started yet
	public static GameObject avatar = null;   //What avatar the user selected
	
	public static GameManager self;

	public FishDetail fish;

	public GameObject SnailPrefab;
	private float SnailYMinPos = -1.3f;
	private float SnailYMaxPos = -0.5f;
	private float numberOfRandomSnails = 10;

	public GameObject FrogPrefab;
	private float FrogYPos = -0.75f;
	private float numberOfRandomFrogs = 10;

	public GameObject[] AlgaesPrefabList;
	private float AlgaesYMinPos = -0.04f;
	private float AlgaesYMaxPos = 1.1f;
	private float numberOfRandomAlgaes = 10;

	private static GameObject avatarPopup;  //Avatar selection popup.
	private static GameObject gameOverPopup; //Game over popup.
	private static Text gameOverText;

	private static string chevronActive = "false"; //Will be used to ensure that left/right chevron cannot be clicked simultaneously (left = left chevron is being used, right = right chevron is being used, false = unused)

	private static bool once = false; //Only used to check once
	public GameObject fedoraPrefab;

	public Text foodCountText;
	public const int NUM_FISH = 10;//The number of food required to win the game

	public static Avatar avatarScript{
		get{ 
			return avatar.GetComponent<Avatar> ();
		}	
	}

	void Awake(){
		self = this;
	}
		
	// Use this for initialization
	void Start () {
		avatarPopup = GameObject.FindGameObjectWithTag ("AvatarSelectionPopup");
		gameOverPopup = GameObject.FindGameObjectWithTag ("GameOverPopup");
		gameOverText = gameOverPopup.transform.FindChild ("Frame_Interior/Text").GetComponent<Text>();

		gameOverPopup.SetActive (false);//Must set it active when launching, otherwise we cannot get instance to it.

		//Pause time
		Time.timeScale = 0;


		//create a number spawn points specified by the user in the list fish.numSpawn
		createFishSpawnPoints (fish.numSpawn);

		spawnOthers ();

		updateFoodCount (0);
	}

	// Update is called once per frame
	void Update () {
		//After 2 seconds
		if(startGame && !once && (Time.deltaTime* 1000 > 2)){ 
			GameObject fedora = (GameObject)Instantiate(fedoraPrefab);
			fedora.transform.parent = fish.waterContainer.transform;
			fedora.transform.localPosition = new Vector2(0,10);
			once = true;
		}
	}
		
	/**
	 * This function selects "num" fish spawn points that is equally distanced from each other.
	 */ 
	public void createFishSpawnPoints(int num){
		//Get the distance that each spawn point should exist within each other.
		float distance = getWaterContainerWidth () / num;
		float currentSpawnPoint = getFirstChildXPosRelParent(fish.waterContainer);

		for(int i=0;i<num;i++){
			if(i == 0){
				currentSpawnPoint += fish.offsetSpawnFromSide;
				fish.fishSpawnList.Add ((GameObject)Instantiate(fish.spawnPrefab));
			}else{
				currentSpawnPoint += distance;
				fish.fishSpawnList.Add ((GameObject)Instantiate (fish.spawnPrefab));
			}

			//Get random y position relative to water container's position
			float y = UnityEngine.Random.Range (0f,0.5f);

			//Put the object under the parent WaterContainer
			fish.fishSpawnList[i].transform.parent = fish.waterContainer.transform;
			fish.fishSpawnList[i].transform.localPosition = new Vector3(currentSpawnPoint,y,0f);

			//Create fishes around each spawnPoint
			for(int a =0;a<fish.numFishInSpawn;a++){
				spawnFish(fish.fishSpawnList[i]);
			}
		}
	}

	/**
	 * Spawn small/big fish inside the spawn container 
	 */
	public void spawnFish(GameObject spawnObject){
		//Randomly spawn fishes of different size
		int fishIndex = UnityEngine.Random.Range (0,2); //Because UnityEngine.Random.Range is used with integers, it returns a number from min to max-1.

		//Get random x and y position relative to the spawn point's position.
		float x = UnityEngine.Random.Range (-fish.fishSpawnRadius, fish.fishSpawnRadius);
		float y = UnityEngine.Random.Range (-0.85f,-1.35f);

		//Create random fish direction
		int direction = UnityEngine.Random.Range (0,2); //Because UnityEngine.Random.Range is used with integers, it returns a number from min to max-1.

		GameObject f = (GameObject)Instantiate (fish.fishPrefabList[fishIndex]);
		f.transform.parent = spawnObject.transform;
		f.transform.localPosition = new Vector3 (x,y,0f);
		if (direction == 0) {
			f.transform.localScale = new Vector3 (f.transform.localScale.x, f.transform.localScale.y,f.transform.localScale.z); //Facing the right
		} else {
			f.transform.localScale = new Vector3 (-f.transform.localScale.x, f.transform.localScale.y, f.transform.localScale.z);//Facing the left
		}
	}

	/**
	 * Spawn algaes, frogs, and snails
	 */ 
	public void spawnOthers(){
		float width = getWaterContainerWidth ();
		float xMin = - (width / 2);
		float xMax = width / 2;

		//Spawn Algaes
		for(int a = 0; a < numberOfRandomAlgaes ;a++){
			int index = UnityEngine.Random.Range (0,AlgaesPrefabList.Length); //Because UnityEngine.Random.Range is used with integers, it returns a number from min to max-1.
			spawnAlgae(index, xMin, xMax, AlgaesYMinPos, AlgaesYMaxPos);
		}

		//Spawn Frogs
		for(int f = 0; f < numberOfRandomFrogs ;f++){
			spawnFrog(xMin, xMax, FrogYPos);
		}

		//Spawn Snails
		for(int s = 0; s < numberOfRandomSnails ;s++){
			spawnSnail(xMin, xMax, SnailYMinPos, SnailYMaxPos);
		}
	}
		
	/**
	 * Spawns 1 of 3 different algaes depending on index at random x and y position (within the constraints provided in parameter)
	 */ 
	public void spawnAlgae(int index, float xMin, float xMax, float yMin, float yMax){
		float x = UnityEngine.Random.Range (xMin,xMax);
		float y = UnityEngine.Random.Range (yMin,yMax);
		float direction = UnityEngine.Random.Range (0,2);

		GameObject a = (GameObject)Instantiate (AlgaesPrefabList[index]);
		a.transform.parent = fish.waterContainer.transform;
		a.transform.localPosition = new Vector2 (x,y);
		if(direction == 0){ //Have it face left
			a.transform.localScale = new Vector2(a.transform.localScale.x,a.transform.localScale.y);
		}else if(direction == 1){ //Have it face right
			a.transform.localScale = new Vector2(-a.transform.localScale.x,a.transform.localScale.y);
		}
	}

	/**
	 * Spawn frogs at random x position (within the constraints provided in parameter).
	 * The y position is fixed as frogs float at water level.
	 */ 
	public void spawnFrog(float xMin, float xMax, float yFix){
		float x = UnityEngine.Random.Range (xMin,xMax);
		float y = yFix;
		float direction = UnityEngine.Random.Range (0,2);
		
		GameObject f = (GameObject)Instantiate (FrogPrefab);
		f.transform.parent = fish.waterContainer.transform;
		f.transform.localPosition = new Vector2 (x,y);
		if(direction == 0){ //Have it face left
			f.transform.localScale = new Vector2(f.transform.localScale.x,f.transform.localScale.y);
		}else if(direction == 1){ //Have it face right
			f.transform.localScale = new Vector2(-f.transform.localScale.x,f.transform.localScale.y);
		}
	}

	/**
	 * Spawn snails at random x and y position (within the constraints provided in parameter)
	 */ 
	public void spawnSnail(float xMin, float xMax, float yMin, float yMax){
		float x = UnityEngine.Random.Range (xMin, xMax);
		float y = UnityEngine.Random.Range (yMin,yMax);
		float direction = UnityEngine.Random.Range (0,2);
		
		GameObject s = (GameObject)Instantiate (SnailPrefab);
		s.transform.parent = fish.waterContainer.transform;
		s.transform.localPosition = new Vector2 (x,y);
		if(direction == 0){ //Have it face left
			s.transform.localScale = new Vector2(s.transform.localScale.x,s.transform.localScale.y);
		}else if(direction == 1){ //Have it face right
			s.transform.localScale = new Vector2(-s.transform.localScale.x,s.transform.localScale.y);
		}
	}

	/**
	 * Get waterContainer's width 
	 */ 
	private float getWaterContainerWidth(){
		GameObject waterContainer = GameObject.Find ("WaterContainer");
		
		//Get number of children with the tag "WaterTag"
		int childCount = GameObject.FindGameObjectsWithTag ("WaterTag").Length - 1; //Because the width is always n - 1
		
		//Get the size of 1 child's spriteRenderer's width. 
		//(Multiply by 2 because it returns just half the width from the pivot point which is located in the center)
		float childWidth = (waterContainer.GetComponentInChildren<SpriteRenderer> ().sprite.bounds.size.x)*2;
		
		return childCount * childWidth;
	}
	
	/**
	 * Get the first child's x position relative to the parent
	 */ 
	private float getFirstChildXPosRelParent(GameObject parent){
		Transform[] transform = parent.GetComponentsInChildren<Transform> ();
		return transform [1].position.x; //The index 0 is reserved for the parent's transform.
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	//These functions below are static functions (to be accessible from other scripts)
	////////////////////////////////////////////////////////////////////////////////////////////////////

	//Set the avatar so that other scripts can access what avatar the user selected
	public static void setAvatar(GameObject avatarObject){
		avatar = avatarObject;
	}

	//Getter foro the avatar variable
	public static GameObject getAvatar(){
		return avatar;
	}

	//Called when game has officially start, immediately after user selects an avatar
	public static void setStartGame(bool boolean){
		startGame = boolean;

		//Set time scale to 1 to continue counting
		Time.timeScale = 1;
	}

	//Disable/Enable the avatar popup
	public static void PopupShow(bool show){
		avatarPopup.SetActive (show);
	}

	//Disable/Enable the GameOver popup
	public static void GameOver(bool show, bool isSuccess){
		gameOverPopup.SetActive (show);

		if (isSuccess) {
			gameOverText.text = "Successful";
		} else {
			gameOverText.text = "Failed";
		}
	}

	//Setter for chevronActive variable
	public static void setChevronActive(string active){
		chevronActive = active;
	}

	//Getter for chevronActive variable
	public static string getChevronActive(){
		return chevronActive;
	}

	public void updateFoodCount(int foodNumber){
		foodCountText.text = "" + foodNumber;

		if (foodNumber >= NUM_FISH) {
			GameManager.setStartGame(false);
			GameManager.GameOver(true, true);
		}
	}

	public void restartLevel(){
		once = false;
		SceneManager.LoadScene (0);
	}
}
