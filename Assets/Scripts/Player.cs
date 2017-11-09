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

	public float warpCooldown;
	private Animator anim;
	private Rigidbody2D mybody;
	private Transform rotDirector;
	private float timeStamp;
	public MainInputManager mainInput;
	public GameObject collisionFX;

	public AudioClip warpSound;
	public AudioClip dieSound;
	public AudioClip dieExplosion;

	// Use this for initialization
	void Awake () {
		mybody = GetComponent<Rigidbody2D> ();
		anim = GetComponentsInChildren<Animator> ()[0];
		rotDirector = GetComponentsInChildren<Transform>()[0];
		collisionFX.SetActive (false);
		timeStamp = Time.time ;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
	//Trigger que determina si el jugador gana o pierde al contacto
	private void OnTriggerEnter2D(Collider2D coll){
		
		if (coll.tag == "EnemyBody") {
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
		}

		if (coll.tag == "Goal") {
			GameManager.instance.setAlive(true);
			GameManager.instance.setEnded(true);
			anim.SetBool ("Win", true);
			StartCoroutine (waitForEndGame(3f));
		}

	
	}

	private void OnCollisionEnter2D(Collision2D coll){
		if (coll.collider.tag == "Buildings") {
			StartCoroutine (collisionEffect (true));
		}
	}

	private void OnCollisionExit2D(Collision2D coll){
		if (coll.collider.tag == "Buildings") {
			StartCoroutine (collisionEffect (false));
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

	IEnumerator collisionEffect(bool state){
		collisionFX.SetActive (state);
		yield return new WaitForSeconds (0.5f);
	}

	IEnumerator respawnPlayer(){
		
		anim.SetBool ("Alive", false);
		SoundManager.instance.PlayPlayerOnce (this.dieSound);
		yield return new WaitForSeconds (0.5f);
		SoundManager.instance.PlayOnce (this.dieExplosion);
		GameManager.instance.setAlive (false);
		GameManager.instance.setEnded (true);
		yield return new WaitForSeconds (0.5f);
		anim.SetBool ("Alive", true);
		anim.SetFloat ("Speed", 2000f);
		yield return new WaitForSeconds (0.05f);
		anim.SetFloat ("Speed", 1f);
		GameManager.instance.setAlive (true);
		GameManager.instance.setEnded (false);
		transform.position = (GameManager.instance.getRespawnPoint ());

	}
}
