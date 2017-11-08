using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MainInput;

public class PlayerController : MonoBehaviour {

	public float xAxisThreshold = 0.02f;
	public float yAxisThreshold = 0.02f;
	public float speed;

	public float rotSpeed = 20f;

	public float maxSpeed;

	private Animator anim;
	private Rigidbody mybody;
	private Vector2 movement;

	private Transform rotDirector;

	public MainInputManager mainInput;
	public GameObject collisionFX;

	//Efectos de sonido para el jugador
	public AudioClip dieSound;
	public AudioClip dieExplosion;
	//Control del audio de caminada del personaje
	public float deltaStep = 1f;


	// Use this for initialization
	void Awake () {
		mybody = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		rotDirector = GetComponentsInChildren<Transform>()[0];
		collisionFX.SetActive (false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GameManager.instance.isAlive ()) {//Los no me puedo seguir moviendo si me muero
			move ();
		}
	}



	private void move(){

		//Rotate
		Quaternion rot = rotDirector.transform.rotation;
		float y  = rot.eulerAngles.y;
		y += mainInput.horizontal * rotSpeed * Time.deltaTime;
		rotDirector.transform.rotation = Quaternion.Euler(90,y,0);

		//Forward
		float l = mainInput.vertical;
		if((l > yAxisThreshold) && mybody.velocity.magnitude < maxSpeed){
			Vector3 faceDirection = rotDirector.transform.TransformDirection(Vector3.up);
			mybody.AddForce( faceDirection * speed * l );
			Debug.Log(faceDirection+"-"+speed+"-"+l);
		}
		else if (mybody.velocity.magnitude < yAxisThreshold)
			mybody.velocity = Vector2.zero;
		else if (l < -yAxisThreshold)


		Debug.Log(mybody.velocity.magnitude+"-"+l);
	}



	private void warpShip(){
		
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
