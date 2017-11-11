using UnityEngine;

public class UFOWaypoints : MonoBehaviour {

	private Transform beginWaypoint;
	private Transform endWaypoint;
	
	void Start () {
		beginWaypoint = GetComponentsInChildren<Transform>()[1];
		endWaypoint = GetComponentsInChildren<Transform>()[2];
	}
	
	void Update () {
		
	}

	public Vector2 getBeginPosition(){
		return beginWaypoint.position;
	}

	public Vector2 getEndPosition(){
		return endWaypoint.position;
	}
}
