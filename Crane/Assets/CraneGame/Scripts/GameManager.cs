using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[HideInInspector]
	public static bool startGame = false; //The game has not started yet
	public static GameObject avatar = null;   //What avatar the user selected
	
	public static GameManager self;

	public GameObject[] fishPrefabList; //The list of fishes
	public GameObject targetFishSpawnContainer; //The parent container where the fish objects should be spawn under
	public float numFish;       //Number of fishes around each spawn point.
	public float fishSpawnRadius = 10; //The radius to which the fishes will spawn within.

	public GameObject fishSpawnPrefab;  //The fishSpawnPoint prefab
	public GameObject[] fishSpawnPointsPrefabList; //The lists of spawn points prefabs of the fishes.
	public float offsetFromSide;        //The offset from the left & right x coordinate for the fish spawn points

	public float numberOfRandomSnails = 10;
	public float numberOfRandomFrogs = 10;
	public float numberOfRandomAlgaes = 10;

	public GameObject SnailPrefab;
	public float SnailYMinPos = -1.3f;
	public float SnailYMaxPos = -0.5f;

	public GameObject FrogPrefab;
	public float FrogYPos = -0.75f;

	public GameObject[] AlgaesPrefabList;
	public float AlgaesYMinPos = -0.04f;
	public float AlgaesYMaxPos = 1.1f;

	public const int NUM_FISH = 10;//The number of food required to win the game

	private static GameObject avatarPopup;  //Avatar selection popup.
	private static GameObject gameOverPopup; //Game over popup.
	private static GameObject fader;
	private static string chevronActive = "false"; //Will be used to ensure that left/right chevron cannot be clicked simultaneously (left = left chevron is being used, right = right chevron is being used, false = unused)

	private static bool once = false; //Only used to check once
	public GameObject fedora;

	void Awake(){
		self = this;

		//This code should be in the game
		//gameOverPopup = GameObject.Find ("GameOverPopup"); 
		//gameOverPopup.SetActive (false); //Set it to false immediately after getting instance of this object since cannot get instance to this object if it is originally disabled.
	}

	// Use this for initialization
	void Start () {
		avatarPopup = GameObject.Find ("AvatarSelectionPopup");
		gameOverPopup = GameObject.Find ("GameOverPopup");
		//fader = GameObject.Find ("BackgroundFader");

		gameOverPopup.SetActive (false);//Must set it active when launching, otherwise we cannot get instance to it.

		//Pause time
		Time.timeScale = 0;

		//create a number spawn points specified by the user in the list fishSpawnPointsPrefabList;
		createFishSpawnPoints (fishSpawnPointsPrefabList.Length);

		spawnOthers ();
	}

	// Update is called once per frame
	void Update () {
		//After 2 seconds
		if(!once && (Time.deltaTime* 1000 >2)){
			GameObject fedora1 = (GameObject)Instantiate(fedora);
			//fedora1.SetActive(true);
			
			//Find a random x position in between the current screen
			//float x = ;
			

			//put easterEgg under parent WaterContainer
			fedora1.transform.parent = targetFishSpawnContainer.transform;
			fedora1.transform.localPosition = new Vector2(0,10);
			once = true;
		}
	}
		
	/**
	 * This function selects "num" fish spawn points that is equally distanced from each other.
	 */ 
	public void createFishSpawnPoints(int num){
		targetFishSpawnContainer = GameObject.Find ("WaterContainer");

		//Get the distance that each spawn point should exist within each other.
		float distance = getWaterContainerWidth () / num;
		float currentSpawnPoint = getFirstChildXPosRelParent(targetFishSpawnContainer);

		for(int i=0;i<num;i++){
			if(i==0){
				currentSpawnPoint += offsetFromSide;
				fishSpawnPointsPrefabList[i] = (GameObject)Instantiate(fishSpawnPrefab);
			}else{
				currentSpawnPoint += distance;
				fishSpawnPointsPrefabList[i] = (GameObject)Instantiate(fishSpawnPrefab);
			}

			//Get random y position relative to water container's position
			float y = Random.Range (0f,0.5f);

			//Put the object under the parent WaterContainer
			fishSpawnPointsPrefabList[i].transform.parent = targetFishSpawnContainer.transform;
			fishSpawnPointsPrefabList[i].transform.localPosition = new Vector3(currentSpawnPoint,y,0f);

			//Create numFish of fishes around each spawnPoint
			for(int a =0;a<numFish;a++){
				spawnFish(fishSpawnPointsPrefabList[i]);
			}
		}
	}

	/**
	 * Spawn small/big fish inside the spawn container 
	 */
	public void spawnFish(GameObject spawnObject){
		//Randomly spawn fishes of different size
		int fishIndex = Random.Range (0,2); //Because Random.Range is used with integers, it returns a number from min to max-1.

		//Get random x and y position relative to the spawn point's position.
		float x = Random.Range (-fishSpawnRadius,fishSpawnRadius);
		float y = Random.Range (-0.85f,-1.35f);

		//Create random fish direction
		int direction = Random.Range (0,2); //Because Random.Range is used with integers, it returns a number from min to max-1.

		GameObject f = (GameObject)Instantiate (fishPrefabList[fishIndex]);
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
			int index = Random.Range (0,2); //Because Random.Range is used with integers, it returns a number from min to max-1.
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
	 * Spawns 1 of 2 different algaes depending on index at random x and y position (within the constraints provided in parameter)
	 */ 
	public void spawnAlgae(int index, float xMin, float xMax, float yMin, float yMax){
		float x = Random.Range (xMin,xMax);
		float y = Random.Range (yMin,yMax);
		float direction = Random.Range (0,2);

		GameObject a = (GameObject)Instantiate (AlgaesPrefabList[index]);
		a.transform.parent = targetFishSpawnContainer.transform;
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
		float x = Random.Range (xMin,xMax);
		float y = yFix;
		float direction = Random.Range (0,2);
		
		GameObject f = (GameObject)Instantiate (FrogPrefab);
		f.transform.parent = targetFishSpawnContainer.transform;
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
		float x = Random.Range (xMin, xMax);
		float y = Random.Range (yMin,yMax);
		float direction = Random.Range (0,2);
		
		GameObject s = (GameObject)Instantiate (SnailPrefab);
		s.transform.parent = targetFishSpawnContainer.transform;
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

	//Disable/Enable the Fader popup (along with the fader)
	public static void PopupShow(bool show){
		avatarPopup.SetActive (show);
		//fader.SetActive (show);
	}

	//Disable/Enable the GameOver popup (along with the fader)
	public static void GameOver(bool show){
		gameOverPopup.SetActive (show);
		//fader.SetActive (show);
	}

	//Setter for chevronActive variable
	public static void setChevronActive(string active){
		chevronActive = active;
	}

	//Getter for chevronActive variable
	public static string getChevronActive(){
		return chevronActive;
	}

	public static int getNumFoodToWin(){
		return NUM_FISH;
	}

	//Remove when done testing (done include in production)
	public void restartLevel(){
		Application.LoadLevel(0);
	}
}
