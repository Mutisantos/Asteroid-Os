using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : SpaceObject {

	public SpaceBullet[] prefabBullets;
	public float speed;

	public int points;

	public float minCooldown;
	public float maxCooldown;

	public AudioClip deadClip;
	
	private Animator animator;
	private Rigidbody2D ufobody;

	private Transform ufoDirection;
	private float cooldownTimer;
	[SerializeField]
	private Vector2 beginPosition;
	[SerializeField]
	private Vector2 endPosition;

	private bool alive;
	
	void Start () {
		ufobody = GetComponent<Rigidbody2D> ();
		animator = GetComponentsInChildren<Animator> ()[0];
		ufoDirection = GetComponentsInChildren<Transform>()[0];
		alive = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(alive){
			fire();
			checkLimits();
			movePosition();
		}
	}

	private void fire(){
		cooldownTimer -= Time.deltaTime;
		if(cooldownTimer <= 0){
			randomizeCooldown();
			Quaternion rot = ufoDirection.transform.rotation;
			float z  = rot.eulerAngles.z;
			Quaternion shootDirection = Quaternion.Euler(0,0,-z);
			Instantiate(prefabBullets[Random.Range(0,prefabBullets.Length-1)],transform.position,shootDirection);
		}
	}

	private void randomizeCooldown(){
		cooldownTimer = Random.Range(minCooldown,maxCooldown);
	}

	public void setWaypointPositions(Vector2 begin, Vector2 end){
		beginPosition = begin;
		endPosition = end;
	}
	public void movePosition ()	{
		Vector2 actualPosition = ufobody.position;
		if (Vector2.Distance (actualPosition, endPosition) > 0.01) {
			ufobody.MovePosition (Vector2.MoveTowards (actualPosition, endPosition, speed));
		} 
		else{
			actualPosition = endPosition;
			endPosition = beginPosition;
			beginPosition = actualPosition;
		}
	}

	private void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Shoot") {
			ufobody.velocity = Vector2.zero;
			GameManager.instance.addScore(points);
			StartCoroutine(destroyShip());
		}
	}



	public IEnumerator destroyShip(){		
		animator.SetBool ("alive", false);
		alive = false;
		ufobody.velocity = Vector2.zero;
		yield return new WaitForSeconds (1.05f);
		SoundManager.instance.PlayMeteorOnce (this.deadClip);
		Destroy(gameObject);
	}


}
