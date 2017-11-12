using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Script that describes the behaviour of an Enemy UFO
 * Esteban.Hernandez
 */
public class UFO : SpaceObject {

	public SpaceBullet[] prefabBullets;
	public AudioClip deadClip;	
	private Animator animator;
	private Rigidbody2D ufobody;
	private Transform ufoDirection;
	private Vector2 beginPosition;
	private Vector2 endPosition;

	public float speed;
	public int points;
	public float minCooldown;
	public float maxCooldown;
	private bool alive;
	private float cooldownTimer;
	
	void Start () {
		ufobody = GetComponent<Rigidbody2D> ();
		animator = GetComponentsInChildren<Animator> ()[0];
		ufoDirection = GetComponentsInChildren<Transform>()[0];
		alive = true;
	}
	
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

	//Randomize the fire rate of the UFO, making the shots randomly
	private void randomizeCooldown(){
		cooldownTimer = Random.Range(minCooldown,maxCooldown);
	}

	//Sets the beginning and ending position of the UFO
	public void setWaypointPositions(Vector2 begin, Vector2 end){
		beginPosition = begin;
		endPosition = end;
	}
	//Moves the UFO towards the ending position
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
