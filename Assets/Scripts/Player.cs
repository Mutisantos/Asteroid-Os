using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MainInput;

public class Player :SpaceObject {

	public float xAxisThreshold = 0.02f;
	public float yAxisThreshold = 0.02f;
	public float moveSpeed;
	public float rotSpeed;
	public float maxSpeed;

	public float respawnInmunityTime = 2f;
	public float warpCooldown;

	private float inmunityTime;
	private Animator anim;
	private Rigidbody2D mybody;
	private Transform rotDirector;
	private float timeStamp;
	public MainInputManager mainInput;
	public AudioClip warpSound;
	public AudioClip dieSound;
	public AudioClip dieExplosion;

	// Use this for initialization
	void Awake () {
		mybody = GetComponent<Rigidbody2D> ();
		anim = GetComponentsInChildren<Animator> ()[0];
		rotDirector = GetComponentsInChildren<Transform>()[0];
		timeStamp = Time.time ;
		inmunityTime = respawnInmunityTime;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(inmunityTime > 0){
			inmunityTime -=Time.deltaTime;
		}
		if (GameManager.instance.isAlive ()) {//Los no me puedo seguir moviendo si me muero
			checkLimits();
			move ();
		}
	}

	private void move(){

		//Rotate
		Quaternion rot = rotDirector.transform.rotation;
		float z  = rot.eulerAngles.z;
		z -= mainInput.horizontal * rotSpeed * Time.deltaTime;
		rotDirector.transform.rotation = Quaternion.Euler(0,0,z);

		//Thrust
		float l = mainInput.vertical;
		if((l > yAxisThreshold)){
			Vector2 faceDirection = rotDirector.transform.TransformDirection(Vector2.up);
			mybody.AddForce( faceDirection * moveSpeed * l );
			anim.SetBool("moving",true);
		}
		else if (l < yAxisThreshold)
			anim.SetBool("moving",false);

		//Warp
		if (mainInput.downDown)
			warpShip();

	}



	private void warpShip(){
		if(timeStamp <= Time.time){
			SoundManager.instance.PlayOnce(warpSound);
			timeStamp = Time.time + warpCooldown;
			float randX = Random.Range(-HorzExtent,HorzExtent);
			float randY = Random.Range(-vertExtent,vertExtent);
			transform.position = new Vector2(randX,randY);
		}
	}
	private void OnTriggerEnter2D(Collider2D coll){
		
		if (coll.tag == "EnemyBody" && inmunityTime <= 0) {
			if (GameManager.instance.getLives () == 0) {
				anim.SetBool ("Alive", false);
				GameManager.instance.setAlive (false);
				GameManager.instance.setEnded (true);
				StartCoroutine (dyingEffects ());
				StartCoroutine (waitForEndGame (3f));
			}
			else {
				StartCoroutine (respawnPlayer ());
			}
			GameManager.instance.addLives (- 1);
		}
	}

	IEnumerator waitForEndGame(float seconds){
		yield return new WaitForSeconds (seconds);
		SoundManager.instance.changeMusic (3);
		SceneManager.LoadSceneAsync("03Fin");
	}

	IEnumerator dyingEffects(){
		SoundManager.instance.PlayPlayerOnce (this.dieSound);
		yield return new WaitForSeconds (0.5f);
		SoundManager.instance.PlayOnce (this.dieExplosion);
	}

	IEnumerator respawnPlayer(){
		
		anim.SetBool ("Alive", false);
		mybody.velocity = Vector2.zero;
		
		SoundManager.instance.PlayPlayerOnce (this.dieSound);
		yield return new WaitForSeconds (0.5f);
		SoundManager.instance.PlayOnce (this.dieExplosion);
		GameManager.instance.setAlive (false);
		yield return new WaitForSeconds (0.4f);
		anim.SetBool ("Alive", true);
		yield return new WaitForSeconds (0.05f);
		GameManager.instance.setAlive (true);

		transform.position = (GameManager.instance.getRespawnPoint ());
		transform.rotation = Quaternion.identity;
		inmunityTime = respawnInmunityTime;
	}
}
