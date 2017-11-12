using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Script that represents the behaviour of a space bullet
 * Esteban.Hernandez
 */
public class SpaceBullet : SpaceObject {

	public AudioClip clip;
	private Rigidbody2D bulletBody;
	public int power; 
	public int speed;
	public bool isEnemyBullet;
	public float timeAlive;
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
		if((timeAlive <= 0 && !isEnemyBullet)|| !GameManager.instance.isAlive()){
			GameManager.instance.clearMultiplier();
			GameManager.instance.addScore(-1);//Decrease score if the shot failed or th
			expire();
		}
	}

	private void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "EnemyBody" && !isEnemyBullet) {
			expire();
		}
		else if (coll.gameObject.tag == "Player" && isEnemyBullet) {
			expire();
		}
	}
	private void expire(){
		if(!isEnemyBullet){
			GameManager.instance.addShots();
		}
		Destroy(gameObject);
	}
}
