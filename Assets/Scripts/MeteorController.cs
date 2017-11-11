using UnityEngine;

public class MeteorController : SpaceObject {

	public SpaceMeteor biggestMeteor;
	private Transform player;

	public AudioClip newMeteorsClip;
	private bool generateNewMeteors = true;
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

	private void generateMeteor(int amount){
	    float vertExtent = BoundaryChecker.instance.VertExtent;
		float HorzExtent = BoundaryChecker.instance.HorzExtent;
		Vector3 randPos = new Vector3(Random.Range(-HorzExtent,HorzExtent), 
									  Random.Range(-vertExtent,vertExtent),
									  0);	
		for(int i = 0; i<amount;i++){
			Debug.Log(randPos);
			while (!isPositionSafe(randPos)){
			    randPos = new Vector3(Random.Range(-HorzExtent,HorzExtent), 
									  Random.Range(-vertExtent,vertExtent),
									  0);	
			}
			SpaceMeteor meteor = Instantiate(biggestMeteor,randPos,Quaternion.identity);
			meteor.transform.SetParent(this.transform);
		}
	}

	private bool isPositionSafe(Vector3 meteorPosition){
		return Vector3.Distance(meteorPosition,player.position) > safeDistance;
	}

	private bool noMeteorsLeft(){
		return transform.childCount == 0;
	}

}
