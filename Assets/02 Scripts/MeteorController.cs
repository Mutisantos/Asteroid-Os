using UnityEngine;

/**Script for handling the meteor/asteroid creation in game 
 * Esteban.Hernandez
 */
public class MeteorController : SpaceObject {
	//Biggest meteor to start every level
	public SpaceMeteor biggestMeteor;
	private Transform player;

	public AudioClip newMeteorsClip;
	private bool generateNewMeteors = true;
	//Distance from the player where the meteors must be created
	public float safeDistance = 2f;

	void Awake () {

	}
	void Start(){
		GameManager.instance.assignMeteorController(this);
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	void Update(){
		checkLimits();
	}
	void LateUpdate () {
		if(generateNewMeteors){
			SoundManager.instance.PlayMeteorOnce(newMeteorsClip);
			generateMeteor(GameManager.instance.getLevel());
			generateNewMeteors = false;
		}
		if(noMeteorsLeft()){
			generateNewMeteors = true;
			GameManager.instance.addLevel();
		}
	}
	
	// Creates new meteors. Every new level the amount increases by 1
	private void generateMeteor(int amount){
	    float vertExtent = BoundaryChecker.instance.VertExtent;
		float HorzExtent = BoundaryChecker.instance.HorzExtent;
		Vector3 randPos = new Vector3();	
		for(int i = 0; i<amount;i++){

			do{
			    randPos = new Vector3(Random.Range(-HorzExtent,HorzExtent), 
									  Random.Range(-vertExtent,vertExtent),
									  0);	
			}while (!isPositionSafe(randPos));
			SpaceMeteor meteor = Instantiate(biggestMeteor,randPos,Quaternion.identity);
			meteor.transform.SetParent(this.transform);
		}
	}

	//Checks if the position of the meteor is too close or not from the player position	
	private bool isPositionSafe(Vector3 meteorPosition){
		return Vector3.Distance(meteorPosition,player.position) > safeDistance;
	}

	private bool noMeteorsLeft(){
		return transform.childCount == 0;
	}
	
	public void clearMeteors(){
		 foreach (Transform child in transform) {
     		GameObject.Destroy(child.gameObject);
 		}
	}

}
