using UnityEngine;
using System.Collections;


//Singleton para GameManager
public class GameManager : MonoBehaviour {

	public static GameManager instance;

	private const int START_LIVES = 3;
	public int lives = 3;
	public float multiplier = 1f;
	public int multiplierCount = 0;
	public int maxMultipCount = 5;
	[SerializeField]
	private int score = 0;
	private bool ended = false;
	private bool alive = true;
	private int level = 1;
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

	public void addLives(int lives){
		this.lives += lives ;
		HUDController.instance.updateLives(this.lives);
	}

	public int getScore(){
		return this.score;
	}


	public void addScore(int points){
		this.score += (int) Mathf.Floor(points * multiplier);
		HUDController.instance.updateScore(score);
		addMultiplier();
	}


	private void addMultiplier(){
		if(multiplierCount == maxMultipCount)
			multiplierCount = 0;
		else
			multiplierCount++;
		multiplier += 0.25f;
		HUDController.instance.updateMultiplier((int)Mathf.Floor(multiplier));
	}

	public void clearMultiplier(){
		multiplierCount = 0;
		multiplier = 1;
		HUDController.instance.updateMultiplier(1);
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

	public int getLevel(){
		return this.level;
	}

	public void addLevel(){
		level++;
	}
} 
