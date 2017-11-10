﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBullet : SpaceObject {

	public int power; 
	public int speed;

	public float timeAlive;

	public AudioClip clip;
	private Rigidbody2D bulletBody;

	void Awake(){
		bulletBody = GetComponent<Rigidbody2D> ();	
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
			GameManager.instance.clearMultiplier();
			expire();
		}
	}

	private void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "EnemyBody") {
			Debug.Log("Meteorito");
			expire();
		}
	}
	private void expire(){
		GameManager.instance.addShots();
		Destroy(gameObject);
	}
}
