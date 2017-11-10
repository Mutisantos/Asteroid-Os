using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : SpaceObject {

	public SpaceMeteor biggestMeteor;
	public Transform player;

	public AudioClip newMeteorsClip;
	private bool generateNewMeteors = true;
	public float safeDistance = 2f;

	void Awake () {

	}
	void Start(){
		
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
