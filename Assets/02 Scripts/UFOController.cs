using UnityEngine;

public class UFOController : MonoBehaviour {
	
	public GameObject[] ufoPrefabs;
	public float minUfoRespawnTime;
	public float maxUfoRespawnTime;

	public int firstLevelforUFOs=2;
	private UFOWaypoints[] waypoints;
	private float respawnTimer;


	void Start () {
		respawnTimer = maxUfoRespawnTime;
		waypoints = GameObject.FindGameObjectWithTag("WaypointContainer").GetComponentsInChildren<UFOWaypoints>();
	}
	
	void Update () {
		if(GameManager.instance.getLevel() >= firstLevelforUFOs && noUFOSLeft()){
			respawnTimer -= Time.deltaTime;
			if(respawnTimer <= 0 ){
				UFOWaypoints waypoint = waypoints[Random.Range(0,waypoints.Length-1)];
				//Rotate the ship if its attacking from below
				Quaternion direction = Quaternion.Euler(0,0, waypoint.getBeginPosition().y >= 0 ? 180: 0);
				GameObject newUfo = Instantiate(ufoPrefabs[Random.Range(0,ufoPrefabs.Length-1)],waypoint.getBeginPosition(),direction);
				newUfo.GetComponent<UFO>().setWaypointPositions(waypoint.getBeginPosition(), waypoint.getEndPosition());
				newUfo.transform.SetParent(this.transform);
				respawnTimer = Random.Range(minUfoRespawnTime, maxUfoRespawnTime);
			}
		}		
	}

	private bool noUFOSLeft(){
		return transform.childCount == 0;
	}
}
