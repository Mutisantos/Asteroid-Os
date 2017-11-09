using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBullet : SpaceObject {

	public int power; 
	public int speed;

	public float timeAlive;

	public AudioClip clip;
	private float timeStamp;
	private Rigidbody2D bulletBody;

	void Awake(){
		bulletBody = GetComponent<Rigidbody2D> ();	
		timeStamp = Time.deltaTime ;
	}
	void Start () {
		Vector2 faceDirection = Vector2.up;
		bulletBody.AddRelativeForce( faceDirection * speed );
		SoundManager.instance.PlayOnce (clip);
	}
	
	void FixedUpdate () {
		checkLimits();
		timeAlive -= Time.deltaTime;
		if(timeAlive <= 0){
			expire();
		}
	}

	private void expire(){
		GameManager.instance.addShots();
		Destroy(gameObject);
	}
}
