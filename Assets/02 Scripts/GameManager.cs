using UnityEngine;
using System.Collections;


//Singleton para GameManager
public class GameManager : MonoBehaviour {

	public static GameManager instance;
	private HUDController hudController;

	private MeteorController meteorController;

	private Player player;
	public int MAX_LIVES = 3;

	private int lives = 0;
	public float multiplier = 1f;
	public int multiplierCount = 0;

	public AudioClip extraLifeClip;
	public int maxMultipCount = 5;
	[SerializeField]
	private int score = 0;
	private bool alive = true;
	private int level = 1;
	private Vector2 respawnPoint;

	private float scoreForLifeCounter;

	public int shotsLeft = 5;

	void Awake() {
		MakeSingleton ();
		lives = MAX_LIVES;
		scoreForLifeCounter = 4f;
	}

	private void MakeSingleton() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	public void assignPlayer(Player player){	
		this.player = player;
	}
	public void assignMeteorController(MeteorController meteorController){	
		this.meteorController = meteorController;
	}

	public void assignHUDController(HUDController hudController){
		this.hudController = hudController;
		this.hudController.updateLives(this.lives);
		this.hudController.updateScore(this.score);		
	}

	public int getLives(){
		return lives;
	}

	public void addLives(int lives){
		this.lives += lives ;
		hudController.updateLives(this.lives);
		if(lives < 0){ //Dying resets the multiplier
			clearMultiplier();
		}
	}

	public int getScore(){
		return score;
	}


	public void addScore(int points){
		score += (int) Mathf.Floor(points * multiplier);
		hudController.updateScore(score);
		addMultiplier();
		if(score > Mathf.Pow(10f,scoreForLifeCounter)){//Add a life every power of 10 > 1000
			addLives(1);
			scoreForLifeCounter++;
			SoundManager.instance.PlayPlayerOnce(extraLifeClip);
		}
	}


	private void addMultiplier(){
		if(multiplierCount == maxMultipCount)
			multiplierCount = 0;
		else
			multiplierCount++;
		multiplier += 0.25f;
		hudController.updateMultiplier((int)Mathf.Floor(multiplier));
	}

	public void clearMultiplier(){
		multiplierCount = 0;
		multiplier = 1;
		hudController.updateMultiplier(1);
	}

	public bool isAlive(){
		return this.alive;
	}

	public void setAlive(bool alive){
		this.alive = alive;
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

	public void endGame(){
		Time.timeScale = 0;
		hudController.setHiddenPanelVisible(true);//Show game over Screen
		SoundManager.instance.stopMusic ();
	}

	public void restartGame(){
		resetValues();
		this.level = -1;
		clearMeteors();
		resetScene();
	}

	public void continueGame(){//Keep playing from the current level
		resetValues();
		resetScene();
	}

	public void resetValues(){
		Time.timeScale = 1;				
		score = 0; 
		lives = MAX_LIVES; 
		alive = true;
		SoundManager.instance.changeMusic(0);
	}

	private void resetScene(){
		clearMultiplier();
		player.StopAllCoroutines();
		player.StartCoroutine (player.respawnPlayer ());
		hudController.updateLives(this.lives);
		hudController.updateScore(this.score);
		hudController.setHiddenPanelVisible(false);
	}

	private void clearMeteors(){
		 foreach (Transform child in meteorController.transform) {
     		GameObject.Destroy(child.gameObject);
 		}
	}

} 
