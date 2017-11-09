using UnityEngine;
using System.Collections;


//Singleton para GameManager
public class GameManager : MonoBehaviour {

	public static GameManager instance;

	private const int START_LIVES = 3;
	public int lives = 3;
	private int score = 0;
	private bool ended = false;
	private bool alive = true;
	[SerializeField]
	private Vector2 respawnPoint;

	public int shotsLeft = 5;

	void Awake() {
		MakeSingleton ();
	}

	private void MakeSingleton() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}


	public int getLives(){
		return this.lives;
	}

	public void setLives(int lives){
		this.lives = lives ;
	}

	public int getScore(){
		return this.score;
	}

	public void setScore(int score){
		this.score = score;
	}

	public void addScore(int points){
		this.score += points;
	}



	public bool isEnded(){
		return this.ended;
	}

	public void setEnded(bool ended){
		this.ended = ended;
	}

	public bool isAlive(){
		return this.alive;
	}

	public void setAlive(bool alive){
		this.alive = alive;
	}

	public void resetValues(){
		this.ended = false; 
		this.score = 0; 
		this.lives = START_LIVES; 
		this.alive = true;
	}


	public Vector2 getRespawnPoint(){
		return this.respawnPoint;
	}

	public void setRespawnPoint(Vector2 respawnPoint){
		this.respawnPoint = respawnPoint;
	}

	public void spendShot(){
		shotsLeft--;
	}

	public void addShots(){
		shotsLeft++;
	}


} 
