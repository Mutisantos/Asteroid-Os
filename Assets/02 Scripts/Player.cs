using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MainInput;

/**Script for Player movement actions and death conditions
* Esteban.Hernandez
*/
public class Player :SpaceObject {

	public MainInputManager mainInput;
	public AudioClip warpSound;
	public AudioClip dieSound;
	public AudioClip dieExplosion;	
	private Animator anim;
	private Rigidbody2D mybody;
	private Transform rotDirector;

	public float xAxisThreshold = 0.02f;
	public float yAxisThreshold = 0.02f;
	public float moveSpeed;
	public float rotSpeed;
	public float maxSpeed;
	public float respawnInmunityTime = 2f;
	public float warpCooldown;
	private float inmunityTime;
	private float timeStamp;

	void Awake () {
		mybody = GetComponent<Rigidbody2D> ();
		anim = GetComponentsInChildren<Animator> ()[0];
		rotDirector = GetComponentsInChildren<Transform>()[0];
		timeStamp = Time.time ;
		inmunityTime = respawnInmunityTime;
	}

	void Start(){
		GameManager.instance.assignPlayer(this);
	}
	
	void FixedUpdate () {
		if(inmunityTime > 0){
			inmunityTime -=Time.deltaTime;
			anim.SetBool("inmune",true);
		}
		else{
			anim.SetBool("inmune",false);
		}
		if (GameManager.instance.isAlive ()) {//Keep moving unless the player is dead
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
		if(l > yAxisThreshold){
			Vector2 faceDirection = rotDirector.transform.TransformDirection(Vector2.up);
			if(mybody.velocity.magnitude < maxSpeed){
				mybody.AddForce( faceDirection * moveSpeed * l );
				anim.SetBool("moving",true);
			}
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
			float vertExtent = BoundaryChecker.instance.VertExtent;
			float HorzExtent = BoundaryChecker.instance.HorzExtent;
			float randX = Random.Range(-HorzExtent,HorzExtent);
			float randY = Random.Range(-vertExtent,vertExtent);
			transform.position = new Vector2(randX,randY);
		}
	}
	private void OnTriggerEnter2D(Collider2D coll){	
		if (coll.tag == "EnemyBody" && inmunityTime <= 0 && GameManager.instance.isAlive()) {
			if (GameManager.instance.getLives () == 0) {
				StartCoroutine (dyingEffects ());
				StartCoroutine (waitForEndGame (2f));
			}
			else {
				GameManager.instance.addLives (-1);
				StartCoroutine (respawnPlayer ());
			}
		}
	}

	private IEnumerator waitForEndGame(float seconds){
		anim.SetBool ("alive", false);
		GameManager.instance.setAlive (false);
		yield return new WaitForSeconds (seconds);
		GameManager.instance.endGame();
	}

	private IEnumerator dyingEffects(){
		SoundManager.instance.PlayOnce (this.dieSound);
		yield return new WaitForSeconds (0.5f);
		SoundManager.instance.PlayOnce (this.dieExplosion);
	}

	public IEnumerator respawnPlayer(){	
		GameManager.instance.setAlive (false);	
		anim.SetBool ("alive", false);
		mybody.velocity = Vector2.zero;
		SoundManager.instance.PlayOnce (this.dieSound);
		yield return new WaitForSeconds (0.75f);
		SoundManager.instance.PlayOnce (this.dieExplosion);
		yield return new WaitForSeconds (0.1f);
		GameManager.instance.setAlive (false);
		yield return new WaitForSeconds (0.5f);
		anim.SetBool ("alive", true);
		GameManager.instance.setAlive (true);
		transform.position = (GameManager.instance.getRespawnPoint ());
		transform.rotation = Quaternion.identity;
		inmunityTime = respawnInmunityTime;
	}
}
